using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Load
{
    public class SqlUtils<T>
    {
        private StringBuilder GetScript(string id)
        {
            try
            {
                PropertyInfo p = typeof(LoadScripts).GetProperty(id, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.IgnoreCase);
                return new StringBuilder((string)p.GetValue(null, null));
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error scripting {0}.", id), ex);
            }
        }

        public void CreateTable(Chunk chunk, SqlConnection cn, SqlTransaction tn, Mapper<T> mapper)
        {
            // Figure out script name to use
            StringBuilder sql = GetScript(String.Format("create_{0}", mapper.TableName));
            ReplaceNames(sql, chunk, mapper);

            using (SqlCommand cmd = new SqlCommand(sql.ToString(), cn, tn))
            {
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("{0} > Created table: {1}...", chunk.ID, mapper.TableName);
        }

        public void BulkInsert(Chunk chunk, SqlConnection cn, SqlTransaction tn, Mapper<T> mapper)
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("{0} > Loading table: {1}...", chunk.ID, mapper.TableName);

            try
            {

                // Figure out script name to use
                StringBuilder sql = new StringBuilder(mapper.Binary ? LoadScripts.bulkinsert_binary : LoadScripts.bulkinsert);
                ReplaceNames(sql, chunk, mapper);

                using (SqlCommand cmd = new SqlCommand(sql.ToString(), cn, tn))
                {
                    //the command timeout limit is one day:
                    cmd.CommandTimeout = 86400;
                    cmd.ExecuteNonQuery();
                }

                var end = DateTime.Now;
                Console.WriteLine("{0} >     ... loaded {1} in {2} sec.", chunk.ID, mapper.TableName, (end - start).TotalSeconds);

                // Save status
                chunk.LoadStart = chunk.LoadStart == DateTime.MinValue ? start : chunk.LoadStart;
                chunk.PrepareEnd = end;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} >     ... error in loading {1}.", chunk.ID, mapper.TableName);
                Console.WriteLine("{0} > {1}", chunk.ID, ex.Message);
                throw;
            }
        }

        public void CreateIndex(Chunk chunk, SqlConnection cn, SqlTransaction tn, Mapper<T> mapper)
        {
            // Figure out script name to use
            StringBuilder sb = GetScript(String.Format("index_{0}", mapper.TableName));
            ReplaceNames(sb, chunk, mapper);

            var sql = sb.ToString();

            if (!String.IsNullOrWhiteSpace(sql))
            {

                DateTime start = DateTime.Now;
                Console.WriteLine("{0} > Creating index on table: {1}...", chunk.ID, mapper.TableName);

                using (SqlCommand cmd = new SqlCommand(sql, cn, tn))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("{0} >     ... index created on {1} in {2} sec.", chunk.ID, mapper.TableName, (DateTime.Now - start).TotalSeconds);
            }
        }

        public void DropTable(Chunk chunk, SqlConnection cn, SqlTransaction tn, Mapper<T> mapper)
        {
            var sql = new StringBuilder(LoadScripts.drop_table);
            ReplaceNames(sql, chunk, mapper);

            using (SqlCommand cmd = new SqlCommand(sql.ToString(), cn, tn))
            {
                cmd.CommandTimeout = 3600;
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("{0} > Dropped table: {1}...", chunk.ID, mapper.TableName);
        }

        private void ReplaceNames(StringBuilder sql, Chunk chunk, Mapper<T> mapper)
        {
            sql.Replace("$dbname", chunk.LoaderDB.InitialCatalog);
            sql.Replace("$tablename", String.Format("{0}_{1}", chunk.ChunkId, mapper.TableName));
            sql.Replace("$ixname", String.Format("IX_{0}_{1}", chunk.ChunkId, mapper.TableName));
            sql.Replace("$filename", mapper.GetFilename(chunk));
        }
    }
}
