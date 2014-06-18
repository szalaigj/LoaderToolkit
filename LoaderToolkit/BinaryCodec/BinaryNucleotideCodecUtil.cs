using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCodec
{
    public static class BinaryNucleotideCodecUtil
    {
        public static readonly Dictionary<string, byte> encodedNucleotidePairs;
        public static readonly Dictionary<int, string> decodedNucleotides;

        static BinaryNucleotideCodecUtil()
        {
            encodedNucleotidePairs = new Dictionary<string, byte>();
            Dictionary<string, int> encodedNucleotides = new Dictionary<string, int>() 
            { 
                {"A",0x8}, 
                {"C",0x9}, 
                {"G",0xA}, 
                {"T",0xB}, 
                {"N",0xC},
                {"R",0xC},
                {"Y",0xC},
                {"K",0xC},
                {"M",0xC},
                {"S",0xC},
                {"W",0xC},
                {"B",0xC},
                {"D",0xC},
                {"H",0xC},
                {"V",0xC},
                {" ",0X4} 
            };
            foreach (var firstNucleotideEntry in encodedNucleotides)
            {
                foreach (var lastNucleotideEntry in encodedNucleotides)
                {
                    string currentNucleotidePair = firstNucleotideEntry.Key + lastNucleotideEntry.Key;
                    int encodedNucleotidePair = (firstNucleotideEntry.Value << 4) + lastNucleotideEntry.Value;
                    encodedNucleotidePairs.Add(currentNucleotidePair, (byte)encodedNucleotidePair);
                }
            }
            decodedNucleotides = new Dictionary<int, string>() 
            { 
                {0x8, "A"}, 
                {0x9, "C"}, 
                {0xA, "G"}, 
                {0xB, "T"},
                {0xC, "N"},
                {0X4, " "} 
            };
        }

        public static byte GetEncodedPart(string nucleotidePair)
        {
            return encodedNucleotidePairs[nucleotidePair];
        }

        public static string DetermineDecodedSeq(byte[] byteSeq)
        {
            string decodedSeq = "";
            foreach (byte encNucPair in byteSeq)
            {
                byte maskForHigherBits = 0xF0;
                var encFirstNucl = (encNucPair & maskForHigherBits) >> 4;
                byte maskForLowerBits = 0x0F;
                var encLastNucl = encNucPair & maskForLowerBits;
                decodedSeq = decodedSeq + decodedNucleotides[encFirstNucl] + decodedNucleotides[encLastNucl];
            }
            decodedSeq = decodedSeq.TrimEnd(' ');
            return decodedSeq;
        }

        public static string DetermineDecodedRelatedRefSeqBlock(int relatedByteSeqLength, long relatedPosStartValue,
            long refPosStartValue, byte[] refSeqValue)
        {
            long offset = relatedPosStartValue - refPosStartValue;
            bool isOffsetEven = (offset % 2 == 0);
            var startRefSeqByteOffset = (offset < 0) ? (int)((offset - 1) / 2 + 64) : (int)(offset / 2 + 64);
            string decodedRelatedRefSeqBlock;
            if (isOffsetEven)
            {
                byte[] relatedRefSeqBlock = new byte[relatedByteSeqLength];
                Array.Copy(refSeqValue, startRefSeqByteOffset, relatedRefSeqBlock, 0, relatedByteSeqLength);
                decodedRelatedRefSeqBlock = DetermineDecodedSeq(relatedRefSeqBlock);
            }
            else
            {
                byte[] relatedRefSeqBlock = new byte[relatedByteSeqLength - 1];
                Array.Copy(refSeqValue, startRefSeqByteOffset + 1, relatedRefSeqBlock, 0, relatedByteSeqLength - 1);
                decodedRelatedRefSeqBlock = DetermineDecodedSeq(relatedRefSeqBlock);
                byte maskForLowerBits = 0x0F;
                var encLastNucl = refSeqValue[startRefSeqByteOffset] & maskForLowerBits;
                decodedRelatedRefSeqBlock = decodedNucleotides[encLastNucl] + decodedRelatedRefSeqBlock;
            }
            return decodedRelatedRefSeqBlock;
        }

        public static string DetermineRefSeqBlockWithPrecAndSuccNucs(long sreadPosStartValue, long sreadPosEndValue,
            long refPosStartValue, byte[] refSeqValue)
        {
            // The sread pos start is left shifted by one because of preceding nucleotide:
            var actualSreadPosStartValue = sreadPosStartValue - 1;
            // The following contains plus one because of length-determination:
            int sreadByteSeqLength = ((int)(sreadPosEndValue - actualSreadPosStartValue) + 1) / 2;
            // If the actualSreadPosStartValue is not aligned to the beginning of the given byte exactly
            // then the size of byte array should be increased:
            sreadByteSeqLength = (Math.Abs(actualSreadPosStartValue - refPosStartValue) % 2 == 1) ? (sreadByteSeqLength + 1) : sreadByteSeqLength;
            string decodedRelatedRefSeqBlock = DetermineDecodedRelatedRefSeqBlock(sreadByteSeqLength,
            actualSreadPosStartValue, refPosStartValue, refSeqValue);
            decodedRelatedRefSeqBlock = ComplementSucceedingNuc(sreadPosEndValue, refPosStartValue, refSeqValue, decodedRelatedRefSeqBlock);
            return decodedRelatedRefSeqBlock;
        }

        public static string ComplementSucceedingNuc(long relatedPosEndValue, long refPosStartValue, byte[] refSeqValue, string decodedRelatedRefSeqBlock)
        {
            string succeedingNuc = "";
            long endOffset = relatedPosEndValue - refPosStartValue;
            bool isEndOffsetOdd = (Math.Abs(endOffset) % 2 == 1);
            if (isEndOffsetOdd)
            {
                var endRefSeqByteOffset = (int)(endOffset / 2 + 64);
                byte maskForHigherBits = 0xF0;
                var succeedingNucBin = (refSeqValue[endRefSeqByteOffset + 1] & maskForHigherBits) >> 4;
                succeedingNuc = BinaryNucleotideCodecUtil.decodedNucleotides[succeedingNucBin];
            }
            // If the end offset is even 
            //    the decodedRelatedRefSeqBlock has contained the succeeding nucleotide because of byte representation
            //    so the decodedRelatedRefSeqBlock is unchanged in this case:
            return decodedRelatedRefSeqBlock + succeedingNuc;
        }
    }
}
