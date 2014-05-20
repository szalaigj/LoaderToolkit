using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class Sam : Mapper<string>
    {
        public override string TableName
        {
            get { return "sam"; }
        }

        public override string PreferredSourceFileExt
        {
            get { return ".hdr"; }
        }

        public override void Map(string obj)
        {
            string[] objParts = obj.Split('\t');

            // The objParts[0] is skipped because the filename is irrelevant.
            // [samID] [int] NOT NULL PRIMARY KEY
            BulkWriter.WriteInt(Int32.Parse(objParts[1]));

            // [line] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(objParts[2]));

            // [type] [varchar](2) NOT NULL
            BulkWriter.WriteVarChar(objParts[3], 2);

            int endOfObjPartsLen = objParts.Length - 4;
            string[] endOfObjParts = new string[endOfObjPartsLen];

            Array.Copy(objParts, 4, endOfObjParts, 0, endOfObjPartsLen);
            string tags = String.Join("\t", endOfObjParts);

            // [tags] [varchar](8000) NOT NULL
            BulkWriter.WriteVarChar(tags, 8000);

            BulkWriter.EndLine();
        }
    }
}
