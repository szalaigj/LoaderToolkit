using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mergers
{
    public class Sread : Merger
    {
        public const string queryForRefIds = "SELECT"
                                           + " refID"
                                           + " FROM (SELECT DISTINCT [rname] FROM [$loaddb].[dbo].[$tablename]) ld"
                                           + " INNER JOIN [$targetdb].[dbo].[refDesc] d ON d.[extID] = ld.[rname]";

        protected override string SourceTableName
        {
            get { return "sreadLoad"; }
        }

        protected override string TargetTableName
        {
            get { return "sread"; }
        }

        protected override string GetOptionalPlaceholder(Chunk chunk, SqlConnection cn, SqlTransaction tn)
        {
            DateTime start = DateTime.Now;
            StringBuilder sql = new StringBuilder(queryForRefIds);
            sql.Replace("$loaddb", chunk.LoaderDB.InitialCatalog);
            sql.Replace("$targetdb", chunk.TargetDB.InitialCatalog);
            sql.Replace("$tablename", String.Format("{0}_{1}", chunk.ChunkId, SourceTableName));
            string refIDs = "";
            using (SqlCommand cmd = new SqlCommand(sql.ToString(), cn, tn))
            {
                cmd.CommandTimeout = 7200;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        refIDs += reader.GetString(0) + ",";
                    }
                }
            }
            // Remove last comma:
            refIDs = refIDs.Substring(0, refIDs.Length - 1);
            Console.WriteLine("{0} >     ... load different refIDs from {1} in {2} sec.", chunk.ID, SourceTableName, (DateTime.Now - start).TotalSeconds);
            return "(" + refIDs + ")";
        }
    }
}
