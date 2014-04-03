using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using LoaderLibrary.Load;
using LoaderLibrary.CommandLineParser;
using LoaderLibrary.Services;
using StructureMap;

namespace BatchLoader.Verbs
{
    [Verb(Name = "Start", Description = "Starts processing of a batch")]
    class Start : Verb
    {
        private int batchID;
        private int batchSize;
        private int threads;
        private bool skip;
        private bool keepFiles;

        private Batch batch;

        private ChunkService<string> chunkService;

        [Parameter(Name = "BatchID", Description = "Batch ID to start.", Required = true)]
        public int BatchID
        {
            get { return batchID; }
            set { batchID = value; }
        }

        [Parameter(Name = "BatchSize", Description = "Batch size.")]
        public int BatchSize
        {
            get { return batchSize; }
            set { batchSize = value; }
        }

        [Parameter(Name = "Threads", Description = "Multithreaded execution.")]
        public int Threads
        {
            get { return threads; }
            set { threads = value; }
        }

        [Option(Name = "Skip", Description = "Skip already finished tasks.")]
        public bool SkipPrepare
        {
            get { return skip; }
            set { skip = value; }
        }

        [Option(Name = "Keep", Description = "Keep bulk-insert files.")]
        public bool KeepFiles
        {
            get { return keepFiles; }
            set { keepFiles = value; }
        }

        public Start()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.batchID = 0;
            this.batchSize = 32;
            this.threads = Environment.ProcessorCount;
            this.skip = false;
            this.keepFiles = false;
            this.chunkService = ObjectFactory.GetNamedInstance<ChunkService<string>>("WithAutoWiring");
        }

        public override void Run()
        {
            LoadBatch();

            var chunks = batch.GetChunksInOrder();

            if (chunks.Length == 0)
            {
                Console.WriteLine("No incomplete chunks found.");
                return;
            }

            for (int i = 0; i < chunks.Length; i++)
            {
                chunks[i].ID = i;
                chunks[i].Skip = skip;
            }

            chunks[0].DisableIndexes();

            int cstart = 0;

            while (cstart < chunks.Length)
            {
                int ccount = Math.Min(batchSize, chunks.Length - cstart);

                // Schedule chunks for load
                TaskScheduler qts = GetScheduler();
                Task[] tasks = new Task[ccount];

                // Prepare and Load
                for (int i = 0; i < ccount; i++)
                {
                    tasks[i] = new Task(PrepareAndLoad, chunks[cstart + i]);
                    tasks[i].Start(qts);
                }

                Task.WaitAll(tasks);

                // Merge
                // This has to been done sequentially
                //for (int i = 0; i < ccount; i++)
                //{
                //    Merge(chunks[cstart + i]);
                //}

                // Cleanup
                //for (int i = 0; i < ccount; i++)
                //{
                //    tasks[i] = new Task(Cleanup, chunks[cstart + i]);
                //    tasks[i].Start(qts);
                //}

                //Task.WaitAll(tasks);

                cstart += batchSize;
            }

            /*if (!no)
            {
                chunks[0].RebuildIndexes();
            }*/
        }

        private void LoadBatch()
        {
            using (var context = new DatabaseContext())
            {
                batch = new Batch();

                batch.BatchID = batchID;

                batch.Load(context);
                batch.LoadChunks(context);

                context.Commit();
            }
        }

        private TaskScheduler GetScheduler()
        {
            return new QueuedTaskScheduler(threads);
        }

        private void PrepareAndLoad(object state)
        {
            var chunk = (Chunk)state;

            chunk.PrepareStart = DateTime.Now;
            SaveChunk(chunk);
            chunkService.CreateFiles(chunk);
            SaveChunk(chunk);

            chunk.LoadStart = DateTime.Now;
            SaveChunk(chunk);
            chunkService.CreateTables(chunk);
            chunkService.RunBulkInsert(chunk);
            SaveChunk(chunk);
        }

        private void Merge(Chunk chunk)
        {
            chunk.MergeStart = DateTime.Now;
            SaveChunk(chunk);
            chunkService.MergeTables(chunk);
            SaveChunk(chunk);
        }

        private void Cleanup(object state)
        {
            var chunk = (Chunk)state;

            chunk.CleanupStart = DateTime.Now;
            SaveChunk(chunk);
            chunkService.DropTables(chunk);
            if (!keepFiles)
            {
                chunkService.DeleteFiles(chunk);
            }
            SaveChunk(chunk);
        }

        private void SaveChunk(Chunk chunk)
        {
            using (var context = new DatabaseContext())
            {
                batch.ModifyChunk((Chunk)chunk, context);

                context.Commit();
            }
        }
    }
}
