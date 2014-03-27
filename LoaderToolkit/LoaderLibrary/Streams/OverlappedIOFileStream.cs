using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace LoaderLibrary.Streams
{
    /// <summary>
    /// Implements a large buffer around a forward-only, read or write only file
    /// that is accessed using asynchronous overlapped IO and no OS buffering.
    /// </summary>
    /// <remarks>
    /// The implementation uses a large read-ahead buffer to drive the IO
    /// subsystem to maximum.
    /// The IO method is optimal for large RAID systems which require a high
    /// number of outstanding IO requests to perform best.
    /// </remarks>
    public class OverlappedIOFileStream : Stream
    {
        #region DLLimport

        const int FILE_FLAG_NO_BUFFERING = unchecked((int)0x20000000);
        const int FILE_FLAG_OVERLAPPED = unchecked((int)0x40000000);
        const int FILE_FLAG_SEQUENTIAL_SCAN = unchecked((int)0x08000000);

        [DllImport("KERNEL32", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false)]
        static extern SafeFileHandle CreateFile(String fileName,
                                                   int desiredAccess,
                                                   System.IO.FileShare shareMode,
                                                   IntPtr securityAttrs,
                                                   System.IO.FileMode creationDisposition,
                                                   int flagsAndAttributes,
                                                   IntPtr templateFile);
        #endregion DLLimport

        #region Member variables

        /// <summary>
        /// Block size to be read directly from the disk
        /// </summary>
        private const int blockSize = 0x10000;    // 64k

        /// <summary>
        /// Number of block to read ahead
        /// </summary>
        private int blockMax = 0x400;     // 1k

        /// <summary>
        /// Number of blocks already read into the read-ahead buffer
        /// </summary>
        private int blockCount;

        /// <summary>
        /// Number of blocks in the entire file (minus the last, incomplete block)
        /// </summary>
        private long blockTotal;

        /// <summary>
        /// Number of outstanding IO operations
        /// </summary>
        private int asyncMax = 8;

        /// <summary>
        /// Events to synchronize outstanding IO
        /// </summary>
        private EventWaitHandle[] asyncWaitHandles;

        /// <summary>
        /// Read-ahead buffer (blockSize * blockMax)
        /// </summary>
        private byte[] buffer;

        private bool lastBlockReached;

        /// <summary>
        /// Contains the last, incomplete block when reading
        /// </summary>
        private byte[] lastBlockBuffer;

        /// <summary>
        /// Last positions
        /// </summary>
        private int bufferOffset;
        private int bufferEnd;


        private FileStream internalStream;

        #endregion
        #region Properties

        public override bool CanRead
        {
            get { return internalStream.CanRead; }
        }

        public override bool CanTimeout
        {
            get { return internalStream.CanTimeout; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return internalStream.CanWrite; }
        }

        public override long Length
        {
            get { return internalStream.Length; }
        }

        public override long Position
        {
            get
            {
                return internalStream.Position;   // TODO 
            }
            set { throw new NotImplementedException(); }
        }

        public override int ReadTimeout
        {
            get { return internalStream.ReadTimeout; }
            set { internalStream.ReadTimeout = value; }
        }

        public override int WriteTimeout
        {
            get { return internalStream.WriteTimeout; }
            set { internalStream.WriteTimeout = value; }
        }

        #endregion
        #region Constructors and initializers

        public OverlappedIOFileStream(string path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            switch (fileAccess)
            {
                case FileAccess.Read:
                    OpenForRead(path, fileMode, fileAccess, fileShare);
                    break;
                case FileAccess.Write:
                    break;
                default:
                    throw new ArgumentException("Overlapped IO files can only be opened for reading or writing.", "fileAccess");
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            internalStream.Dispose();
            FreeBuffer();
        }

        public new void Dispose()
        {
            base.Dispose();

            internalStream.Dispose();
            FreeBuffer();
        }

        #endregion

        private void OpenForRead(string path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            // Open file for normal read, get file size and the very last, incomplete block

            using (var f = new FileStream(path, fileMode, fileAccess, fileShare, blockSize))
            {
                // Check if the last block is complete
                blockTotal = f.Length / blockSize;
                if (f.Length > blockTotal * blockSize)
                {
                    // The very last block is incomplete, read it into a buffer
                    lastBlockBuffer = new byte[f.Length - blockTotal * blockSize];
                    f.Position = blockTotal * blockSize;
                    f.Read(lastBlockBuffer, 0, lastBlockBuffer.Length);
                }

                f.Close();
            }

            // Now open file for unbuffered, overlapped async IO
            if (blockTotal > 0)
            {
                internalStream = OpenUnbufferedOverlappedFile(path, fileMode, fileAccess, fileShare);

                AllocateBuffer();
                ReadAhead();
            }
        }

        private void OpenForWrite()
        {
            throw new NotImplementedException();
        }

        private FileStream OpenUnbufferedOverlappedFile(string path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            int flags = FILE_FLAG_NO_BUFFERING;     // default to simmple no buffering
            flags |= FILE_FLAG_SEQUENTIAL_SCAN;     // default to sequential scan only
            flags |= FILE_FLAG_OVERLAPPED;          // default to overlapped io

            var handle = CreateFile(path, (int)fileAccess, fileShare, IntPtr.Zero, fileMode, flags, IntPtr.Zero);

            if (!handle.IsInvalid)
            {
                return new FileStream(handle, fileAccess, blockSize, true);
            }
            else
            {
                throw new InvalidOperationException("File handle is invalid.");
            }
        }

        public override void Close()
        {
            internalStream.Close();
        }

        // --

        private void AllocateBuffer()
        {
            buffer = new byte[blockSize * blockMax];

            // Initialize wait handles
            asyncWaitHandles = new EventWaitHandle[asyncMax];
            for (int i = 0; i < asyncMax; i++)
            {
                asyncWaitHandles[i] = new ManualResetEvent(true);
            }
        }

        private void FreeBuffer()
        {
            buffer = null;

            if (asyncWaitHandles != null)
            {
                for (int i = 0; i < asyncMax; i++)
                {
                    asyncWaitHandles[i].Dispose();
                    asyncWaitHandles[i] = null;
                }
            }
        }

        #region Read functions

        public override int ReadByte()
        {
            return internalStream.ReadByte();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = 0;

            while (count > 0)
            {
                if (lastBlockReached)
                {
                    // see if there's anything in the last bytes
                    var remaining = Math.Min(bufferEnd - bufferOffset, count);
                    if (remaining > 0)
                    {
                        Buffer.BlockCopy(lastBlockBuffer, bufferOffset, buffer, offset, remaining);

                        read += remaining;
                        count -= remaining;
                        bufferOffset += remaining;

                        return read;
                    }
                    else
                    {
                        return 0;
                    }
                }

                if (bufferEnd == 0 && ReadAhead() == 0 && lastBlockBuffer != null)
                {
                    lastBlockReached = true;
                    bufferOffset = 0;
                    bufferEnd = lastBlockBuffer.Length;
                    continue;
                }
                else if (bufferOffset + count < bufferEnd)
                {
                    Buffer.BlockCopy(this.buffer, bufferOffset, buffer, offset, count);
                    read += count;
                    bufferOffset += count;
                    return read;
                }
                else if (bufferEnd - bufferOffset > 0)
                {
                    var remaining = bufferEnd - bufferOffset;
                    Buffer.BlockCopy(this.buffer, bufferOffset, buffer, offset, remaining);
                    read += remaining;
                    count -= remaining;
                    bufferOffset += remaining;
                    bufferEnd = 0;
                }
                else
                {
                    return 0;
                }
            }

            return read;
        }

        private int ReadAhead()
        {
            // Reset buffer position
            bufferOffset = 0;
            bufferEnd = 0;
            blockCount = 0;

            while (blockCount < blockMax && internalStream.Length - internalStream.Position > blockSize)
            {
                var i = WaitHandle.WaitAny(asyncWaitHandles);

                // Begin new read operation
                asyncWaitHandles[i].Reset();
                var ar = internalStream.BeginRead(buffer, blockCount * blockSize, blockSize, ReadAheadCallback, i);

                blockCount++;
            }

            // Wait for all uncompleted
            WaitHandle.WaitAll(asyncWaitHandles);

            bufferEnd = blockCount * blockSize;

            return bufferEnd;
        }

        private void ReadAheadCallback(IAsyncResult ar)
        {
            var i = (int)ar.AsyncState;
            internalStream.EndRead(ar);
            asyncWaitHandles[i].Set();
        }

        #endregion
        #region Write functions

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void WriteByte(byte value)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            // TODO
            throw new NotImplementedException();
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            // TODO
            throw new NotImplementedException();
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            // TODO
            throw new NotImplementedException();
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            // TODO
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            // TODO
            internalStream.Flush();
        }

        public void Flush(bool flushToDisk)
        {
            // TODO
            internalStream.Flush(flushToDisk);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
    }
}
