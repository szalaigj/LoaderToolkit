using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Streams
{
    public class StreamReaderForLoaderWithPrefix : DefaultStreamReaderForLoader
    {
        public StreamReaderForLoaderWithPrefix(ISelector<string> selector)
            : base(selector)
        {
        }

        public override IEnumerable<string> selectObjects(Chunk chunk)
        {
            // The name of source file (without path, extensions and file name suffix) is passed to Selector
            // which will concatenate this string for the beginning of the read lines.
            var fileName = Path.GetFileNameWithoutExtension(chunk.Filename);
            var fileSuffix = chunk.FileSuffix;
            if ((fileSuffix != null) && (fileName.EndsWith(fileSuffix)))
            {
                fileName = fileName.Substring(0, fileName.Length - fileSuffix.Length);
            }
            Selector.Prefix = fileName;
            return this.Select(Selector.selectObjects);
        }
    }
}
