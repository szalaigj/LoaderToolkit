using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class BasesDist : Mapper<string>
    {
        public override string TableName
        {
            get { return "basesDistLoad"; }
        }

        public override string PreferredSourceFileExt
        {
            get { return ".mutTrip"; }
        }

        public override void Map(string obj)
        {
            // the obj - which is read line in fact - is splitted by tab or space character: 
            // Note: not all whitespace because e.g. line end char is white space char also...
            char[] tabOrSpace = new char[] { ' ', '\t' };
            string[] objParts = obj.Split(tabOrSpace);

            // If the fileName contains char '_' the original file is chunked to a lot of files
            // so the original file name - which should be the sample name - is obtained by splitting.
            string[] firstTokenParts = objParts[0].Split('_');
            // [sampleName] [varchar](50) NOT NULL
            BulkWriter.WriteVarChar(firstTokenParts[0], 50);

            // [chr] [varchar](20) NOT NULL
            BulkWriter.WriteVarChar(objParts[1], 20);

            // [pos] [bigint] NOT NULL
            BulkWriter.WriteBigInt(Int64.Parse(objParts[2]));

            // [refNuc] [char] NOT NULL
            BulkWriter.WriteChar(objParts[3], 1);

            // [A] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(objParts[4]));

            // [C] [int] NOT NULL
            // Note: the 'G' is followed by 'C' in the source file
            BulkWriter.WriteInt(Int32.Parse(objParts[6]));

            // [G] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(objParts[5]));

            // [T] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(objParts[7]));

            // [triplet] [varchar](100) NOT NULL
            BulkWriter.WriteVarChar(objParts[8], 100);

            BulkWriter.EndLine();
        }
    }
}
