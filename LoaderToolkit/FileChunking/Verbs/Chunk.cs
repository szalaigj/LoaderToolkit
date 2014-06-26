using LoaderLibrary.CommandLineParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChunking.Verbs
{
    [Verb(Name = "Chunk", Description = "Chunks up files.")]
    public class Chunk : Verb
    {
        private string source;
        private string outputPath;
        private int outputMeasure;
        private int skipFirstLines;

        [Parameter(Name = "Source", Description = "Source file pattern.", Required = true)]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        [Parameter(Name = "OutputPath", Description = "Path of output (chunked) files.", Required = true)]
        public string OutputPath
        {
            get { return outputPath; }
            set { outputPath = value; }
        }

        [Parameter(Name = "OutputMeasure", Description = "Number of output file pieces.")]
        public int OutputMeasure
        {
            get { return outputMeasure; }
            set { outputMeasure = value; }
        }

        [Parameter(Name = "SkipFirstLines", Description = "Number of first lines which should be skipped.")]
        public int SkipFirstLines
        {
            get { return skipFirstLines; }
            set { skipFirstLines = value; }
        }

        public Chunk()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.source = null;
            this.outputPath = null;
            this.outputMeasure = 32;
            this.skipFirstLines = 0;
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
                var startTime = DateTime.Now;
                Console.WriteLine("Computing number of lines of input file...");
                int lineCount = File.ReadLines(file).Count();
                var endTime = DateTime.Now;
                TimeSpan elapsedTime = endTime - startTime;
                Console.WriteLine("Number of lines is computed. Elapsed time: " + elapsedTime);
                int lineCountOfOutputFiles = lineCount / OutputMeasure;
                Console.WriteLine("Note: the first {0} lines will be skipped", skipFirstLines);
                int firstLinesCounter = 0;
                using (StreamReader sr = new StreamReader(file))
                {
                    String line;
                    int cntOfLines = 0;
                    int fileIndex = 0;
                    Console.WriteLine("Now creating chunked output files...");
                    StreamWriter writer = GetOutputStream(file, fileIndex);
                    startTime = DateTime.Now;
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (firstLinesCounter < skipFirstLines)
                            {
                                firstLinesCounter++;
                            }
                            else
                            {
                                // The last output file will contain the remainder lines also:
                                if ((cntOfLines == lineCountOfOutputFiles - 1) && (fileIndex != OutputMeasure - 1))
                                {
                                    writer.WriteLine(line);
                                    writer.Flush();
                                    cntOfLines = 0;
                                    fileIndex++;
                                    endTime = DateTime.Now;
                                    elapsedTime = endTime - startTime;
                                    Console.WriteLine("{0}. output file is created. Elapsed time: " + elapsedTime, fileIndex);
                                    writer = GetOutputStream(file, fileIndex);
                                    startTime = DateTime.Now;
                                }
                                else
                                {
                                    cntOfLines++;
                                    writer.WriteLine(line);
                                }
                            }
                        }
                        // For handling the last output file:
                        writer.Flush();
                    }
                    finally
                    {
                        if (writer != null)
                            writer.Dispose();
                    }
                }
            }
        }

        private StreamWriter GetOutputStream(string file, int fileIndex)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileExt = Path.GetExtension(file);
            string outputFile = OutputPath +"\\"+ fileName +"_"+ fileIndex + fileExt;
            return new StreamWriter(outputFile);
        }
    }
}
