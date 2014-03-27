using LoaderLibrary.Load;
using LoaderLibrary.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Services
{
    public class DefaultBulkInsertFileCreator : BaseBulkInsertFileCreator<string>
    {
        public DefaultBulkInsertFileCreator(BaseStreamReaderForLoader<string> streamReaderForLoader,
            FileUtils<string> fileUtils, List<Mapper<string>> mappings)
            : base(streamReaderForLoader, fileUtils, mappings)
        {
        }
    }
}
