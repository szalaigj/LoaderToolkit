using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class Cover : Mapper<string>
    {
        public override string TableName
        {
            get { return "coverLoad"; }
        }

        public override string PreferredSourceFileExt
        {
            get { return ".ucount"; }
        }

        public override void Map(string obj)
        {
            string[] objParts = obj.Split('\t');

            string[] firstTokenParts = objParts[0].Split('_');
            // [sampleName] [varchar](16) NOT NULL
            BulkWriter.WriteVarChar(firstTokenParts[0], 16);

            // [speciesID] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(firstTokenParts[1]));

            // [extID] [varchar](80) NOT NULL
            BulkWriter.WriteVarChar(objParts[1], 80);

            // [pos] [bigint] NOT NULL
            BulkWriter.WriteBigInt(Int64.Parse(objParts[2]));

            // [refNuc] [char] NOT NULL
            BulkWriter.WriteChar(objParts[3], 1);

            // [coverage] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(objParts[4]));

            // [Acount] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(objParts[5]));

            // [Ccount] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(objParts[6]));

            // [Gcount] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(objParts[7]));

            // [Tcount] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(objParts[8]));

            // [incount] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(objParts[9]));

            // [delcount] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(objParts[10]));

            BulkWriter.EndLine();
        }
    }
}
