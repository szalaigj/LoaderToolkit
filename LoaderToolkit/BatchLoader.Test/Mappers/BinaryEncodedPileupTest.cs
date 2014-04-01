using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BatchLoader.Mappers;
using LoaderLibrary.Load;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BatchLoader.Test.Mappers
{
    [TestClass]
    public class BinaryEncodedPileupTest
    {
        [TestMethod, Description("This method should work properly when normal input and binary output.")]
        public void MapTest1()
        {
            // Given
            BinaryEncodedPileup systemUnderTest = new BinaryEncodedPileup();
            FileUtils<string> fileUtils = new FileUtils<string>();
            systemUnderTest.Binary = true;
            string filename = "output_bin.dat";
            systemUnderTest.OutputStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 0x10000 /* 64K */);
            systemUnderTest.OutputBinary = new BinaryWriter(systemUnderTest.OutputStream);
            systemUnderTest.BulkWriter = new BulkFileWriter(systemUnderTest.OutputBinary);
            string inputLine = "CPA1_a\tgi|441431414|ref|NW_003871055.3|\t5155\tA\t1\t^~,\t=";

            // When
            systemUnderTest.Map(inputLine);
            fileUtils.Close(systemUnderTest);

            // Then
            inputLine.IndexOf("CPA");
        }

        [TestMethod, Description("This method should work properly when normal input and text output.")]
        public void MapTest2()
        {
            // Given
            BinaryEncodedPileup systemUnderTest = new BinaryEncodedPileup();
            FileUtils<string> fileUtils = new FileUtils<string>();
            systemUnderTest.Binary = false;
            string filename = "output_text.txt";
            systemUnderTest.OutputStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 0x10000 /* 64K */);
            systemUnderTest.OutputWriter = new StreamWriter(systemUnderTest.OutputStream, Encoding.Unicode);
            systemUnderTest.BulkWriter = new BulkFileWriter(systemUnderTest.OutputWriter);
            string inputLine = "CPA1_a\tgi|441431414|ref|NW_003871055.3|\t5155\tA\t1\t^~,\t=";

            // When
            systemUnderTest.Map(inputLine);
            fileUtils.Close(systemUnderTest);

            // Then
            inputLine.IndexOf("CPA");
        }
    }
}
