using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BatchLoader.Services;
using System.Collections.Generic;

namespace BatchLoader.Test.Services
{
    [TestClass]
    public class BinaryEncodingUtilTest
    {
        [TestMethod, Description("This method should work properly when normal input and existing encodingDomainName are given.")]
        public void ConvertInputToEncodedBytesTest1()
        {
            // Given
            var input = "AC";
            var encodingDomainName = EncoderDomainNames.RefNuc;
            // "A" -> 001, "C" -> 010
            // "AC" -> 001010 => 10 (in decimal numeral system representation)
            byte [] expected = new byte[] {10};

            // When
            byte [] result = BinaryEncodingUtil.ConvertInputToEncodedBytes(input, encodingDomainName);

            // Then
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod, Description("This method should work properly when normal input (with large length) and existing encodingDomainName are given.")]
        public void ConvertInputToEncodedBytesTest2()
        {
            // Given
            var input = "TACGT";
            var encodingDomainName = EncoderDomainNames.RefNuc;
            // "A" -> 001, "C" -> 010, "G" -> 011, "T" -> 100
            // "TACGT" -> 100001010011100 => 17052 (in decimal numeral system representation)
            // but in the bytes representation: it means two bytes 1000010 (66) 10011100 (156)
            byte[] expected = new byte[] { 66, 156 };

            // When
            byte[] result = BinaryEncodingUtil.ConvertInputToEncodedBytes(input, encodingDomainName);

            // Then
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod, Description("This method should work properly when normal input is given.")]
        public void ConvertBasesInputToEncodedBytesTest1()
        {
            // Given
            var input = ".$,^+,+3ATCA";
            List<string> byProductsBySkipChars;
            // the input without by-product: .,,A -> 00001|00010|00010|00011
            // its 'byte-style' arrangement:          0000|10001000|01000011
            // so the expected bytes:                 (0)    (136)    (67)
            byte[] expectedResult = new byte[] { 0, 136, 67 };
            List<string> expectedByProduct = new List<string> { "2ATC\t", "2\t", "+\t", "0\t" };

            // When
            byte[] result = BinaryEncodingUtil.ConvertBasesInputToEncodedBytes(input, out byProductsBySkipChars);

            // Then
            CollectionAssert.AreEqual(expectedResult, result);
            CollectionAssert.AreEqual(expectedByProduct, byProductsBySkipChars);
        }
    }
}
