using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Services
{
    public static class BinaryEncodingUtil
    {
        public static readonly Dictionary<string, Dictionary<string, BitArray>> encoders;
        public static readonly Dictionary<string, int> encoderLengths;

        static BinaryEncodingUtil()
        {
            encoders = new Dictionary<string, Dictionary<string, BitArray>>();
            encoderLengths = new Dictionary<string, int>();
            InitializeEncoders();
        }

        private static void InitializeEncoders()
        {
            var refNucName = "refNuc";
            var refNuc = InitializeRefNucEncoder();
            InitializeEncoderLength(refNucName, refNuc);
            encoders.Add(refNucName, refNuc);

            var basesName = "bases";
            var bases = InitializeBasesEncoder();
            InitializeEncoderLength(basesName, bases);
            encoders.Add(basesName, bases);

            var basesQualName = "basesQual";
            var basesQual = InitializeBasesQualEncoder();
            InitializeEncoderLength(basesQualName, basesQual);
            encoders.Add(basesQualName, basesQual);
        }

        private static void InitializeEncoderLength(string encoderName, Dictionary<string, BitArray> encoder)
        {
            var fixedLengthOfEncoder = CheckInitializedEncoder(encoder);
            encoderLengths.Add(encoderName, fixedLengthOfEncoder);
        }

        private static int CheckInitializedEncoder(Dictionary<string, BitArray> encoder)
        {
            int? fixedLengthOfEncoder = null;
            foreach (BitArray bitArray in encoder.Values)
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

        /// <summary>
        /// This methods uses EncodingResource.resx as dictionary.
        /// This dictionary contains mappings of one character to 'bits' (zero-one string).
        /// The method use bit array representation of these bits.
        /// The encodingDomain determines which encoder should be used of this class.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodingDomainName"></param>
        /// <returns></returns>
        public static byte[] ConvertInputToEncodedBytes(string input, string encodingDomainName)
        {
            Dictionary<string, BitArray> encodingDomain;
            if (encoders.TryGetValue(encodingDomainName, out encodingDomain))
            {
                BitArray bitArrayOfInput = DetermineBitArrayOfInput(input, encodingDomain);
                byte[] encodedBytes = new byte[bitArrayOfInput.Length > 0 ? ((bitArrayOfInput.Length - 1) / 8) + 1 : 0];
                bitArrayOfInput.CopyTo(encodedBytes, 0);
                // The following is important because the BitArray will reverse the natural representation of bytes.
                // E.g.: 11 00011011 => {27, 3} (of a byte array) and not {3, 27} as it might be expected.
                Array.Reverse(encodedBytes);
                return encodedBytes;
            }
            else
            {
                throw new ArgumentException("The encoders does not contain the encodingDomain!");
            }
        }

        private static BitArray DetermineBitArrayOfInput(string input, Dictionary<string, BitArray> encodingDomain)
        {
            BitArray bitArrayOfInput = new BitArray(0);
            for (int index = 0; index < input.Length; index++)
            {
                var inputPart = input.Substring(index, 1);
                BitArray bitArrayOfInputPart;
                if (encodingDomain.TryGetValue(inputPart, out bitArrayOfInputPart))
                {
                    bitArrayOfInput = AppendBitArray(bitArrayOfInput, bitArrayOfInputPart);
                }
                else
                {
                    throw new ArgumentException("The input cannot be encoded because it contains invalid part!", inputPart);
                }
            }
            return bitArrayOfInput;
        }

        /// <summary>
        /// The implementation of this method is 'prepend' in fact.
        /// However, this implementation provides the proper representation of the resulting bit array.
        /// It is because the BitArray will reverse the natural representation of bits.
        /// E.g.: new BitArray(new bool[] {false, false, false, true}) -> 1000 when the content of bit array is copied to byte arrays.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="appended"></param>
        /// <returns></returns>
        public static BitArray AppendBitArray(BitArray original, BitArray appended)
        {
            var bools = new bool[original.Count + appended.Count];
            appended.CopyTo(bools, 0);
            original.CopyTo(bools, appended.Count);
            return new BitArray(bools);
        }

        /// <summary>
        /// This method is for encoding domain 'bases' which is more specific than others.
        /// The main lines are similar to general ConvertInputToEncodedBytes.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="byProductsBySkipChars"></param>
        /// <returns></returns>
        public static byte[] ConvertBasesInputToEncodedBytes(string input, out List<string> byProductsBySkipChars)
        {
            Dictionary<string, BitArray> encodingDomain;
            if (encoders.TryGetValue("bases", out encodingDomain))
            {
                byProductsBySkipChars = new List<string> { "", "", "", ""};
                BitArray bitArrayOfInput = DetermineBitArrayOfBasesInput(input, encodingDomain, byProductsBySkipChars);
                byte[] encodedBytes = new byte[bitArrayOfInput.Length > 0 ? ((bitArrayOfInput.Length - 1) / 8) + 1 : 0];
                bitArrayOfInput.CopyTo(encodedBytes, 0);
                // The following is important because the BitArray will reverse the natural representation of bytes.
                // E.g.: 11 00011011 => {27, 3} (of a byte array) and not {3, 27} as it might be expected.
                Array.Reverse(encodedBytes);
                return encodedBytes;
            }
            else
            {
                throw new ArgumentException("The encoders does not contain the encodingDomain!");
            }
        }

        private static BitArray DetermineBitArrayOfBasesInput(string input, Dictionary<string, BitArray> encodingDomain,
            List<string> byProductsBySkipChars)
        {
            BitArray bitArrayOfInput = new BitArray(0);
            int index = 0;
            while (index < input.Length)
            {
                var inputPart = input.Substring(index, 1);
                
                if ("^".Equals(inputPart))
                {
                    index = RecordReadStartingFactAndItsQual(input, byProductsBySkipChars, index);
                }
                else if ("$".Equals(inputPart))
                {
                    index = RecordReadEndingFact(byProductsBySkipChars, index);
                }
                // It is very important that the case "+" is preceded by case "^" because read mapping quality can contain symbol "+".
                else if ("+".Equals(inputPart))
                {
                    index = RecordExtraNucleotidesFact(input, byProductsBySkipChars, index);
                }
                // It is very important that the case "-" is preceded by case "^" because read mapping quality can contain symbol "-".
                else if ("-".Equals(inputPart))
                {
                    index = SkipMissingNucleotidesFact(input, index);
                }
                else
                {
                    BitArray bitArrayOfInputPart;
                    if (encodingDomain.TryGetValue(inputPart, out bitArrayOfInputPart))
                    {
                        bitArrayOfInput = AppendBitArray(bitArrayOfInput, bitArrayOfInputPart);
                        index++;
                    }
                    else
                    {
                        throw new ArgumentException("The input cannot be encoded because it contains invalid part!", inputPart);
                    }
                }
            }
            return bitArrayOfInput;
        }

        private static int RecordReadStartingFactAndItsQual(string input, List<string> byProductsBySkipChars, int index)
        {
            int readIndexInBases = index - 1;
            var readStartingSigns = byProductsBySkipChars[1];
            byProductsBySkipChars[1] = readStartingSigns + readIndexInBases + "\t";
            index++;
            var readMappingQual = input.Substring(index, 1);
            var readMappingQuals = byProductsBySkipChars[2];
            byProductsBySkipChars[2] = readMappingQuals + readMappingQual + "\t";
            index++;
            return index;
        }

        private static int RecordReadEndingFact(List<string> byProductsBySkipChars, int index)
        {
            int readIndexInBases = index - 1;
            var readEndingSigns = byProductsBySkipChars[3];
            byProductsBySkipChars[3] = readEndingSigns + readIndexInBases + "\t";
            index++;
            return index;
        }

        private static int RecordExtraNucleotidesFact(string input, List<string> byProductsBySkipChars, int index)
        {
            int readIndexInBases = index - 1;
            index++;
            var extraNucleotidesLength = Convert.ToInt32(input.Substring(index, 1));
            index++;
            var extraNucleotides = input.Substring(index, extraNucleotidesLength);
            var extraNucleotidesStore = byProductsBySkipChars[0];
            byProductsBySkipChars[0] = extraNucleotidesStore + readIndexInBases + extraNucleotides + "\t";
            index += extraNucleotidesLength;
            return index;
        }

        private static int SkipMissingNucleotidesFact(string input, int index)
        {
            index++;
            var extraNucleotidesLength = Convert.ToInt32(input.Substring(index, 1));
            index += extraNucleotidesLength + 1;
            return index;
        }

        private static Dictionary<string, BitArray> InitializeRefNucEncoder()
        {
            var refNuc = new Dictionary<string, BitArray>();
            refNuc.Add("A", EncodeZeroOneStringToBitArrays(EncodingResource.refNuc_A));
            refNuc.Add("C", EncodeZeroOneStringToBitArrays(EncodingResource.refNuc_C));
            refNuc.Add("G", EncodeZeroOneStringToBitArrays(EncodingResource.refNuc_G));
            refNuc.Add("T", EncodeZeroOneStringToBitArrays(EncodingResource.refNuc_T));
            return refNuc;
        }

        private static Dictionary<string, BitArray> InitializeBasesEncoder()
        {
            var bases = new Dictionary<string, BitArray>();
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

        private static Dictionary<string, BitArray> InitializeBasesQualEncoder()
        {
            var basesQual = new Dictionary<string, BitArray>();
            for (int intReprOfChar = 33 /*'!'*/; intReprOfChar <= 126 /*'~'*/; intReprOfChar++)
            {
                char ch = (char)intReprOfChar;
                string stringReprOfChar = new string(new char[] {ch});
                string stringBitOfChar = ConvertQualityCharToBitString(ch);
                basesQual.Add(stringReprOfChar, EncodeZeroOneStringToBitArrays(stringBitOfChar));
            }
            return basesQual;
        }

        private static string ConvertQualityCharToBitString(char qualityChar)
        {
            int asciiCodeOfQualityChar = (int)qualityChar;
            int relatedQualityScore = asciiCodeOfQualityChar - 33;
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
