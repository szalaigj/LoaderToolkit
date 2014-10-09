using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class FilteredPileup : SimplePileup
    {
        public override string TableName
        {
            get { return "fdpLoad"; }
        }

        public override string PreferredSourceFileExt
        {
            get { return ".fdp"; }
        }

        protected override void MapFirstToken(string firstToken)
        {
            string[] firstTokenParts = firstToken.Split('_');
            // [sampleName] [varchar](16) NOT NULL
            BulkWriter.WriteVarChar(firstTokenParts[0], 16);

            // [speciesID] [int] NOT NULL
            BulkWriter.WriteInt(Int32.Parse(firstTokenParts[1]));
        }
    }
}
