//------------------------------------------------------------------------------
// <copyright file="CSSqlFunction.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using BinaryCodec;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class UserDefinedFunctions
{
    private class NucSeparator
    {
        public SqlInt64 refPos { get; set; }
        public SqlString refNuc { get; set; }
        public SqlInt32 coverage { get; set; }
        public SqlInt32 A { get; set; }
        public SqlInt32 C { get; set; }
        public SqlInt32 G { get; set; }
        public SqlInt32 T { get; set; }
        public SqlString triplet { get; set; }
    }

    public static void FillRowFromRefSeqBlockAndMisIndel(object tableTypeObject, out SqlInt64 refPos, out SqlString refNuc,
        out SqlInt32 coverage, out SqlInt32 a, out SqlInt32 c, out SqlInt32 g, out SqlInt32 t, out SqlString triplet)
    {
        var tableType = (NucSeparator)tableTypeObject;

        refPos = tableType.refPos;
        refNuc = tableType.refNuc;
        coverage = tableType.coverage;
        a = tableType.A;
        c = tableType.C;
        g = tableType.G;
        t = tableType.T;
        triplet = tableType.triplet;
    }

    [Microsoft.SqlServer.Server.SqlFunction(
        DataAccess = DataAccessKind.Read,
        TableDefinition = "refPos bigint, refNuc nvarchar(1), coverage int, A int, C int, G int, T int, triplet nvarchar(3)",
        FillRowMethodName = "FillRowFromRefSeqBlockAndMisIndel")]
    public static IEnumerable DetNucDistr(SqlInt64 refPosStart, SqlBinary refSeq, SqlInt64 sreadPosStart, SqlInt64 sreadPosEnd,
        SqlString misMNuc, SqlString indel)
    {
        ArrayList result = new ArrayList();
        // The following is used because of triplet:
        string decodedRelatedRefSeqBlock = BinaryNucleotideCodecUtil.DetermineRefSeqBlockWithPrecAndSuccNucs(sreadPosStart.Value,
            sreadPosEnd.Value, refPosStart.Value, refSeq.Value);
        List<long> deletionPositions = DetermineDelPositions(sreadPosStart.Value, indel.Value);
        Dictionary<long, string> mutationPositions = DetermineMutPositions(sreadPosStart.Value, misMNuc.Value);
        long actRefPos = sreadPosStart.Value;
        // The refIndex is one-based because of triplet:
        int refIndex = 1;
        // The last element of decodedRelatedRefSeqBlock is not needed because it is only triplet:
        while (refIndex < decodedRelatedRefSeqBlock.Length - 1)
        {
            RecordActNucDetail(result, decodedRelatedRefSeqBlock, deletionPositions, mutationPositions, actRefPos, refIndex);
            refIndex++;
            actRefPos++;
        }
        return result;
    }

    private static List<long> DetermineDelPositions(long sreadPosStartValue, string indelValue)
    {
        List<long> deletionPositions = new List<long>();
        string deletionPattern = @"[0-9]+-[ACGTN]+";
        MatchCollection matches = Regex.Matches(indelValue, deletionPattern);
        foreach (Match match in matches)
        {
            var foundDeletion = match.Value;
            string[] foundDelParts = foundDeletion.Split('-');
            long delStartPos = sreadPosStartValue + Int32.Parse(foundDelParts[0]) + 1;
            var delLen = foundDelParts[1].Length;
            for (int index = 0; index < delLen; index++)
            {
                deletionPositions.Add(delStartPos + index);
            }
        }
        return deletionPositions;
    }

    private static Dictionary<long, string> DetermineMutPositions(long sreadPosStartValue, string misMNucValue)
    {
        Dictionary<long, string> mutationPositions = new Dictionary<long, string>();
        string mutationPattern = @"[0-9]+ [ACGTN]+";
        MatchCollection matches = Regex.Matches(misMNucValue, mutationPattern);
        foreach (Match match in matches)
        {
            var foundMutation = match.Value;
            string[] foundMutParts = foundMutation.Split(' ');
            long mutStartPos = sreadPosStartValue + Int32.Parse(foundMutParts[0]);
            var mutNuc = foundMutParts[1];
            mutationPositions.Add(mutStartPos, mutNuc);
        }
        return mutationPositions;
    }

    private static void RecordActNucDetail(ArrayList result, string decodedRelatedRefSeqBlock, List<long> deletionPositions, Dictionary<long, string> mutationPositions, long actRefPos, int refIndex)
    {
        string actRefNuc = "" + decodedRelatedRefSeqBlock[refIndex];
        string actTriplet = DetermineTriplet(refIndex, actRefNuc, decodedRelatedRefSeqBlock);
        if (deletionPositions.Contains(actRefPos))
        {
            result.Add(new NucSeparator
            {
                refPos = actRefPos,
                refNuc = actRefNuc,
                coverage = 0,
                A = 0,
                C = 0,
                G = 0,
                T = 0,
                triplet = actTriplet
            });
        }
        else
        {
            string mutValue;
            if (mutationPositions.TryGetValue(actRefPos, out mutValue))
            {
                SwitchByDiscriminatorNuc(result, actRefPos, actRefNuc, mutValue, actTriplet);
            }
            else
            {
                SwitchByDiscriminatorNuc(result, actRefPos, actRefNuc, actRefNuc, actTriplet);
            }
        }
    }

    private static string DetermineTriplet(int refIndex, string actRefNuc, string decodedRelatedRefSeqBlock)
    {
        var prevRefNuc = (decodedRelatedRefSeqBlock[refIndex - 1] == ' ') ? 'x' : decodedRelatedRefSeqBlock[refIndex - 1];
        var nextRefNuc = (decodedRelatedRefSeqBlock[refIndex + 1] == ' ') ? 'x' : decodedRelatedRefSeqBlock[refIndex + 1];
        return prevRefNuc + actRefNuc + nextRefNuc;
    }

    private static void SwitchByDiscriminatorNuc(ArrayList result, long actRefPos, string actRefNuc, string discriminatorNuc, string actTriplet)
    {
        NucSeparator nucSep = new NucSeparator { refPos = actRefPos, refNuc = actRefNuc, triplet = actTriplet };
        switch (discriminatorNuc)
        {
            case "A":
                nucSep.coverage = 1;
                nucSep.A = 1;
                nucSep.C = 0;
                nucSep.G = 0;
                nucSep.T = 0;
                break;
            case "C":
                nucSep.coverage = 1;
                nucSep.A = 0;
                nucSep.C = 1;
                nucSep.G = 0;
                nucSep.T = 0;
                break;
            case "G":
                nucSep.coverage = 1;
                nucSep.A = 0;
                nucSep.C = 0;
                nucSep.G = 1;
                nucSep.T = 0;
                break;
            case "T":
                nucSep.coverage = 1;
                nucSep.A = 0;
                nucSep.C = 0;
                nucSep.G = 0;
                nucSep.T = 1;
                break;
            case "N":
                nucSep.coverage = 0;
                nucSep.A = 0;
                nucSep.C = 0;
                nucSep.G = 0;
                nucSep.T = 0;
                break;
            default:
                throw new InvalidExpressionException("The mutation value is invalid in the column misMNuc.");
        }
        result.Add(nucSep);
    }
}
