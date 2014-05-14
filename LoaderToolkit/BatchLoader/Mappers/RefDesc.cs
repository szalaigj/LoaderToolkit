using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class RefDesc : Mapper<string>
    {
        public override string TableName
        {
            get { return "refDesc"; }
        }

        public override void Map(string obj)
        {
            string[] objParts = obj.Split('\t');

            // The objParts[0] is skipped because the filename is irrelevant.
            // [refID] [int] NOT NULL PRIMARY KEY
            BulkWriter.WriteInt(Int32.Parse(objParts[1]));

            // [extID] [varchar](80) NOT NULL
            BulkWriter.WriteVarChar(objParts[2], 80);

            // [desc] [varchar](200) NULL
            BulkWriter.WriteVarChar(objParts[3], 200);

            BulkWriter.EndLine();
        }
    }
}
