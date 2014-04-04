using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Streams
{
    public class TwitterStreamReaderForLoader : CommonStreamReaderForLoader<Dictionary<string, object>>
    {
        public TwitterStreamReaderForLoader(ISelector<Dictionary<string, object>> selector)
            : base(selector)
        {
            // Twitter uses \r to delimit status messages
            this.lineDelimiterChar = "\r";
        }
    }
}
