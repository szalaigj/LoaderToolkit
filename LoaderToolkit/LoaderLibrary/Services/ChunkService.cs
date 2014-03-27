using LoaderLibrary.Load;
using LoaderLibrary.Streams;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Services
{
    public class ChunkService<T>
    {
        private List<Mapper<T>> mappings;
        private List<Merger> mergers;
        private FileUtils<T> fileUtils;
        private SqlUtils<T> sqlUtils;
        private BaseBulkInsertFileCreator<T> bulkInsertFileCreator;

        // The following parameters will be set by wiring of StructureMap via LoaderRegistry.
        public ChunkService(List<Mapper<T>> mappings, List<Merger> mergers, FileUtils<T> fileUtils, SqlUtils<T> sqlUtils,
            BaseBulkInsertFileCreator<T> bulkInsertFileCreator)
        {
            this.mappings = mappings;
            this.mergers = mergers;
            this.fileUtils = fileUtils;
            this.sqlUtils = sqlUtils;
            this.bulkInsertFileCreator = bulkInsertFileCreator;
        }

        /// <summary>
        /// Checks if all files exist
        /// </summary>
        /// <returns></returns>
        public bool CheckFiles(Chunk chunk)
        {
            var res = true;

            foreach (var m in mappings)
            {
                res &= File.Exists(m.GetFilename(chunk));
            }

            return res;
        }

        /// <summary>
        /// Creates bulk insert files
        /// </summary>
        public void CreateFiles(Chunk chunk)
        {
            if (chunk.Skip && chunk.IsPrepareComplete && CheckFiles(chunk) || chunk.Skip && chunk.IsLoadComplete)
            {
                Console.WriteLine("{0} > Skip preparing: {1}", chunk.ID, Path.GetFileName(chunk.Filename));
                return;
            }
            bulkInsertFileCreator.CreateFiles(chunk);
        }

        /// <summary>
        /// Creates tables for bulk insert in loader db
        /// </summary>
        public void CreateTables(Chunk chunk)
        {
            if (chunk.Skip && chunk.IsLoadComplete)
            {
                Console.WriteLine("{0} > Skip creating table: {1}", chunk.ID, Path.GetFileName(chunk.Filename));
                return;
            }

            using (SqlConnection cn = new SqlConnection(chunk.LoaderDB.ConnectionString))
            {
                cn.Open();

                using (SqlTransaction tn = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    foreach (Mapper<T> m in mappings)
                    {
                        sqlUtils.DropTable(chunk, cn, tn, m);
                        sqlUtils.CreateTable(chunk, cn, tn, m);
                    }

                    tn.Commit();
                }
            }
        }

        /// <summary>
        /// Executes bulk insert
        /// </summary>
        public void RunBulkInsert(Chunk chunk)
        {
            if (chunk.Skip && chunk.IsLoadComplete)
            {
                Console.WriteLine("{0} > Skip loading: {1}", chunk.ID, Path.GetFileName(chunk.Filename));
                return;
            }

            using (SqlConnection cn = new SqlConnection(chunk.LoaderDB.ConnectionString))
            {
                cn.Open();

                foreach (Mapper<T> m in mappings)
                {
                    using (SqlTransaction tn = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        sqlUtils.BulkInsert(chunk, cn, tn, m);

                        tn.Commit();
                    }

                    using (SqlTransaction tn = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        sqlUtils.CreateIndex(chunk, cn, tn, m);

                        tn.Commit();
                    }
                }
            }

            // Save status
            chunk.LoadEnd = DateTime.Now;
        }

        /// <summary>
        /// Merges tables into main db
        /// </summary>
        public void MergeTables(Chunk chunk)
        {
            if (chunk.Skip && chunk.IsMergeComplete)
            {
                Console.WriteLine("{0} > Skip merging: {1}", chunk.ID, Path.GetFileName(chunk.Filename));
                return;
            }

            using (SqlConnection cn = new SqlConnection(chunk.LoaderDB.ConnectionString))
            {
                cn.Open();

                foreach (Merger m in mergers)
                {
                    m.RunID = chunk.RunId;

                    using (SqlTransaction tn = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        m.MergeTable(chunk, cn, tn);

                        tn.Commit();
                    }
                }
            }


            // Save status
            chunk.MergeEnd = DateTime.Now;
        }

        /// <summary>
        /// Drops loader tables
        /// </summary>
        public void DropTables(Chunk chunk)
        {
            using (SqlConnection cn = new SqlConnection(chunk.LoaderDB.ConnectionString))
            {
                cn.Open();

                foreach (Mapper<T> m in mappings)
                {
                    using (SqlTransaction tn = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        sqlUtils.DropTable(chunk, cn, tn, m);

                        tn.Commit();
                    }
                }
            }

            // Save status
            chunk.CleanupEnd = DateTime.Now;
        }

        /// <summary>
        /// Deletes bulk insert files
        /// </summary>
        public void DeleteFiles(Chunk chunk)
        {
            foreach (Mapper<T> m in mappings)
            {
                fileUtils.Delete(chunk, m);
            }

            string dir = chunk.GetBulkDirectory();
            if (Directory.Exists(dir))
            {
                try
                {
                    System.IO.Directory.Delete(dir);
                }
                catch (Exception)
                {
                }
            }

            // Save status
            chunk.CleanupEnd = DateTime.Now;
        }
    }
}
