using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public abstract class BasePileup : Mapper<string>
    {
        public override string TableName
        {
            get { return "pileup"; }
        }

        protected void MapFirstToken(string firstToken)
        {
            // e.g.: firstToken = "CPA1_a"
            int posOfFirstToken = 0;
            while (Char.IsLetter(firstToken, posOfFirstToken))
            {
                posOfFirstToken++;
            }
            var sampleGroup = firstToken.Substring(0, posOfFirstToken);
            // [sampleGroup] [varchar](8) NOT NULL
            BulkWriter.WriteVarChar(sampleGroup, 8);

            int startPosOfSampleID = posOfFirstToken;
            while (Char.IsNumber(firstToken, posOfFirstToken))
            {
                posOfFirstToken++;
            }
            var sampleID = firstToken.Substring(startPosOfSampleID, posOfFirstToken);
            // [sampleID] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(sampleID));

            // Note: there is "+ 1" in the following because "_" should be skipped.
            var lane = firstToken.Substring(posOfFirstToken + 1);
            // [lane] [char] NOT NULL
            BulkWriter.WriteChar(lane, 1);
        }
    }
}
