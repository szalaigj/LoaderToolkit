using LoaderLibrary.CommandLineParser;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAdapter.Verbs
{
    [Verb(Name = "SamAdapt", Description = "Separate and transform header and alignment sections of a sam file.")]
    public class SamAdapt : Verb
    {
        public const string queryForRefIds = "SELECT"
                                           + " [refID], [extID]"
                                           + " FROM [$targetdb].[dbo].[refDesc] d"
                                           + " WHERE [refID] & 0xFFFF0000 = $speciesID";

        private string source;
        private string outputPath;
        private int samID;
        private int speciesID;
        private string targetDB;

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

        [Parameter(Name = "SpeciesID", Description = "The species ID which is a part of the allocated refID.", Required = true)]
        // Value of this property is the higher bit parts of the allocated refID.
        public int SpeciesID
        {
            get { return speciesID; }
            set { speciesID = value; }
        }

        [Parameter(Name = "TargetDB", Description = "Database to use as target.", Required = true)]
        public string TargetDB
        {
            get { return targetDB; }
            set { targetDB = value; }
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
            // Load the extID (rname) - refID mappings:
            var extIDsToRefIDs = LoadRelatedRefIDsFromDB();

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
                        }
                        else
                        {
                            var lineParts = line.Split('\t');
                            int refID;
                            if (extIDsToRefIDs.TryGetValue(lineParts[2], out refID))
                            {
                                string modifiedLine = lineParts[0] + "\t" + lineParts[1] + "\t" + refID;
                                int index = 3;
                                while (index < lineParts.Length)
                                {
                                    modifiedLine += "\t" + lineParts[index];
                                    index++;
                                }
                                writerForAlignment.WriteLine(samID + "\t" + modifiedLine);
                                writerForAlignment.Flush();
                            }
                            else
                            {
                                Console.WriteLine("WARNING: {0}. row of source file" + 
                                    "contains rname '{1}' which is not in the refDesc of target db.", firstPosOfLine, lineParts[2]);
                            }
                        }
                        firstPosOfLine++;
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

        private Dictionary<string, int> LoadRelatedRefIDsFromDB()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            using (var context = new DatabaseContext())
            {
                string sql = queryForRefIds;
                sql = sql.Replace("$targetdb", targetDB);
                sql = sql.Replace("$speciesID", speciesID.ToString());
                using (var cmd = new SqlCommand(sql, context.Connection, context.Transaction))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            LoadFromDataReader(dr, result);
                        }  
                    }
                }
                context.Commit();
            }
            return result;
        }

        private void LoadFromDataReader(SqlDataReader dr, Dictionary<string, int> dict)
        {
            int o = -1;
            var refID = dr.GetInt32(++o);
            var extID = dr.GetString(++o);
            dict.Add(extID, refID);
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
