using LoaderLibrary.CommandLineParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAdapter.Verbs
{
    [Verb(Name = "SamAdapt", Description = "Separate and transform header and alignment sections of a sam file.")]
    public class SamAdapt : Verb
    {
        private string source;
        private string outputPath;
        private int samID;

        [Parameter(Name = "Source", Description = "Source file pattern.", Required = true)]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        [Parameter(Name = "OutputPath", Description = "Path of output (separeted and transformed) files.", Required = true)]
        public string OutputPath
        {
            get { return outputPath; }
            set { outputPath = value; }
        }

        [Parameter(Name = "SamID", Description = "The sam file identifier which will be the allocated, planned samID.")]
        // Value of this property is the sam file identifier which can be arbitrary but should not be equal to existing samID in the DB.
        public int SamID
        {
            get { return samID; }
            set { samID = value; }
        }

        public SamAdapt()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.source = null;
            this.outputPath = null;
            this.samID = 1;
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
            else if (files.Length > 1)
            {
                throw new ArgumentException("More than one file matching the source pattern were found.");
            }
            else
            {
                Console.WriteLine("Found the file.");
            }

            string file = files[0];

            using (StreamReader sr = new StreamReader(file))
            {
                String line;
                Console.WriteLine("Now creating output files...");
                StreamWriter writerForHeader = GetOutputStreamForHeaderSection(file);
                StreamWriter writerForAlignment = GetOutputStreamForAlignmentSection(file);
                long firstPosOfLine = 1;
                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("@"))
                        {
                            // Skip character '@':
                            var lineWithoutAtSign = line.Substring(1);
                            writerForHeader.WriteLine(samID + "\t" + firstPosOfLine + "\t" + lineWithoutAtSign);
                            writerForHeader.Flush();
                            firstPosOfLine++;
                        }
                        else
                        {
                            writerForAlignment.WriteLine(samID + "\t" + line);
                            writerForAlignment.Flush();
                        }
                    }
                }
                finally
                {
                    if (writerForHeader != null)
                        writerForHeader.Dispose();
                    if (writerForAlignment != null)
                        writerForAlignment.Dispose();
                }
            }
        }

        private StreamWriter GetOutputStreamForHeaderSection(string file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileExt = Path.GetExtension(file);
            string outputFile = OutputPath + Path.DirectorySeparatorChar + fileName + ".hdr";
            return new StreamWriter(outputFile);
        }

        private StreamWriter GetOutputStreamForAlignmentSection(string file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileExt = Path.GetExtension(file);
            string outputFile = OutputPath + Path.DirectorySeparatorChar + fileName + ".aln";
            return new StreamWriter(outputFile);
        }
    }
}
