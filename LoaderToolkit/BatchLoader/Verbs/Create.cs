using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LoaderLibrary.Load;
using LoaderLibrary.CommandLineParser;
using System.Reflection;
using StructureMap;

namespace BatchLoader.Verbs
{
    [Verb(Name = "Create", Description = "Create new batch.")]
    class Create : BatchLoaderVerb
    {
        private string mode;
        private string source;
        private string fileSuffix;
        private string bulkPath;
        private string targetDB;
        private string loaderDB;
        private string server;
        private bool integratedSecurity;
        private string userID;
        private string password;
        private bool binary;

        [Parameter(Name = "Mode", Description = "Mapper/Merger class names which are used for load/target tables.", Required = true)]
        public override string Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        [Parameter(Name = "Source", Description = "Source file pattern.", Required = true)]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        [Parameter(Name = "FileSuffix", Description = "The suffix of the file name (without extension) which occurs in (all) files.", Required = false)]
        public string FileSuffix
        {
            get { return fileSuffix; }
            set { fileSuffix = value; }
        }

        [Parameter(Name = "BulkPath", Description = "Bulk load files' path", Required = true)]
        public string BulkPath
        {
            get { return bulkPath; }
            set { bulkPath = value; }
        }

        [Parameter(Name = "TargetDB", Description = "Database to use as target.")]
        public string TargetDB
        {
            get { return targetDB; }
            set { targetDB = value; }
        }

        [Parameter(Name = "LoaderDB", Description = "Database to use for loading.")]
        public string LoaderDB
        {
            get { return loaderDB; }
            set { loaderDB = value; }
        }

        [Parameter(Name = "Server", Description = "Database server with target and load DB co-located.")]
        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        [Option(Name = "EnableIntegratedSecurity", Description = "Use Windows login.")]
        public bool IntegratedSecurity
        {
            get { return integratedSecurity; }
            set { integratedSecurity = value; }
        }

        [Parameter(Name = "UserId", Description = "User ID.")]
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        [Parameter(Name = "Password", Description = "Password")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [Option(Name = "Binary", Description = "User binary files for bulk insert.")]
        public bool Binary
        {
            get { return binary; }
            set { binary = value; }
        }

        public Create()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.source = null;
            this.bulkPath = null;
            this.targetDB = Constants.DefaultTargetDB;
            this.loaderDB = Constants.DefaultLoaderDB;
            this.server = Constants.DefaultServer;
            this.integratedSecurity = true;
            this.userID = String.Empty;
            this.password = String.Empty;
            this.binary = false;
        }

        public override void Run()
        {
            // Find files matching pattern
            var dir = Path.GetDirectoryName(Source);
            var pat = Path.GetFileName(Source);
            var ext = Path.GetExtension(Source);
            var files = Directory.GetFiles(dir, pat);

            var currentMapper = ObjectFactory.GetInstance<Mapper<string>>();
            if (!currentMapper.PreferredSourceFileExt.Equals(ext))
            {
                Console.WriteLine("WARNING: the extension of the given Source does not equal the " +
                    "preferred file extension of the given Mapper type! Are you sure you want to use it?");
            }

            if (files.Length == 0)
            {
                throw new ArgumentException("No files matching the source pattern were found.");
            }
            else
            {
                Console.WriteLine("Found {0} file(s), now creating batch...", files.Length);
            }

            var b = new Batch();

            using (var context = new DatabaseContext())
            {
                // Create batch object
                
                b.SourcePath = Path.GetDirectoryName(Source);
                b.FileSuffix = FileSuffix;
                b.BulkPath = BulkPath;

                b.TargetDB.DataSource = Server;
                b.TargetDB.InitialCatalog = TargetDB;
                b.TargetDB.IntegratedSecurity = IntegratedSecurity;
                if (!IntegratedSecurity)
                {
                    b.TargetDB.UserID = UserID;
                    b.TargetDB.Password = Password;
                }

                b.LoaderDB.DataSource = Server;
                b.LoaderDB.InitialCatalog = LoaderDB;
                b.LoaderDB.IntegratedSecurity = IntegratedSecurity;
                if (!IntegratedSecurity)
                {
                    b.LoaderDB.UserID = UserID;
                    b.LoaderDB.Password = Password;
                }

                b.Binary = binary;

                // Create chunks
                foreach (var f in files)
                {
                    var c = new Chunk();

                    c.ChunkId = Path.GetFileName(f);
                    b.Chunks.Add(c);
                }

                b.Save(context);
                b.CreateChunks(context);

                context.Commit();
            }

            Console.WriteLine("New batch created with ID {0}", b.BatchID);

        }
    }
}
