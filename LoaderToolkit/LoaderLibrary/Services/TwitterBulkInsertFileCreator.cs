using LoaderLibrary.Load;
using LoaderLibrary.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Services
{
    public class TwitterBulkInsertFileCreator : BaseBulkInsertFileCreator<Dictionary<string, object>>
    {
        public TwitterBulkInsertFileCreator(BaseStreamReaderForLoader<Dictionary<string, object>> streamReaderForLoader,
            FileUtils<Dictionary<string, object>> fileUtils, List<Mapper<Dictionary<string, object>>> mappings)
            : base(streamReaderForLoader, fileUtils, mappings)
        {
        }
    }
}
