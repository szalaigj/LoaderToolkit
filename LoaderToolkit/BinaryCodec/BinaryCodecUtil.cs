using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCodec
{
    public static class BinaryCodecUtil
    {
        public static readonly Dictionary<Constants.CodecDomainNames, BidirectionalDictionary<string, BitArray>> codecs;
        public static readonly Dictionary<Constants.CodecDomainNames, int> codecBitStringLengths;

        static BinaryCodecUtil()
        {
            codecs = new Dictionary<Constants.CodecDomainNames, BidirectionalDictionary<string, BitArray>>();
            codecBitStringLengths = new Dictionary<Constants.CodecDomainNames, int>();
            CodecInitializer.InitializeCodecs(codecs, codecBitStringLengths);
        }

        /// <summary>
        /// This methods uses EncodingResource.resx as dictionary.
        /// This dictionary contains mappings of one character to 'bits' (zero-one string).
        /// The method use bit array representation of these bits.
        /// The codecDomain determines which codec should be used of this class.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="codecDomainName"></param>
        /// <returns></returns>
        public static byte[] ConvertInputToEncodedBytes(string input, Constants.CodecDomainNames codecDomainName)
        {
            BidirectionalDictionary<string, BitArray> codecDomain;
            if (codecs.TryGetValue(codecDomainName, out codecDomain))
            {
                BitArray bitArrayOfInput = BitArrayOfInputCreator.DetermineBitArrayOfInput(input, codecDomain);
                byte[] encodedBytes = new byte[bitArrayOfInput.Length > 0 ? ((bitArrayOfInput.Length - 1) / 8) + 1 : 0];
                bitArrayOfInput.CopyTo(encodedBytes, 0);
                return encodedBytes;
            }
            else
            {
                throw new ArgumentException("The codecs does not contain the codecDomain!");
            }
        }

        /// <summary>
        /// This method is for codec domain 'bases' which is more specific than others.
        /// The main lines are similar to general ConvertInputToEncodedBytes.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="byProductsBySkipChars"></param>
        /// <returns></returns>
        public static byte[] ConvertBasesInputToEncodedBytes(string input, out List<string> byProductsBySkipChars)
        {
            BidirectionalDictionary<string, BitArray> codecDomain;
            if (codecs.TryGetValue(Constants.CodecDomainNames.Bases, out codecDomain))
            {
                byProductsBySkipChars = new List<string> { "", "", "", "" };
                BitArray bitArrayOfInput = BitArrayOfInputCreator.DetermineBitArrayOfBasesInput(input, codecDomain, byProductsBySkipChars);
                byte[] encodedBytes = new byte[bitArrayOfInput.Length > 0 ? ((bitArrayOfInput.Length - 1) / 8) + 1 : 0];
                bitArrayOfInput.CopyTo(encodedBytes, 0);
                return encodedBytes;
            }
            else
            {
                throw new ArgumentException("The codecs does not contain the codecDomain!");
            }
        }

        /// <summary>
        /// This method decodes the input bytes based on the codecDomainName.
        /// </summary>
        /// <param name="bytesOfEncodedValue"></param>
        /// <param name="codecDomainName"></param>
        /// <returns></returns>
        public static string DecodeInputBytes(byte[] bytesOfEncodedValue, Constants.CodecDomainNames codecDomainName)
        {
            string result = "";
            BidirectionalDictionary<string, BitArray> codecDomain;
            int codecBitStringLength;
            if (codecs.TryGetValue(codecDomainName, out codecDomain) 
                && codecBitStringLengths.TryGetValue(codecDomainName, out codecBitStringLength))
            {
                var bools = DetermineBoolsOfInput(bytesOfEncodedValue);
                result = BoolsDecoder.DecodeInputBools(bools, codecDomain, codecBitStringLength);
            }
            else
            {
                throw new ArgumentException("The codecs does not contain the codecDomain!");
            }
            return result;
        }

        private static bool[] DetermineBoolsOfInput(byte[] bytesOfEncodedValue)
        {
            BitArray bitArray = new BitArray(bytesOfEncodedValue);
            var bitArrayCount = bitArray.Count;
            var bools = new bool[bitArrayCount];
            bitArray.CopyTo(bools, 0);
            return bools;
        }
    }
}
