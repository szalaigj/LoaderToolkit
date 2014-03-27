using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Streams
{
    public class CommonStreamReaderForLoader<T> : BaseStreamReaderForLoader<T>
    {
        private Stream inputStream;
        private GZipStream inputZipStream;
        private bool compressed;
        private bool ownsInputStream;

        private Encoding encoding;
        private Decoder decoder;

        private byte[] byteBuffer;

        private char[] charBuffer;
        private int charPos;
        private int charLen;

        public CommonStreamReaderForLoader(ISelector<T> selector)
            : base (selector)
        {
            InitializeMembers();
        }

        protected void InitializeMembers()
        {
            this.inputStream = null;
            this.inputZipStream = null;
            this.ownsInputStream = false;

            // Twitter streams are always encoded as ASCII
            // special characters are escaped using the JSON format
            this.encoding = ASCIIEncoding.ASCII;
            this.decoder = encoding.GetDecoder();

            // Byte buffer to read from the stream
            this.byteBuffer = new byte[0x10000];        //************************

            // Character buffer to convert from the byte buffer
            this.charBuffer = new char[encoding.GetMaxCharCount(byteBuffer.Length)];
            this.charPos = 0;
            this.charLen = 0;
        }

        public override void InitializeInputStream(string path, bool overlapped, bool compressed)
        {
            if (overlapped)
            {
                this.inputStream = new OverlappedIOFileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else
            {
                this.inputStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            if (compressed)
            {
                this.inputZipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            }

            this.ownsInputStream = true;
            this.compressed = compressed;
        }

        public override void InitializeInputStream(Stream stream)
        {
            inputStream = stream;
            ownsInputStream = false;
        }

        protected override void Close()
        {
            if (ownsInputStream && inputStream != null)
            {
                inputStream.Close();
                inputStream.Dispose();
                inputStream = null;
            }

            if (inputZipStream != null)
            {
                inputZipStream.Close();
                inputZipStream.Dispose();
                inputZipStream = null;
            }
        }

        public override string ReadLine()
        {
            if (inputStream == null)
            {
                return null;
            }

            // No more data in the buffer, try read or end of file
            if (charPos == charLen && ReadBuffer() == 0)
            {
                Close();
                return null;
            }

            // Use a string builder if required
            StringBuilder builder = null;

            do
            {

                int pos = this.charPos;
                do
                {
                    char ch = this.charBuffer[pos];

                    // Twitter uses \r to delimit status messages
                    // If a newline is reached, return it
                    switch (ch)
                    {
                        case '\r':
                            string str;

                            if (builder != null)
                            {
                                builder.Append(charBuffer, charPos, pos - charPos);
                                str = builder.ToString();
                            }
                            else
                            {
                                str = new string(charBuffer, charPos, pos - charPos);
                            }

                            // Update buffer position
                            charPos = pos + 1;
                            return str;
                    }
                    pos++;
                }
                while (pos < charLen);

                // If the entire buffer belongs to the same line, initialize a StringBuilder
                // so concatenation will be optimized
                int len = charLen - charPos;
                if (builder == null)
                {
                    builder = new StringBuilder(len + 80);
                }

                builder.Append(this.charBuffer, charPos, len);
            }
            while (this.ReadBuffer() > 0);

            return builder.ToString();
        }

        public override IEnumerable<T> selectObjects(Chunk chunk)
        {
            return this.Select(Selector.selectObjects);
        }

        private int ReadBuffer()
        {
            var stream = (Stream)(compressed ? inputZipStream : inputStream);

            // This function only works with single or double byte encoders
            // Encodings which vary the number of bytes per character won't work

            // Fill in the buffer from the stream
            int num = stream.Read(byteBuffer, 0, byteBuffer.Length);

            // Decode bytes and write into charater buffer
            charPos = 0;
            charLen = decoder.GetChars(byteBuffer, 0, num, charBuffer, 0);

            return charLen;
        }
    }
}
