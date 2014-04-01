using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Services
{
    public enum EncoderDomainNames { RefNuc, Bases, BasesQual }

    public static class BinaryEncodingUtil
    {
        public static readonly Dictionary<EncoderDomainNames, Dictionary<string, BitArray>> encoders;
        public static readonly Dictionary<EncoderDomainNames, int> encoderBitStringLengths;

        private const string separator = "\t";

        private class InputIndeces
        {
            /// <summary>
            /// The actual char position of the input string.
            /// </summary>
            public int InputCharIndex { get; set; }

            /// <summary>
            /// The actual read position of the bases.
            /// </summary>
            public int ReadIndex { get; set; }

        }

        static BinaryEncodingUtil()
        {
            encoders = new Dictionary<EncoderDomainNames, Dictionary<string, BitArray>>();
            encoderBitStringLengths = new Dictionary<EncoderDomainNames, int>();
            InitializeEncoders();
        }

        private static void InitializeEncoders()
        {
            var refNucDomainName = EncoderDomainNames.RefNuc;
            var refNuc = InitializeRefNucEncoder();
            InitializeEncoderLength(refNucDomainName, refNuc);
            encoders.Add(refNucDomainName, refNuc);

            var basesDomainName = EncoderDomainNames.Bases;
            var bases = InitializeBasesEncoder();
            InitializeEncoderLength(basesDomainName, bases);
            encoders.Add(basesDomainName, bases);

            var basesQualDomainName = EncoderDomainNames.BasesQual;
            var basesQual = InitializeBasesQualEncoder();
            InitializeEncoderLength(basesQualDomainName, basesQual);
            encoders.Add(basesQualDomainName, basesQual);
        }

        private static void InitializeEncoderLength(EncoderDomainNames encoderDomainName, Dictionary<string, BitArray> encoder)
        {
            var fixedLengthOfEncoder = CheckInitializedEncoder(encoder);
            encoderBitStringLengths.Add(encoderDomainName, fixedLengthOfEncoder);
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
        public static byte[] ConvertInputToEncodedBytes(string input, EncoderDomainNames encodingDomainName)
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
            if (encoders.TryGetValue(EncoderDomainNames.Bases, out encodingDomain))
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
            var inputIndeces = new InputIndeces();
            while (inputIndeces.InputCharIndex < input.Length)
            {
                var inputPart = input.Substring(inputIndeces.InputCharIndex, 1);
                if ("^".Equals(inputPart))
                {
                    RecordReadStartingFactAndItsQual(input, byProductsBySkipChars, inputIndeces);
                }
                else if ("$".Equals(inputPart))
                {
                    RecordReadEndingFact(byProductsBySkipChars, inputIndeces);
                }
                // It is very important that the case "+" is preceded by case "^" because read mapping quality can contain symbol "+".
                else if ("+".Equals(inputPart))
                {
                    RecordExtraNucleotidesFact(input, byProductsBySkipChars, inputIndeces);
                }
                // It is very important that the case "-" is preceded by case "^" because read mapping quality can contain symbol "-".
                else if ("-".Equals(inputPart))
                {
                    SkipMissingNucleotidesFact(input, inputIndeces);
                }
                else
                {
                    bitArrayOfInput = UpdateBitArray(encodingDomain, bitArrayOfInput, inputIndeces, inputPart);
                }
            }
            return bitArrayOfInput;
        }

        private static BitArray UpdateBitArray(Dictionary<string, BitArray> encodingDomain, BitArray bitArrayOfInput, InputIndeces inputIndeces, string inputPart)
        {
            BitArray bitArrayOfInputPart;
            if (encodingDomain.TryGetValue(inputPart, out bitArrayOfInputPart))
            {
                bitArrayOfInput = AppendBitArray(bitArrayOfInput, bitArrayOfInputPart);
                inputIndeces.InputCharIndex++;
                inputIndeces.ReadIndex++;
            }
            else
            {
                throw new ArgumentException("The input cannot be encoded because it contains invalid part!", inputPart);
            }
            return bitArrayOfInput;
        }

        /// <summary>
        /// This method dockets the read indices which are started at actual position.
        /// Note: The read nucleotide value follow starting sign and read mapping quality in this case.
        /// E.g.: input: ,^~. when the input processing reach the char '^' then ReadIndex is 1
        ///       because the ReadIndex has been incremented after char ',' processing.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="byProductsBySkipChars"></param>
        /// <param name="inputIndeces"></param>
        private static void RecordReadStartingFactAndItsQual(string input, List<string> byProductsBySkipChars, InputIndeces inputIndeces)
        {
            var readStartingSigns = byProductsBySkipChars[1];
            byProductsBySkipChars[1] = readStartingSigns + inputIndeces.ReadIndex + separator;
            inputIndeces.InputCharIndex++;
            var readMappingQual = input.Substring(inputIndeces.InputCharIndex, 1);
            var readMappingQuals = byProductsBySkipChars[2];
            byProductsBySkipChars[2] = readMappingQuals + readMappingQual + separator;
            inputIndeces.InputCharIndex++;
        }

        /// <summary>
        /// This method dockets the read indices which are ended at actual position.
        /// Note: The read nucleotide value is followed by ending sign in this case
        ///       but the ReadIndex has already been incremented so it should be handled.
        /// E.g.: input: .$, the char '$' is assigned to the read '.'.
        /// </summary>
        /// <param name="byProductsBySkipChars"></param>
        /// <param name="inputIndeces"></param>
        private static void RecordReadEndingFact(List<string> byProductsBySkipChars, InputIndeces inputIndeces)
        {
            var readEndingSigns = byProductsBySkipChars[3];
            var previousReadIndex = inputIndeces.ReadIndex - 1;
            byProductsBySkipChars[3] = readEndingSigns + previousReadIndex + separator;
            inputIndeces.InputCharIndex++;
        }

        /// <summary>
        /// This method dockets the read indices which have extra nucleotides after actual position.
        /// Note: The read nucleotide value is followed by extra nucleotides signs in this case.
        ///       but the ReadIndex has already been incremented so it should be handled.
        /// E.g.: input: .+2AB, the char '+' is assigned to the read '.'.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="byProductsBySkipChars"></param>
        /// <param name="inputIndeces"></param>
        private static void RecordExtraNucleotidesFact(string input, List<string> byProductsBySkipChars, InputIndeces inputIndeces)
        {
            inputIndeces.InputCharIndex++;
            var extraNucleotidesLength = Convert.ToInt32(input.Substring(inputIndeces.InputCharIndex, 1));
            inputIndeces.InputCharIndex++;
            var extraNucleotides = input.Substring(inputIndeces.InputCharIndex, extraNucleotidesLength);
            var extraNucleotidesStore = byProductsBySkipChars[0];
            var previousReadIndex = inputIndeces.ReadIndex - 1;
            byProductsBySkipChars[0] = extraNucleotidesStore + previousReadIndex + extraNucleotides + separator;
            inputIndeces.InputCharIndex += extraNucleotidesLength;
        }

        /// <summary>
        /// This method skips the description of missing nucleotides after actual position (because the asterisk will indicate this fact).
        /// Note: The read nucleotide value is followed by missing nucleotides signs in this case.
        /// E.g.: input: .-2AB, the char '-' is assigned to the read '.'.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputIndeces"></param>
        private static void SkipMissingNucleotidesFact(string input, InputIndeces inputIndeces)
        {
            inputIndeces.InputCharIndex++;
            var extraNucleotidesLength = Convert.ToInt32(input.Substring(inputIndeces.InputCharIndex, 1));
            inputIndeces.InputCharIndex += extraNucleotidesLength + 1;
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
