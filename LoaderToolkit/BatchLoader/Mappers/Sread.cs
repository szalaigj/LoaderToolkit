using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class Sread : Mapper<string>
    {
        public override string TableName
        {
            get { return "sreadLoad"; }
        }

        public override void Map(string obj)
        {
            string[] objParts = obj.Split('\t');

            // The objParts[0] is skipped because the filename is irrelevant.
            // [samID] [int] NOT NULL PRIMARY KEY
            BulkWriter.WriteInt(Int32.Parse(objParts[1]));

            var qname = objParts[2];
            var flag = objParts[3];
            var rname = objParts[4];
            var pos = objParts[5];
            var mapq = objParts[6];
            var cigar = objParts[7];
            var seq = objParts[11];
            var qual = objParts[12];

            // [extID] [varchar](150) NOT NULL
            BulkWriter.WriteVarChar(qname, 150);

            // [rname] [varchar](50) NOT NULL
            BulkWriter.WriteVarChar(rname, 50);

            // [dir] [bit] NOT NULL
            bool dir = DetermineDir(flag);
            BulkWriter.WriteBit(dir);

            // [mapq] [tinyint] NOT NULL
            BulkWriter.WriteTinyInt(SByte.Parse(mapq));

            // [seq] [varchar](8000) NOT NULL
            seq = EliminateSoftClippingPartsFromSeq(cigar, seq);
            BulkWriter.WriteVarChar(seq, 8000);

            // [inPos] [varchar](8000) NULL

            // [delPos] [varchar](8000) NULL

            // [posStart] [bigint] NOT NULL

            // [posEnd] [bigint] NOT NULL

            // [qual] [varchar](8000) NOT NULL
            BulkWriter.WriteVarChar(qual, 8000);

            BulkWriter.EndLine();
        }

        private bool DetermineDir(string flag)
        {
            var bitwiseFlag = UInt16.Parse(flag);
            // The fifth bit indicates the direction:
            var reversed = GetBit(bitwiseFlag, 5);
            return reversed;
        }

        private bool GetBit(ushort bitwiseFlag, int bitNumber)
        {
            var bit = (bitwiseFlag & (1 << bitNumber - 1)) != 0;
            return bit;
        }

        private string EliminateSoftClippingPartsFromSeq(string cigar, string seq)
        {
            string result = string.Copy(seq);
            string patternForSoftClippingAtTheEnd = @"\d+S$";
            string match = Regex.Match(cigar, patternForSoftClippingAtTheEnd).Value;
            if (!string.IsNullOrEmpty(match))
            {
                var endClippingLen = Int32.Parse(match.Substring(0, match.Length - 1));
                result = result.Substring(0, result.Length - endClippingLen);
            }

            string patternForSoftClippingAtTheBeginning = @"^\d+S";
            match = Regex.Match(cigar, patternForSoftClippingAtTheBeginning).Value;
            if (!string.IsNullOrEmpty(match))
            {
                var startClippingLen = Int32.Parse(match.Substring(0, match.Length - 1));
                result = result.Substring(startClippingLen);
            }
            return result;
        }

        private void ProcessCigar(string cigar, string pos, bool dir, long posStart, long posEnd, 
            out string insPos, out string delPos)
        {
            insPos = "";
            delPos = "";
            var posValue = Int64.Parse(pos);
            // This regex pattern contains positive lookbehind expressions
            // which is needed for zero-length matchs
            string pattern = @"(?<=M)|(?<=I)|(?<=D)|(?<=N)|(?<=S)|(?<=H)|(?<=P)|(?<=\=)|(?<=X)";
            // an element of array cigarOperations will contain one integer and one operation type id:
            // (note: the last element is empty so it is skipped)
            var results = Regex.Split(cigar, pattern);
            string[] cigarOperations = new string[results.Length-1];
            Array.Copy(results, cigarOperations, cigarOperations.Length);
            int innerPos = 0;
            foreach(string op in cigarOperations)
            {
                var opLen = op.Substring(0, op.Length - 1);
                var opLenValue = Int32.Parse(opLen);
                var opType = op.Substring(op.Length - 1);
                if (!"S".Equals(opType) && !"H".Equals(opType))
                {
                    if ("I".Equals(opType))
                    {
                        insPos += innerPos + " " + opLen + "\t";
                    }
                    innerPos += opLenValue;
                }
            }
        }
    }
}
