using LoaderLibrary.Load;
using LoaderLibrary.Streams;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Services
{
    public class TwitterBulkInsertFileCreator : BaseBulkInsertFileCreator<Dictionary<string, object>>
    {
        public TwitterBulkInsertFileCreator(FileUtils<Dictionary<string, object>> fileUtils, 
            List<Mapper<Dictionary<string, object>>> mappings)
            : base(fileUtils, mappings)
        {
        }

        protected override BaseStreamReaderForLoader<Dictionary<string, object>> InstantiateReader()
        {
            return ObjectFactory.GetInstance<TwitterStreamReaderForLoader>();
        }
    }
    
}
