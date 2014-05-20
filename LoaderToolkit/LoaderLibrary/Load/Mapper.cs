using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace LoaderLibrary.Load
{
    public abstract class Mapper<T>
    {
        private bool skipped;
        private bool binary;

        private FileStream outputStream = null;
        private BinaryWriter outputBinary = null;
        private TextWriter outputWriter = null;

        private BulkFileWriter bulkWriter = null;

        public abstract string PreferredSourceFileExt { get; }

        public bool Skipped
        {
            get { return skipped; }
            set { skipped = value; }
        }

        public bool Binary
        {
            get { return binary; }
            set { binary = value; }
        }

        public FileStream OutputStream
        {
            get { return outputStream; }
            set { outputStream = value; }
        }

        public BinaryWriter OutputBinary
        {
            get { return outputBinary; }
            set { outputBinary = value; }
        }

        public TextWriter OutputWriter
        {
            get { return outputWriter; }
            set { outputWriter = value; }
        }

        public BulkFileWriter BulkWriter
        {
            get { return bulkWriter; }
            set { bulkWriter = value; }
        }

        //public short RunID { get; set; }

        public abstract string TableName { get; }

        public abstract void Map(T obj);

        public string GetFilename(Chunk chunk)
        {
            var filename = Path.Combine(chunk.GetBulkDirectory(), TableName);
            filename += binary ? ".dat" : ".txt";

            return filename;
        }
    }
}
