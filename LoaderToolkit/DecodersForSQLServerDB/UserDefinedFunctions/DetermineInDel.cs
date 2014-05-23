//------------------------------------------------------------------------------
// <copyright file="CSSqlFunction.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;
using System.Collections;

public partial class UserDefinedFunctions
{
    private class InDelRow
    {
        public SqlBoolean InDel { get; set; }
        public SqlInt32 ChainLen { get; set; }
        public SqlString NucChain { get; set; }
    }

    public static void FillRowFromExtraAndMissingNuc(object tableTypeObject, out SqlBoolean inDel, out SqlInt32 chainLen, out SqlString nucChain)
    {
        var tableType = (InDelRow)tableTypeObject;

        inDel = tableType.InDel;
        chainLen = tableType.ChainLen;
        nucChain = tableType.NucChain;
    }

    [Microsoft.SqlServer.Server.SqlFunction(
        DataAccess = DataAccessKind.Read,
        TableDefinition = "inDel bit, chainLen int, nucChain nvarchar(100)",
        FillRowMethodName = "FillRowFromExtraAndMissingNuc")]
    public static IEnumerable DetermineInDel(SqlInt64 posStart, SqlString indel)
    {
        ArrayList resultList = new ArrayList();
        string patternForIndelEntry = @"[0-9]+[-\+]+[a-zA-Z]+";
        MatchCollection matches = Regex.Matches(indel.Value, patternForIndelEntry);
        foreach (Match match in matches)
        {
            var foundIndel = match.Value;
            var isInsertion = Regex.IsMatch(foundIndel, @"^[0-9]+[\+]+[a-zA-Z]+$");
            char splittingChar = isInsertion ? '+' : '-';
            string[] foundIndelParts = foundIndel.Split(splittingChar);
            long indelStartPos = posStart.Value + Int32.Parse(foundIndelParts[0]);
            var nucChainValue = foundIndelParts[1];
            resultList.Add(new InDelRow { InDel = isInsertion, ChainLen = nucChainValue.Length, NucChain = nucChainValue });
        }
        return resultList;
    }
}
