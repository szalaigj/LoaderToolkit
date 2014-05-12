using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class Ref : Mapper<string>
    {
        public override string TableName
        {
            get { return "ref"; }
        }

        public override void Map(string obj)
        {
            string[] objParts = obj.Split('\t');

            // If the fileName contains char '_' the original file is chunked to a lot of files
            // so the file index - which should be the refID - is obtained by splitting.
            // And the original file name is irrelevant.
            string[] firstTokenParts = objParts[0].Split('_');

            long firstPosOfLine = long.Parse(objParts[1]);

            string partsOfNucSeq = objParts[2];

            char[] nucsOfLine = partsOfNucSeq.ToCharArray();

            for (int index = 0; index < 60; index++)
            {
                char nuc = nucsOfLine[index];

                // [refID] [int] NOT NULL PRIMARY KEY
                BulkWriter.WriteInt(Int32.Parse(firstTokenParts[1]));

                // [pos] [bigint] NOT NULL
                BulkWriter.WriteBigInt(firstPosOfLine);

                // [refNuc] [char] NULL
                BulkWriter.WriteNullableChar(nuc.ToString(), 1);

                BulkWriter.EndLine();
                firstPosOfLine++;
            }
        }
    }
}
