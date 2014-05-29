using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;

namespace LoaderLibrary.Load
{
   
    public abstract class Merger
    {
        //public short RunID { get; set; }
        protected abstract string SourceTableName { get; }
        protected abstract string TargetTableName { get; }

        protected virtual string GetOptionalPlaceholder(Chunk chunk, SqlConnection cn, SqlTransaction tn)
        {
            return null;
        }

        public void MergeTable(Chunk chunk, SqlConnection cn, SqlTransaction tn)
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("{0} > Merging table: {1}...", chunk.ID, TargetTableName);

            // Figure out script name to use
            PropertyInfo p = typeof(LoadScripts).GetProperty(String.Format("merge_{0}", TargetTableName), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.IgnoreCase);
            StringBuilder sql = new StringBuilder((string)p.GetValue(null, null));

            sql.Replace("$loaddb", chunk.LoaderDB.InitialCatalog);
            sql.Replace("$targetdb", chunk.TargetDB.InitialCatalog);
            sql.Replace("$tablename", String.Format("{0}_{1}", chunk.ChunkId, SourceTableName));
            var optPlaceholder = GetOptionalPlaceholder(chunk, cn, tn);
            if (optPlaceholder != null)
            {
                sql.Replace("$opt", optPlaceholder);
            }
            //sql.Replace("$run_id", chunk.RunId.ToString());

            using (SqlCommand cmd = new SqlCommand(sql.ToString(), cn, tn))
            {
                cmd.CommandTimeout = 7200;

                //cmd.Parameters.Add("@run_id", SqlDbType.SmallInt).Value = RunID;

                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("{0} >     ... merged {1} in {2} sec.", chunk.ID, TargetTableName, (DateTime.Now - start).TotalSeconds);
        }
    }
}
