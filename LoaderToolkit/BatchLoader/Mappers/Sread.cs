using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class Sread : BaseSread
    {
        public override string TableName
        {
            get { return "sreadLoad"; }
        }

        protected override void HandleSeq(string seq)
        {
            // [seq] [varchar](8000) NOT NULL
            BulkWriter.WriteVarChar(seq, 8000);
        }
    }
}
