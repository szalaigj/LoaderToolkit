using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCodec
{
    static class BoolsDecoder
    {
        public static string DecodeInputBools(bool[] bools, BidirectionalDictionary<string, BitArray> codecDomain, int codecBitStringLength)
        {
            string result = "";
            bool[] boolsOfEncodedSign = new bool[codecBitStringLength];
            int indexOfBoolsOfEncodedSign = 0;
            int falseCounter = 0;
            for (int index = 0; index < bools.Length; index++)
            {
                bool actualValue = bools[index];
                boolsOfEncodedSign[indexOfBoolsOfEncodedSign] = actualValue;
                indexOfBoolsOfEncodedSign++;
                if (!actualValue)
                {
                    falseCounter++;
                }
                else
                {
                    falseCounter = 0;
                }
                if ((indexOfBoolsOfEncodedSign % codecBitStringLength) == 0)
                {
                    if (falseCounter == codecBitStringLength)
                    {
                        // If there are as many 'false's of BitArray as codecBitStringLength then these are only the trailing zeros.
                        break;
                    }
                    result = DecodeUnitOfInputBits(codecDomain, codecBitStringLength, result,
                        ref boolsOfEncodedSign, ref indexOfBoolsOfEncodedSign, ref falseCounter);
                }
            }
            return result;
        }

        private static string DecodeUnitOfInputBits(BidirectionalDictionary<string, BitArray> codecDomain, int codecBitStringLength,
            string actualDecodedString, ref bool[] boolsOfEncodedSign, ref int indexOfBoolsOfEncodedSign, ref int falseCounter)
        {
            var bitArrayOfEncodedSign = new BitArray(boolsOfEncodedSign);
            string decodedSign;
            if (codecDomain.Reverse.TryGetValue(bitArrayOfEncodedSign, out decodedSign))
            {
                actualDecodedString += decodedSign;
            }
            else
            {
                throw new ArgumentException("The codecDomain does not contain the unit of input bools!");
            }
            indexOfBoolsOfEncodedSign = 0;
            boolsOfEncodedSign = new bool[codecBitStringLength];
            falseCounter = 0;
            return actualDecodedString;
        }
    }
}
