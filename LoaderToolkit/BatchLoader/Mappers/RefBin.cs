using BinaryCodec;
using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class RefBin : Mapper<string>
    {
        public override string TableName
        {
            get { return "refBin"; }
        }

        public override string PreferredSourceFileExt
        {
            get { return ".fao"; }
        }

        public override void Map(string obj)
        {
            int incrementOfPosStart = 256;
            string[] objParts = obj.Split('\t');

            // If the fileName contains char '_' the original file is chunked to a lot of files
            // so the file index - which should be the refID - is obtained by splitting.
            // And the original file name is irrelevant.
            string[] firstTokenParts = objParts[0].Split('_');

            // [refID] [int] NOT NULL PRIMARY KEY
            BulkWriter.WriteInt(Int32.Parse(firstTokenParts[firstTokenParts.Length - 1]));

            long firstPosOfLine = long.Parse(objParts[1]);

            // [posStart] [bigint] NOT NULL
            BulkWriter.WriteBigInt(firstPosOfLine);

            string partsOfNucSeq = objParts[2];

            byte[] encodedNucleotides = new byte[incrementOfPosStart];
            for (int index = 0; index < incrementOfPosStart * 2; index += 2)
            {
                var nucleotidePair = partsOfNucSeq.Substring(index, 2);
                var encodedPair = BinaryNucleotideCodecUtil.GetEncodedPart(nucleotidePair);
                encodedNucleotides[index / 2] = encodedPair;
            }

            // [seqBlock] [binary](incrementOfPosStart) NOT NULL
            BulkWriter.WriteBinary(encodedNucleotides, incrementOfPosStart);
        }
    }
}
