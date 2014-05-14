using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mergers
{
    public class Sam : Merger
    {
        protected override string SourceTableName
        {
            get { return "sam"; }
        }

        protected override string TargetTableName
        {
            get { return "sam"; }
        }
    }
}
