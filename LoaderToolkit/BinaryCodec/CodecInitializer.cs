using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCodec
{
    static class CodecInitializer
    {
        public static void InitializeCodecs(Dictionary<Constants.CodecDomainNames, BidirectionalDictionary<string, BitArray>> codecs,
            Dictionary<Constants.CodecDomainNames, int> codecBitStringLengths)
        {
            var refNucDomainName = Constants.CodecDomainNames.RefNuc;
            var refNuc = InitializeRefNucCodec();
            InitializeCodecLength(refNucDomainName, refNuc, codecBitStringLengths);
            codecs.Add(refNucDomainName, refNuc);

            var basesDomainName = Constants.CodecDomainNames.Bases;
            var bases = InitializeBasesCodec();
            InitializeCodecLength(basesDomainName, bases, codecBitStringLengths);
            codecs.Add(basesDomainName, bases);

            var basesQualDomainName = Constants.CodecDomainNames.BasesQual;
            var basesQual = InitializeBasesQualCodec();
            InitializeCodecLength(basesQualDomainName, basesQual, codecBitStringLengths);
            codecs.Add(basesQualDomainName, basesQual);
        }

        private static void InitializeCodecLength(Constants.CodecDomainNames codecDomainName,
            BidirectionalDictionary<string, BitArray> codec, Dictionary<Constants.CodecDomainNames, int> codecBitStringLengths)
        {
            var fixedLengthOfEncoder = CheckInitializedEncoder(codec);
            codecBitStringLengths.Add(codecDomainName, fixedLengthOfEncoder);
        }

        private static int CheckInitializedEncoder(BidirectionalDictionary<string, BitArray> codec)
        {
            int? fixedLengthOfEncoder = null;
            foreach (BitArray bitArray in codec.Forward.Values)
            {
                if (fixedLengthOfEncoder == null)
                {
                    fixedLengthOfEncoder = bitArray.Count;
                }
                else if (bitArray.Count != fixedLengthOfEncoder)
                {
                    string exceptionMessage = String.Format("The encoder should contain bit arrays of the same length: {0} " +
                    "but the length of one of them is: {1}! The EncodingResource.resx contains invalid entries.",
                    fixedLengthOfEncoder, bitArray.Count);
                    throw new ArgumentException(exceptionMessage);
                }
            }
            int result = 0;
            if (fixedLengthOfEncoder.HasValue)
            {
                result = fixedLengthOfEncoder.Value;
            }
            return result;
        }

        private static BitArray EncodeZeroOneStringToBitArrays(string input)
        {
            char[] inputChars = input.ToCharArray();
            // The following is important because the BitArray will reverse the natural representation of bits.
            // E.g.: new BitArray(new bool[] {false, false, false, true}) -> 1000 when the content of bit array is copied to byte arrays.
            Array.Reverse(inputChars);
            bool[] bits = new bool[inputChars.Length];
            int index = 0;
            foreach (char ch in inputChars)
            {
                switch (ch)
                {
                    case '0':
                        bits[index] = false;
                        index++;
                        break;
                    case '1':
                        bits[index] = true;
                        index++;
                        break;
                    default:
                        throw new ArgumentException("The input should contain only zero or one!");
                }
            }
            return new BitArray(bits);
        }

        private static BidirectionalDictionary<string, BitArray> InitializeRefNucCodec()
        {
            var refNuc = new BidirectionalDictionary<string, BitArray>(new BitArrayEqualityComparer());
            refNuc.Add("A", EncodeZeroOneStringToBitArrays(EncodingResource.refNuc_A));
            refNuc.Add("C", EncodeZeroOneStringToBitArrays(EncodingResource.refNuc_C));
            refNuc.Add("G", EncodeZeroOneStringToBitArrays(EncodingResource.refNuc_G));
            refNuc.Add("T", EncodeZeroOneStringToBitArrays(EncodingResource.refNuc_T));
            return refNuc;
        }

        private static BidirectionalDictionary<string, BitArray> InitializeBasesCodec()
        {
            var bases = new BidirectionalDictionary<string, BitArray>(new BitArrayEqualityComparer());
            bases.Add(".", EncodeZeroOneStringToBitArrays(EncodingResource.bases_Dot));
            bases.Add(",", EncodeZeroOneStringToBitArrays(EncodingResource.bases_Comma));
            bases.Add("A", EncodeZeroOneStringToBitArrays(EncodingResource.bases_CapitalA));
            bases.Add("a", EncodeZeroOneStringToBitArrays(EncodingResource.bases_SmallA));
            bases.Add("C", EncodeZeroOneStringToBitArrays(EncodingResource.bases_CapitalC));
            bases.Add("c", EncodeZeroOneStringToBitArrays(EncodingResource.bases_SmallC));
            bases.Add("G", EncodeZeroOneStringToBitArrays(EncodingResource.bases_CapitalG));
            bases.Add("g", EncodeZeroOneStringToBitArrays(EncodingResource.bases_SmallG));
            bases.Add("T", EncodeZeroOneStringToBitArrays(EncodingResource.bases_CapitalT));
            bases.Add("t", EncodeZeroOneStringToBitArrays(EncodingResource.bases_SmallT));
            bases.Add("N", EncodeZeroOneStringToBitArrays(EncodingResource.bases_CapitalN));
            bases.Add("n", EncodeZeroOneStringToBitArrays(EncodingResource.bases_SmallN));
            bases.Add("*", EncodeZeroOneStringToBitArrays(EncodingResource.bases_Asterisk));
            return bases;
        }

        private static BidirectionalDictionary<string, BitArray> InitializeBasesQualCodec()
        {
            var basesQual = new BidirectionalDictionary<string, BitArray>(new BitArrayEqualityComparer());
            for (int intReprOfChar = 33 /*'!'*/; intReprOfChar <= 126 /*'~'*/; intReprOfChar++)
            {
                char ch = (char)intReprOfChar;
                string stringReprOfChar = new string(new char[] { ch });
                string stringBitOfChar = ConvertQualityCharToBitString(intReprOfChar);
                basesQual.Add(stringReprOfChar, EncodeZeroOneStringToBitArrays(stringBitOfChar));
            }
            return basesQual;
        }

        private static string ConvertQualityCharToBitString(int asciiCodeOfQualityChar)
        {
            // The first quality char is '+' which has ascii code 33 so the related quality score will be 1 (and not zero).
            int relatedQualityScore = asciiCodeOfQualityChar - 32;
            return ConvertIntToBitString(relatedQualityScore, 7);
        }

        private static string ConvertIntToBitString(int x, int lengthOfBits)
        {
            char[] bits = new char[lengthOfBits];
            for (int index = 0; index < lengthOfBits; index++)
            {
                bits[index] = '0';
            }

            int i = lengthOfBits - 1;

            while (x != 0)
            {
                bits[i--] = (x & 1) == 1 ? '1' : '0';
                x >>= 1;
            }

            return new string(bits);
        }
    }
}
