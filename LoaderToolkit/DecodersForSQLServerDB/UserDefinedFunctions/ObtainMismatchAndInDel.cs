//------------------------------------------------------------------------------
// <copyright file="CSSqlFunction.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    private class MismatchInDelRow
    {
        public SqlString Mismatch { get; set; }
        public SqlString Indel { get; set; }
    }

    public static void FillRowFromSequencesAndExtraAndMissingNuc(object tableTypeObject, out SqlString mismatch, out SqlString indel)
    {
        var tableType = (MismatchInDelRow)tableTypeObject;

        mismatch = tableType.Mismatch;
        indel = tableType.Indel;
    }

    [Microsoft.SqlServer.Server.SqlFunction(
        DataAccess = DataAccessKind.Read,
        TableDefinition = "misMNuc nvarchar(4000), indel nvarchar(4000)",
        FillRowMethodName = "FillRowFromSequencesAndExtraAndMissingNuc")]
    public static IEnumerable ObtainMismatchAndInDel(SqlString refSeq, SqlString sreadSeq, SqlString insPos, SqlString delPos)
    {
        string resultMismatch = "";
        string resultIndel = "";
        Dictionary<int, int> insOffsetLen = DetermineOffsetLen(insPos);
        Dictionary<int, int> delOffsetLen = DetermineOffsetLen(delPos);
        string refSeqValue = refSeq.Value;
        char[] nucsOfRefSeq = refSeqValue.ToCharArray();
        string sreadSeqValue = sreadSeq.Value;
        char[] nucsOfSreadSeq = sreadSeqValue.ToCharArray();
        int refIndex = 0;
        int sreadIndex = 0;
        while ((refIndex < nucsOfRefSeq.Length) && (sreadIndex < nucsOfSreadSeq.Length))
        {
            var sreadNuc = nucsOfSreadSeq[sreadIndex];
            if (nucsOfRefSeq[refIndex] != sreadNuc)
            {
                resultMismatch += refIndex + " " + sreadNuc + "\t";
            }
            int length;
            if (insOffsetLen.TryGetValue(refIndex, out length))
            {
                RecordInsertion(ref resultIndel, sreadSeqValue, ref refIndex, ref sreadIndex, length);
            }
            else if (delOffsetLen.TryGetValue(refIndex, out length))
            {
                RecordDeletion(ref resultIndel, refSeqValue, ref refIndex, ref sreadIndex, length);
            }
            else
            {
                StepIndecesNormally(ref refIndex, ref sreadIndex);
            }
        }
        MismatchInDelRow result = new MismatchInDelRow { Mismatch = new SqlString(resultMismatch), Indel = new SqlString(resultIndel) };
        return new ArrayList { result };
    }

    private static Dictionary<int, int> DetermineOffsetLen(SqlString pos)
    {
        Dictionary<int, int> result = new Dictionary<int, int>();
        if (!pos.IsNull)
        {
            string strPos = pos.Value;
            var splittingResult = strPos.Split('\t');
            string[] positions = new string[splittingResult.Length - 1];
            Array.Copy(splittingResult, positions, positions.Length);
            foreach (var position in positions)
            {
                var partsOfPosition = position.Split(' ');
                int offset = Int32.Parse(partsOfPosition[0]);
                int length = Int32.Parse(partsOfPosition[1]);
                result.Add(offset, length);
            }
        }
        return result;
    }

    private static void RecordInsertion(ref string resultIndel, string sreadSeqValue, ref int refIndex, ref int sreadIndex, int length)
    {
        resultIndel += refIndex + "+" + sreadSeqValue.Substring(sreadIndex + 1, length) + "\t";
        sreadIndex += length + 1;
        refIndex++;
    }

    private static void RecordDeletion(ref string resultIndel, string refSeqValue, ref int refIndex, ref int sreadIndex, int length)
    {
        resultIndel += refIndex + "-" + refSeqValue.Substring(refIndex + 1, length) + "\t";
        refIndex += length + 1;
        sreadIndex++;
    }

    private static void StepIndecesNormally(ref int refIndex, ref int sreadIndex)
    {
        refIndex++;
        sreadIndex++;
    }

    [Microsoft.SqlServer.Server.SqlFunction(
        DataAccess = DataAccessKind.Read,
        TableDefinition = "misMNuc nvarchar(4000), indel nvarchar(4000)",
        FillRowMethodName = "FillRowFromSequencesAndExtraAndMissingNuc")]
    public static IEnumerable ObtainMismatchAndInDelBin(SqlBinary refSeq, SqlInt64 refPosStart, SqlBinary sreadSeq, SqlInt64 sreadPosStart,
        SqlString insPos, SqlString delPos)
    {
        Dictionary<int, string> decodedNucleotides = new Dictionary<int, string>() 
        { 
             {0x8, "A"}, 
             {0x9, "C"}, 
             {0xA, "G"}, 
             {0xB, "T"}, 
             {0xC, "N"},
             {0X4, " "} 
        };
        string resultMismatch = "";
        string resultIndel = "";
        Dictionary<int, int> insOffsetLen = DetermineOffsetLen(insPos);
        Dictionary<int, int> delOffsetLen = DetermineOffsetLen(delPos);
        byte[] sreadSeqValue = sreadSeq.Value;
        string decodedSreadSeq = DetermineDecodedSeq(decodedNucleotides, sreadSeqValue);
        long offset = sreadPosStart.Value - refPosStart.Value;
        bool isOffsetEven = (offset % 2 == 0);
        var startRefSeqByteOffset = (int)(offset / 2 + 64);
        byte[] refSeqValue = refSeq.Value;
        string decodedRelatedRefSeqBlock;
        if (isOffsetEven)
        {
            byte[] relatedRefSeqBlock = new byte[sreadSeqValue.Length];
            Array.Copy(refSeqValue, startRefSeqByteOffset, relatedRefSeqBlock, 0, sreadSeqValue.Length);
            decodedRelatedRefSeqBlock = DetermineDecodedSeq(decodedNucleotides, relatedRefSeqBlock);
        }
        else
        {
            byte[] relatedRefSeqBlock = new byte[sreadSeqValue.Length - 1];
            Array.Copy(refSeqValue, startRefSeqByteOffset + 1, relatedRefSeqBlock, 0, sreadSeqValue.Length - 1);
            decodedRelatedRefSeqBlock = DetermineDecodedSeq(decodedNucleotides, relatedRefSeqBlock);
            byte maskForLowerBits = 0x0F;
            var encLastNucl = refSeqValue[startRefSeqByteOffset] & maskForLowerBits;
            decodedRelatedRefSeqBlock = decodedNucleotides[encLastNucl] + decodedRelatedRefSeqBlock;
        }

        int refIndex = 0;
        int sreadIndex = 0;
        while ((refIndex < decodedRelatedRefSeqBlock.Length) && (sreadIndex < decodedSreadSeq.Length))
        {
            var sreadNuc = decodedSreadSeq[sreadIndex];
            if (decodedRelatedRefSeqBlock[refIndex] != sreadNuc)
            {
                resultMismatch += refIndex + " " + sreadNuc + "\t";
            }
            int length;
            if (insOffsetLen.TryGetValue(refIndex, out length))
            {
                RecordInsertion(ref resultIndel, decodedSreadSeq, ref refIndex, ref sreadIndex, length);
            }
            else if (delOffsetLen.TryGetValue(refIndex, out length))
            {
                RecordDeletion(ref resultIndel, decodedRelatedRefSeqBlock, ref refIndex, ref sreadIndex, length);
            }
            else
            {
                StepIndecesNormally(ref refIndex, ref sreadIndex);
            }
        }
        MismatchInDelRow result = new MismatchInDelRow { Mismatch = new SqlString(resultMismatch), Indel = new SqlString(resultIndel) };
        return new ArrayList { result };
    }

    private static string DetermineDecodedSeq(Dictionary<int, string> decodedNucleotides, byte[] byteSeq)
    {
        string decodedSeq = "";
        foreach (byte encNucPair in byteSeq)
        {
            byte maskForHigherBits = 0xF0;
            var encFirstNucl = (encNucPair & maskForHigherBits) >> 4;
            byte maskForLowerBits = 0x0F;
            var encLastNucl = encNucPair & maskForLowerBits;
            decodedSeq = decodedSeq + decodedNucleotides[encFirstNucl] + decodedNucleotides[encLastNucl];
        }
        decodedSeq = decodedSeq.TrimEnd(' ');
        return decodedSeq;
    }
}
