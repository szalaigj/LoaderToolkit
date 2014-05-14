using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mergers
{
    public class Sread : Merger
    {
        protected override string SourceTableName
        {
            get { return "sreadLoad"; }
        }

        protected override string TargetTableName
        {
            get { return "sread"; }
        }
    }
}
