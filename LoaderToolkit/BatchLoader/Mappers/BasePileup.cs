﻿using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public abstract class BasePileup : Mapper<string>
    {
        public override string TableName
        {
            get { return "pupLoad"; }
        }

        public override string PreferredSourceFileExt
        {
            get { return ".pup"; }
        }

        protected virtual void MapFirstToken(string firstToken)
        {
            // [sampleName] [varchar](16) NOT NULL
            BulkWriter.WriteVarChar(firstToken, 16);

            string pattern = @"^[a-zA-Z]+[0-9]+_[a-zA-Z]{1}$";
            var isSampleNameRegular = Regex.IsMatch(firstToken, pattern);

            if (isSampleNameRegular)
            {
                // e.g.: firstToken = "CPA1_a"
                int posOfFirstToken = 0;
                while (Char.IsLetter(firstToken, posOfFirstToken))
                {
                    posOfFirstToken++;
                }
                var sampleGroup = firstToken.Substring(0, posOfFirstToken);
                // [sampleGroup] [varchar](8) NULL
                BulkWriter.WriteVarChar(sampleGroup, 8);

                int startPosOfSampleExtID = posOfFirstToken;
                while (Char.IsNumber(firstToken, posOfFirstToken))
                {
                    posOfFirstToken++;
                }
                var sampleExtID = firstToken.Substring(startPosOfSampleExtID, posOfFirstToken - startPosOfSampleExtID);
                // [sampleExtID] [int] NULL
                BulkWriter.WriteNullableInt(Int32.Parse(sampleExtID));

                // Note: there is "+ 1" in the following because "_" should be skipped.
                var lane = firstToken.Substring(posOfFirstToken + 1);
                // [lane] [char] NULL
                BulkWriter.WriteNullableChar(lane, 1);
            }
            else
            {
                // [sampleGroup] [varchar](8) NULL
                BulkWriter.WriteVarChar(null, 8);

                // [sampleExtID] [int] NULL
                BulkWriter.WriteNullableInt(null);

                // [lane] [char] NULL
                BulkWriter.WriteNullableChar(null, 1);
            }
        }
    }
}
