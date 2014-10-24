using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mergers
{
    public class Cover : Merger
    {
        protected override string SourceTableName
        {
            get { return "coverLoad"; }
        }

        protected override string TargetTableName
        {
            get { return "cover"; }
        }
    }
}
