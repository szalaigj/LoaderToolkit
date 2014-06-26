using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mergers
{
    public class Gtf : Merger
    {
        protected override string SourceTableName
        {
            get { return "gtf"; }
        }

        protected override string TargetTableName
        {
            get { return "gtf"; }
        }
    }
}
