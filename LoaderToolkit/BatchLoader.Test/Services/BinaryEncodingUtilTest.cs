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
            var input = "ACGT";
            var encodingDomainName = EncoderDomainNames.RefNuc;
            // "A" -> 00, "C" -> 01, "G" -> 10, "T" -> 11
            // "ACGT" -> 00011011 => 27 (in decimal numeral system representation)
            byte [] expected = new byte[] {27};

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
            // "A" -> 00, "C" -> 01, "G" -> 10, "T" -> 11
            // "TACGT" -> 1100011011 => 795 (in decimal numeral system representation)
            // but in the bytes representation: it means two bytes 11 (3) 00011011 (27)
            byte[] expected = new byte[] { 3, 27 };

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
            // the input without by-product: .,,A -> 00000|00001|00001|00010
            // its 'byte-style' arrangement:         00000|00000100|00100010
            // so the expected bytes:                 (0)     (4)     (34)
            byte[] expectedResult = new byte[] { 0, 4, 34 };
            List<string> expectedByProduct = new List<string> { "2ATC\t", "2\t", "+\t", "0\t" };

            // When
            byte[] result = BinaryEncodingUtil.ConvertBasesInputToEncodedBytes(input, out byProductsBySkipChars);

            // Then
            CollectionAssert.AreEqual(expectedResult, result);
            CollectionAssert.AreEqual(expectedByProduct, byProductsBySkipChars);
        }
    }
}
