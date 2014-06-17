using BinaryCodec;
using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class SreadBin : BaseSread
    {
        public override string TableName
        {
            get { return "sreadBinLoad"; }
        }

        protected override void HandleSeq(string seq)
        {
            // The binary encoding works only even length:
            if ((seq.Length % 2) == 1)
	        {
		        seq += " ";
	        }
            byte[] encodedNucleotides = new byte[seq.Length / 2];
            for (int index = 0; index < seq.Length; index += 2)
            {
                var nucleotidePair = seq.Substring(index, 2);
                var encodedPair = BinaryNucleotideCodecUtil.GetEncodedPart(nucleotidePair);
                encodedNucleotides[index / 2] = encodedPair;
            }
            // [seq] [varbinary](512) NOT NULL
            BulkWriter.WriteBinary(encodedNucleotides, seq.Length / 2);
        }
    }
}
