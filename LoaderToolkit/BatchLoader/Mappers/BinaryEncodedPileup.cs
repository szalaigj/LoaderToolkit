﻿using BinaryCodec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class BinaryEncodedPileup : BasePileup
    {
        public override void Map(string obj)
        {
            // the obj - which is read line in fact - is splitted by tab character:
            string[] objParts = obj.Split('\t');

            MapFirstToken(objParts[0]);

            var secondToken = objParts[1];
            // [refSeqID] [varchar](50) NOT NULL
            BulkWriter.WriteVarChar(secondToken, 50);

            var thirdToken = objParts[2];
            // [refSeqPos] [bigint] NOT NULL
            BulkWriter.WriteBigInt(Int64.Parse(thirdToken));

            var fourthToken = objParts[3];
            // [refNuc] [char] NOT NULL
            BulkWriter.WriteChar(fourthToken, 1);

            var fifthToken = objParts[4];
            // [coverage] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(fifthToken));

            var sixthToken = objParts[5];
            // [bases] [varbinary](8000) NOT NULL
            Dictionary<BinaryCodec.Constants.ColumnsFromSkipChars, string> byProductsBySkipChars;
            byte[] encodedBases = BinaryCodecUtil.ConvertBasesInputToEncodedBytes(sixthToken, out byProductsBySkipChars);
            BulkWriter.WriteVarBinary(encodedBases, 8000);

            if (objParts.Length == 7)
            {
                var seventhToken = objParts[6];
                // [basesQual] [varchar](8000) NULL
                BulkWriter.WriteVarChar(seventhToken, 8000);
            }
            else
            {
                BulkWriter.WriteVarChar(null, 8000);
            }

            var extraNuc = byProductsBySkipChars[BinaryCodec.Constants.ColumnsFromSkipChars.ExtraNuc];
            MapAdditionalColumn(extraNuc);

            var missingNuc = byProductsBySkipChars[BinaryCodec.Constants.ColumnsFromSkipChars.MissingNuc];
            MapAdditionalColumn(missingNuc);

            var startingSigns = byProductsBySkipChars[BinaryCodec.Constants.ColumnsFromSkipChars.StartingSigns];
            MapAdditionalColumn(startingSigns);

            var mappingQual = byProductsBySkipChars[BinaryCodec.Constants.ColumnsFromSkipChars.MappingQual];
            MapAdditionalColumn(mappingQual);

            var endingSigns = byProductsBySkipChars[BinaryCodec.Constants.ColumnsFromSkipChars.EndingSigns];
            MapAdditionalColumn(endingSigns);

            BulkWriter.EndLine();
        }

        private void MapAdditionalColumn(string additionalColumn)
        {
            // [additionalColumn] [varchar](8000) NULL (additionalColumn == exNuc || missNuc || startSg || mapQual || endSg)
            if ("".Equals(additionalColumn))
            {
                BulkWriter.WriteVarChar(null, 8000);
            }
            else
            {
                BulkWriter.WriteVarChar(additionalColumn, 8000);
            }
        }
    }
}
