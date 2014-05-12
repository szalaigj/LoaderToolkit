using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mergers
{
    public class Ref : Merger
    {
        protected override string SourceTableName
        {
            get { return "ref"; }
        }

        protected override string TargetTableName
        {
            get { return "ref"; }
        }
    }
}
