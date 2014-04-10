using LoaderLibrary.Load;
using LoaderLibrary.Streams;
using StructureMap;
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
        public DefaultBulkInsertFileCreator(FileUtils<string> fileUtils, List<Mapper<string>> mappings)
            : base(fileUtils, mappings)
        {
        }

        protected BaseStreamReaderForLoader<string> InstantiateReader()
        {
            return ObjectFactory.GetInstance<StreamReaderForLoaderWithPrefix>();
        }
    }
}
