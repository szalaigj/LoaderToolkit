using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class Gtf : Mapper<string>
    {
        public override string TableName
        {
            get { return "gtf"; }
        }

        public override string PreferredSourceFileExt
        {
            get { return ".gtf"; }
        }

        public override void Map(string obj)
        {
            string[] objParts = obj.Split('\t');
            // The objParts[0] is skipped because the filename is irrelevant.

            // The following is needed to skip the first header lines from gtf files:
            if (objParts.Length == 10)
            {
                // [seqname] [varchar](150) NOT NULL
                BulkWriter.WriteVarChar(objParts[1], 150);

                // [source] [varchar](150) NOT NULL
                BulkWriter.WriteVarChar(objParts[2], 150);

                // [feature] [varchar](150) NOT NULL
                BulkWriter.WriteVarChar(objParts[3], 150);

                // [start] [bigint] NOT NULL
                BulkWriter.WriteBigInt(Int64.Parse(objParts[4]));

                // [end] [bigint] NOT NULL
                BulkWriter.WriteBigInt(Int64.Parse(objParts[5]));

                // [score] [varchar](50) NOT NULL
                BulkWriter.WriteVarChar(objParts[6], 50);

                // [strand] [char] NOT NULL
                BulkWriter.WriteChar(objParts[7], 1);

                // [frame] [tinyint] NOT NULL
                BulkWriter.WriteTinyInt(Byte.Parse(objParts[8]));

                // [attribute] [varchar](8000) NOT NULL
                BulkWriter.WriteVarChar(objParts[9], 8000);

                BulkWriter.EndLine();   
            }
        }
    }
}
