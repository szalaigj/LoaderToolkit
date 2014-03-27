using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Load
{
    public class FileUtils<T>
    {
        public void Close(Mapper<T> mapper)
        {
            if (mapper.Binary)
            {
                mapper.OutputBinary.Flush();
                mapper.OutputBinary.Close();
                mapper.OutputBinary.Dispose();
                mapper.OutputBinary = null;
            }
            else
            {
                mapper.OutputWriter.Flush();
                mapper.OutputWriter.Close();
                mapper.OutputWriter.Dispose();
                mapper.OutputWriter = null;
            }

            /*mapper.OutputStream.Flush();
            mapper.OutputStream.Close();
            mapper.OutputStream.Dispose();
            mapper.OutputStream = null;*/
        }

        public void Delete(Chunk chunk, Mapper<T> mapper)
        {
            string fn = mapper.GetFilename(chunk);
            if (File.Exists(fn))
            {
                File.Delete(fn);
            }
        }

        public bool Open(Chunk chunk, bool skip, Mapper<T> mapper)
        {
            string dir = chunk.GetBulkDirectory();
            Directory.CreateDirectory(dir);

            string filename = mapper.GetFilename(chunk);

            if (skip && File.Exists(filename))
            {
                mapper.OutputWriter = null;
                mapper.Skipped = true;
            }
            else
            {
                mapper.OutputStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, Constants.WriteBufferSize);

                if (mapper.Binary)
                {
                    mapper.OutputBinary = new BinaryWriter(mapper.OutputStream);
                    mapper.BulkWriter = new BulkFileWriter(mapper.OutputBinary);
                }
                else
                {
                    mapper.OutputWriter = new StreamWriter(mapper.OutputStream, Encoding.Unicode);
                    mapper.BulkWriter = new BulkFileWriter(mapper.OutputWriter);
                }

                mapper.Skipped = false;
            }

            return mapper.Skipped;
        }
    }
}
