using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mergers
{
    public class BinaryEncodedPileup : Merger
    {
        protected override string SourceTableName
        {
            get { return "pileups"; }
        }

        protected override string TargetTableName
        {
            get { return "pileups"; }
        }
    }
}
