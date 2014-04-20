using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace BinaryCodec.Test
{
    [TestClass]
    public class BinaryCodecUtilTest
    {
        [TestMethod, Description("This method should work properly when normal input and existing encodingDomainName are given.")]
        public void ConvertInputToEncodedBytesTest1()
        {
            // Given
            var input = "AC";
            var codecDomainName = Constants.CodecDomainNames.RefNuc;
            // "A" -> 001, "C" -> 010
            // But the BitArray  will reverse the natural representation of bits.
            // E.g.: new BitArray(new bool[] {true, false, false, false, true, false}) -> 010001 when the content of bit array is 
            // copied to byte arrays. And the following method ConvertInputToEncodedBytes appends 
            // bool values (!) of BitArray of next character for the previous ones.
            // "A" -> {true, false, false}, "C" -> {false, true, false}; "A" append "C" -> {true, false, false, false, true, false}
            // So "AC" -> 010 001 => 17 (in decimal numeral system representation)
            byte[] expected = new byte[] { 17 };

            // When
            byte[] result = BinaryCodecUtil.ConvertInputToEncodedBytes(input, codecDomainName);

            // Then
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod, Description("This method should work properly when normal input (with large length) and existing encodingDomainName are given.")]
        public void ConvertInputToEncodedBytesTest2()
        {
            // Given
            var input = "TACGT";
            var codecDomainName = Constants.CodecDomainNames.RefNuc;
            // "A" -> 001, "C" -> 010, "G" -> 011, "T" -> 100
            // See the explanation of the following in comment of method ConvertInputToEncodedBytesTest1.
            // "TACGT" ->100 011 010 001 100 => 18060 (in decimal numeral system representation)
            // but in the bytes representation: it means two bytes 1000110 (70) 10001100 (140)
            // And these are reversed by BitArray for the following form:
            byte[] expected = new byte[] { 140, 70 };

            // When
            byte[] result = BinaryCodecUtil.ConvertInputToEncodedBytes(input, codecDomainName);

            // Then
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod, Description("This method should work properly when normal input (with large length) and existing encodingDomainName basesQual are given.")]
        public void ConvertBasesQualInputToEncodedBytesTest1()
        {
            // Given
            var input = "Z__";
            var codecDomainName = Constants.CodecDomainNames.BasesQual;
            byte[] expected = new byte[] { 186, 223, 15 };

            // When
            byte[] result = BinaryCodecUtil.ConvertInputToEncodedBytes(input, codecDomainName);

            // Then
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod, Description("This method should work properly when normal input is given.")]
        public void ConvertBasesInputToEncodedBytesTest1()
        {
            // Given
            var input = ".$,^+,+3ATCA";
            Dictionary<Constants.ColumnsFromSkipChars, string> byProductsBySkipChars;
            // the input without by-product: .,,A -> 00011|00010|00010|00001
            //                                        (A)   (,)   (,)   (.)
            // its 'byte-style' arrangement:          0001|10001000|01000001
            // so the expected bytes:                 (1)    (136)    (65)
            // and these are reversed by BitArray for the following form:
            byte[] expectedResult = new byte[] { 65, 136, 1 };
            Dictionary<Constants.ColumnsFromSkipChars, string> expectedByProduct = new Dictionary<Constants.ColumnsFromSkipChars, string>()
            { 
                {Constants.ColumnsFromSkipChars.ExtraNuc, "2ATC\t"}, 
                {Constants.ColumnsFromSkipChars.MissingNuc, ""},
                {Constants.ColumnsFromSkipChars.StartingSigns, "2\t"},
                {Constants.ColumnsFromSkipChars.MappingQual, "+\t"},
                {Constants.ColumnsFromSkipChars.EndingSigns, "0\t"}
            };

            // When
            byte[] result = BinaryCodecUtil.ConvertBasesInputToEncodedBytes(input, out byProductsBySkipChars);

            // Then
            CollectionAssert.AreEqual(expectedResult, result);
            CollectionAssert.AreEqual(expectedByProduct, byProductsBySkipChars);
        }

        [TestMethod, Description("This method should work properly when normal input with missing nucleotides is given.")]
        public void ConvertBasesInputToEncodedBytesTest2()
        {
            // Given
            var input = ".-2AG,,A";
            Dictionary<Constants.ColumnsFromSkipChars, string> byProductsBySkipChars;
            // the input without by-product: .,,A -> 00011|00010|00010|00001
            //                                        (A)   (,)   (,)   (.)
            // its 'byte-style' arrangement:          0001|10001000|01000001
            // so the expected bytes:                 (1)    (136)    (65)
            // and these are reversed by BitArray for the following form:
            byte[] expectedResult = new byte[] { 65, 136, 1 };
            Dictionary<Constants.ColumnsFromSkipChars, string> expectedByProduct = new Dictionary<Constants.ColumnsFromSkipChars, string>()
            { 
                {Constants.ColumnsFromSkipChars.ExtraNuc, ""}, 
                {Constants.ColumnsFromSkipChars.MissingNuc, "0AG\t"},
                {Constants.ColumnsFromSkipChars.StartingSigns, ""},
                {Constants.ColumnsFromSkipChars.MappingQual, ""},
                {Constants.ColumnsFromSkipChars.EndingSigns, ""}
            };

            // When
            byte[] result = BinaryCodecUtil.ConvertBasesInputToEncodedBytes(input, out byProductsBySkipChars);

            // Then
            CollectionAssert.AreEqual(expectedResult, result);
            CollectionAssert.AreEqual(expectedByProduct, byProductsBySkipChars);
        }

        [TestMethod, Description("This method should work properly when normal refNuc input is given.")]
        public void DecodeInputFileTest1()
        {
            // Given
            string refNucForEncoding = "TAG";
            var codecDomainName = Constants.CodecDomainNames.RefNuc;
            byte[] refNucInEncodedBytes = BinaryCodecUtil.ConvertInputToEncodedBytes(refNucForEncoding, codecDomainName);
            // Prepare the input file:
            string filename = "input_bin.dat";
            WriteEncodedBytesToFile(filename, refNucInEncodedBytes);

            // When
            var result = DecodeInputFile(filename, codecDomainName);

            // Then
            Assert.AreEqual(refNucForEncoding, result);
        }

        [TestMethod, Description("This method should work properly when normal bases input is given.")]
        public void DecodeInputFileTest2()
        {
            // Given
            string basesForEncoding = ".$,^+,+3ATCA";
            string expectedResult = ".,,A";
            var codecDomainName = Constants.CodecDomainNames.Bases;
            Dictionary<Constants.ColumnsFromSkipChars, string> byProductsBySkipChars;
            byte[] basesInEncodedBytes = BinaryCodecUtil.ConvertBasesInputToEncodedBytes(basesForEncoding, out byProductsBySkipChars);
            // Prepare the input file:
            string filename = "input_bin.dat";
            WriteEncodedBytesToFile(filename, basesInEncodedBytes);

            // When
            var result = DecodeInputFile(filename, codecDomainName);

            // Then
            Assert.AreEqual(expectedResult, result);
        }

        private static string DecodeInputFile(string fileName, Constants.CodecDomainNames codecDomainName)
        {
            string result;
            using (BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                // Suppose the stream cannot be longer than 4294967295 in bytes (the limit is approx. 4 Gigabyte).
                int length = (int)binReader.BaseStream.Length;
                byte[] bytesFromInputFile = binReader.ReadBytes(length);
                result = BinaryCodecUtil.DecodeInputBytes(bytesFromInputFile, codecDomainName);
            }
            return result;
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
