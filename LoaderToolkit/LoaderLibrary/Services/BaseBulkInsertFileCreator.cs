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
    public abstract class BaseBulkInsertFileCreator<T>
    {
        private List<Mapper<T>> mappings;
        private FileUtils<T> fileUtils;

        public List<Mapper<T>> Mappings
        {
            get { return mappings; }
        }
        
        public FileUtils<T> FileUtils
        {
            get { return fileUtils; }
        }

        public BaseBulkInsertFileCreator(FileUtils<T> fileUtils, List<Mapper<T>> mappings)
        {
            this.fileUtils = fileUtils;
            this.mappings = mappings;
        }

        public void CreateFiles(Chunk chunk)
        {
            var start = DateTime.Now;
            Console.WriteLine("{0} > Preparing: {1}", chunk.ID, Path.GetFileName(chunk.Filename));

            bool skipall = true;
            foreach (Mapper<T> m in Mappings)
            {
                m.Binary = chunk.Binary;
                //m.RunID = chunk.RunId;
                skipall &= FileUtils.Open(chunk, chunk.Skip, m);
            }

            int q = 0;
            if (!skipall)
            {
                using (var reader = InitializeReader(chunk))
                {
                    //var objects = reader.AsParallel().WithMergeOptions(ParallelMergeOptions.NotBuffered).WithDegreeOfParallelism(Environment.ProcessorCount).Select(
                    var objects = reader.selectObjects(chunk);

                    q = DoMapOnObjects(q, objects);

                    foreach (Mapper<T> m in Mappings)
                    {
                        if (!m.Skipped)
                        {
                            FileUtils.Close(m);
                        }
                    }
                }
            }

            var end = DateTime.Now;
            Console.WriteLine("{0} > {1} rows written in {2} sec.", chunk.ID, q, (end - start).TotalSeconds);

            // Save status
            chunk.PrepareEnd = end;
        }

        protected BaseStreamReaderForLoader<T> InitializeReader(Chunk chunk)
        {
            var streamReaderForLoader = InstantiateReader();
            var compressed = chunk.Filename.EndsWith(".gz", StringComparison.InvariantCultureIgnoreCase);
            streamReaderForLoader.InitializeInputStream(chunk.Filename, chunk.Overlapped, compressed);
            return streamReaderForLoader;
        }

        protected abstract BaseStreamReaderForLoader<T> InstantiateReader();

        protected int DoMapOnObjects(int q, IEnumerable<T> objects)
        {
            foreach (var obj in objects)
            {
                if (obj != null)
                {
                    foreach (Mapper<T> m in Mappings)
                    {
                        if (!m.Skipped)
                        {
                            m.Map(obj);
                        }
                    }
                    q++;

                    /*if (q % 1000 == 0)
                    {
                        Console.WriteLine(q);
                    }*/
                }
            }
            return q;
        }
    }
}
