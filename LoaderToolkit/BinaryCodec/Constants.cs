using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCodec
{
    public static class Constants
    {
        public enum CodecDomainNames { RefNuc, Bases, BasesQual }

        public enum ColumnsFromSkipChars { ExtraNuc, MissingNuc, StartingSigns, MappingQual, EndingSigns }

        public const string separator = "\t";
    }
}
