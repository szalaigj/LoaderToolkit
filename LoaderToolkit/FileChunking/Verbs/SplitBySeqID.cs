using LoaderLibrary.CommandLineParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChunking.Verbs
{
    [Verb(Name = "SplitBySeqID", Description = "Chunks up files by sequence ID.")]
    public class SplitBySeqID : Verb
    {
        private string source;
        private string outputPath;
        private int lastRefID;
        private int lineLength;

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

        [Parameter(Name = "LastRefID", Description = "The last ref ID of the DB.")]
        public int LastRefID
        {
            get { return lastRefID; }
            set { lastRefID = value; }
        }

        [Parameter(Name = "LineLength", Description = "The length of one line.")]
        public int LineLength
        {
            get { return lineLength; }
            set { lineLength = value; }
        }

        public SplitBySeqID()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.source = null;
            this.outputPath = null;
            this.lastRefID = 0;
            this.lineLength = 60;
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
                using (StreamReader sr = new StreamReader(file))
                {
                    String line;
                    int fileIndex = lastRefID;
                    Console.WriteLine("Now creating chunked output files...");
                    StreamWriter writerForSeqDesc = GetOutputStreamForSeqDesc(file);
                    StreamWriter writerBySeqID = null;
                    var startTime = DateTime.Now;
                    long firstPosOfLine = 1;
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith(">"))
                            {
                                fileIndex++;
                                WriteOutSeqDesc(line, fileIndex, writerForSeqDesc);
                                if (writerBySeqID == null)
                                {
                                    writerBySeqID = GetOutputStream(file, fileIndex);
                                }
                                else
                                {
                                    writerBySeqID.Flush();
                                    var endTime = DateTime.Now;
                                    var elapsedTime = endTime - startTime;
                                    Console.WriteLine("{0}. output file is created. Elapsed time: " + elapsedTime, fileIndex - 1);
                                    writerBySeqID = GetOutputStream(file, fileIndex);
                                    startTime = DateTime.Now;
                                }
                            }
                            else
                            {
                                writerBySeqID.WriteLine(firstPosOfLine + "\t" + line);
                                firstPosOfLine += lineLength;
                            }
                        }
                        // For handling the last output file:
                        writerBySeqID.Flush();
                    }
                    finally
                    {
                        if (writerForSeqDesc != null)
                            writerForSeqDesc.Dispose();
                        if (writerBySeqID != null)
                            writerBySeqID.Dispose();
                    }
                }
            }
        }

        private static void WriteOutSeqDesc(String line, int fileIndex, StreamWriter writerForSeqDesc)
        {
            var delimiterPosOfSeqID = line.IndexOf(" ");
            // Skip character '>':
            var seqID = line.Substring(1, delimiterPosOfSeqID);
            var seqDesc = line.Substring(delimiterPosOfSeqID + 1);
            writerForSeqDesc.WriteLine("INSERT INTO [dbo].[refDesc] ([refID],[extID],[desc]) VALUES (" + fileIndex + ",'" + seqID + "','" + seqDesc + "')");
            writerForSeqDesc.Flush();
        }

        private StreamWriter GetOutputStreamForSeqDesc(string file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileExt = Path.GetExtension(file);
            string outputFile = OutputPath + "\\" + fileName + "_refdesc.dsc";
            return new StreamWriter(outputFile);
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
