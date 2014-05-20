using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    /// <summary>
    /// This class is only a stub for demonstration purpose
    /// </summary>
    class Tweet : Mapper<Dictionary<string, object>>
    {
        public override string TableName
        {
            get { return "tweet"; }
        }

        public override string PreferredSourceFileExt
        {
            // TODO: this file extension is unknown.
            get { return "UNKNOWN"; }
        }

        public override void Map(Dictionary<string, object> obj)
        {
            if (obj.ContainsKey("text"))
            {
                MapOne(obj);

                if (obj.ContainsKey("retweeted_status"))
                {
                    MapOne((Dictionary<string, object>)obj["retweeted_status"]);
                }
            }
        }

        private void MapOne(Dictionary<string, object> obj)
        {
            // [run_id] [smallint] NOT NULL
            //BulkWriter.WriteSmallInt(RunID);

            // The original code is removed because of dependency...
        }
    }
}
