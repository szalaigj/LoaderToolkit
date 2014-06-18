using LoaderLibrary.CommandLineParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefFilePreparing.Verbs
{
    [Verb(Name = "Organize", Description = "Organize reference genome/chromosome files.")]
    public class Organize : Verb
    {
        private string source;
        private string outputPath;
        private int incrementOfPosStart;

        [Parameter(Name = "Source", Description = "Source file pattern.", Required = true)]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        [Parameter(Name = "OutputPath", Description = "Path of output (organized) files.", Required = true)]
        public string OutputPath
        {
            get { return outputPath; }
            set { outputPath = value; }
        }

        [Parameter(Name = "IncrementOfPosStart", Description = "The distance between consecutive posStart values.")]
        public int IncrementOfPosStart
        {
            get { return incrementOfPosStart; }
            set { incrementOfPosStart = value; }
        }

        public Organize()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.source = null;
            this.outputPath = null;
            this.incrementOfPosStart = 256;
        }

        public override void Run()
        {
            // Find files matching pattern
            var dir = Path.GetDirectoryName(Source);
            var pat = Path.GetFileName(Source);
            var files = Directory.GetFiles(dir, pat);

            if (files.Length == 0)
            {
                throw new ArgumentException("No files matching the source pattern were found.");
            }
            else
            {
                Console.WriteLine("Found {0} file(s)", files.Length);
            }

            foreach (string file in files)
            {
                Console.WriteLine("Now creating organized file for source FASTA {0}...", file);
                var startTime = DateTime.Now;
                string line;
                string outputLineBuffer = "";
                string outputLine;
                // overlapping sequence from previous line:
                string prevOverlap = new string(' ', incrementOfPosStart / 2);
                // overlapping sequence from next line:
                string nextOverlap;

                using (StreamReader sr = new StreamReader(file))
                {
                    StreamWriter writer = GetOutputStream(file);
                    long firstPosOfLine = 1;
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            var seqPart = line.Split('\t')[1];
                            outputLineBuffer += seqPart;
                            var bufferLen = outputLineBuffer.Length;
                            if (bufferLen >= incrementOfPosStart)
                            {
                                outputLine = outputLineBuffer.Substring(0, incrementOfPosStart);
                                nextOverlap = outputLine.Substring(0, incrementOfPosStart / 2);
                                if (firstPosOfLine != 1)
                                {
                                    writer.WriteLine(nextOverlap);
                                }
                                writer.Write(firstPosOfLine + "\t" + prevOverlap + outputLine);
                                writer.Flush();
                                firstPosOfLine += incrementOfPosStart;
                                prevOverlap = outputLine.Substring(incrementOfPosStart / 2, incrementOfPosStart / 2);
                                outputLineBuffer = (bufferLen == incrementOfPosStart) ? ""
                                                   : outputLineBuffer.Substring(incrementOfPosStart, bufferLen - incrementOfPosStart);
                            }
                        }
                        int fillingLen = ((3 * incrementOfPosStart) / 2) - outputLineBuffer.Length;
                        nextOverlap = new string(' ', fillingLen);
                        outputLine = outputLineBuffer + nextOverlap;
                        // First close the previous line with the next overlap:
                        writer.WriteLine(outputLine.Substring(0, incrementOfPosStart / 2));
                        // Then write the last buffered line:
                        writer.WriteLine(firstPosOfLine + "\t" + prevOverlap + outputLine);
                        writer.Flush();
                        var endTime = DateTime.Now;
                        var elapsedTime = endTime - startTime;
                        Console.WriteLine("The output file is created for {0}. Elapsed time: " + elapsedTime, file);
                    }
                    finally
                    {
                        if (writer != null)
                            writer.Dispose();
                    }
                }
            }
        }

        private StreamWriter GetOutputStream(string file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileExt = Path.GetExtension(file);
            string outputFile = OutputPath + Path.DirectorySeparatorChar + fileName + ".fao";
            return new StreamWriter(outputFile);
        }
    }
}
