using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BatchLoader.Services;
using System.Collections.Generic;
using System.IO;
using LoaderLibrary.Load;

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

        [TestMethod, Description("This method should work properly when normal refNuc input is given.")]
        public void DecodeInputFileTest1()
        {
            // Given
            string refNucForEncoding = "TAG";
            var encodingDomainName = EncoderDomainNames.RefNuc;
            byte[] refNucInEncodedBytes = BinaryEncodingUtil.ConvertInputToEncodedBytes(refNucForEncoding, encodingDomainName);
            // Prepare the input file:
            string filename = "input_bin.dat";
            WriteEncodedBytesToFile(filename, refNucInEncodedBytes);

            // When
            var result = BinaryEncodingUtil.DecodeInputFile(filename, encodingDomainName);

            // Then
            Assert.AreEqual(refNucForEncoding, result);
        }

        [TestMethod, Description("This method should work properly when normal bases input is given.")]
        public void DecodeInputFileTest2()
        {
            // Given
            string basesForEncoding = ".$,^+,+3ATCA";
            var encodingDomainName = EncoderDomainNames.Bases;
            List<string> byProductsBySkipChars;
            byte[] basesInEncodedBytes = BinaryEncodingUtil.ConvertBasesInputToEncodedBytes(basesForEncoding, out byProductsBySkipChars);
            // Prepare the input file:
            string filename = "input_bin.dat";
            WriteEncodedBytesToFile(filename, basesInEncodedBytes);

            // When
            var result = BinaryEncodingUtil.DecodeInputFile(filename, encodingDomainName);

            // Then
            Assert.AreEqual(basesForEncoding, result);
        }

        private void WriteEncodedBytesToFile(string filename, byte[] basesInEncodedBytes)
        {
            var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 0x10000 /* 64K */);
            var binWriter = new BinaryWriter(fileStream);
            binWriter.Write(basesInEncodedBytes);
            binWriter.Flush();
            binWriter.Close();
            binWriter.Dispose();
            binWriter = null;
        }
    }
}
