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

            // [qname] [varchar](150) NOT NULL
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
            
            long posStart;
            long posEnd;
            string insPos;
            string delPos;
            ProcessCigar(cigar, pos, out posStart, out posEnd, out insPos, out delPos);

            insPos = (string.Empty.Equals(insPos)) ? null : insPos;
            delPos = (string.Empty.Equals(delPos)) ? null : delPos;

            // [insPos] [varchar](8000) NULL
            BulkWriter.WriteVarChar(insPos, 8000);

            // [delPos] [varchar](8000) NULL
            BulkWriter.WriteVarChar(delPos, 8000);

            // [posStart] [bigint] NOT NULL
            BulkWriter.WriteBigInt(posStart);

            // [posEnd] [bigint] NOT NULL
            BulkWriter.WriteBigInt(posEnd);

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

        private void ProcessCigar(string cigar, string pos, out long posStart, out long posEnd, 
            out string insPos, out string delPos)
        {
            insPos = "";
            delPos = "";
            // This regex pattern contains positive lookbehind expressions
            // which is needed for zero-length matchs
            string pattern = @"(?<=M)|(?<=I)|(?<=D)|(?<=N)|(?<=S)|(?<=H)|(?<=P)|(?<=\=)|(?<=X)";
            // an element of array cigarOperations will contain one integer and one operation type id:
            // (note: the last element is empty so it is skipped)
            var results = Regex.Split(cigar, pattern);
            string[] cigarOperations = new string[results.Length-1];
            Array.Copy(results, cigarOperations, cigarOperations.Length);
            int innerPos = ProcessOperations(ref insPos, ref delPos, cigarOperations);
            posStart = Int64.Parse(pos);
            posEnd = posStart + innerPos - 1;
        }

        private int ProcessOperations(ref string insPos, ref string delPos, string[] cigarOperations)
        {
            // the relative position related to the SEQ which is 0-based:
            // note: the clipping and insertion operations should be ignored
            //       so when the end of CIGAR string is reached it is NOT sure the innerPos will equal to length of SEQ
            int innerPos = 0;
            foreach (string op in cigarOperations)
            {
                var opLen = op.Substring(0, op.Length - 1);
                var opLenValue = Int32.Parse(opLen);
                var opType = op.Substring(op.Length - 1);
                if (!"S".Equals(opType) && !"H".Equals(opType))
                {
                    if ("I".Equals(opType))
                    {
                        insPos += recordPos(innerPos - 1, opLen);
                    }
                    else if (("D".Equals(opType)) || ("N".Equals(opType)))
                    {
                        delPos += recordPos(innerPos - 1, opLen);
                        innerPos += opLenValue;
                    }
                    else
                    {
                        innerPos += opLenValue;
                    }
                }
            }
            return innerPos;
        }

        /// <summary>
        /// The record will contain only the position which is relative to posStart.
        /// </summary>
        /// <param name="actualOffsetPos"></param>
        /// <param name="opLen"></param>
        /// <returns></returns>
        private string recordPos(long actualOffsetPos, string opLen)
        {
            return actualOffsetPos + " " + opLen + "\t";
        }
    }
}
