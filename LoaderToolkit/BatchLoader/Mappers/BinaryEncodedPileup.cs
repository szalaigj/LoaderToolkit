using BatchLoader.Services;
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
            // [run_id] [smallint] NOT NULL
            BulkWriter.WriteSmallInt(RunID);

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
            // [refNuc] [binary](1) NOT NULL
            byte[] encodedRefNuc = BinaryEncodingUtil.ConvertInputToEncodedBytes(fourthToken, EncoderDomainNames.RefNuc);
            BulkWriter.WriteBinary(encodedRefNuc, 1);

            var fifthToken = objParts[4];
            // [alignedReadsNO] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(fifthToken));

            var sixthToken = objParts[5];
            // [bases] [varbinary](8000) NOT NULL
            List<string> byProductsBySkipChars;
            byte[] encodedBases = BinaryEncodingUtil.ConvertBasesInputToEncodedBytes(sixthToken, out byProductsBySkipChars);
            BulkWriter.WriteVarBinary(encodedBases, 8000);

            if (objParts.Length == 7)
            {
                var seventhToken = objParts[6];
                // [basesQual] [varbinary](8000) NULL
                byte[] encodedBasesQual = BinaryEncodingUtil.ConvertInputToEncodedBytes(seventhToken, EncoderDomainNames.BasesQual);
                BulkWriter.WriteVarBinary(encodedBasesQual, 8000);
            }
            else
            {
                BulkWriter.WriteVarBinary(null, 8000);
            }

            var extraNuc = byProductsBySkipChars[0];
            MapAdditionalColumn(extraNuc);

            var startingSigns = byProductsBySkipChars[1];
            MapAdditionalColumn(startingSigns);

            var mappingQual = byProductsBySkipChars[2];
            MapAdditionalColumn(mappingQual);

            var endingSigns = byProductsBySkipChars[3];
            MapAdditionalColumn(endingSigns);
        }

        private void MapAdditionalColumn(string addtionalColumn)
        {
            // [addtionalColumn] [varchar](8000) NULL (addtionalColumn == extraNuc || startingSigns || mappingQual || endingSigns)
            if ("".Equals(addtionalColumn))
            {
                BulkWriter.WriteVarChar(null, 8000);
            }
            else
            {
                BulkWriter.WriteVarChar(addtionalColumn, 8000);
            }
        }
    }
}
