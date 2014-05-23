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
using System.Collections.Generic;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlInt32 IsNucX(SqlInt64 posStart, SqlString misMNuc, SqlInt64 refPos, SqlString refNuc, SqlString countNuc)
    {
        SqlInt32 result;
        Dictionary<long, string> mutationPositions = new Dictionary<long, string>();
        string mutationPattern = @"[0-9]+ [ACGTN]+";
        MatchCollection matches = Regex.Matches(misMNuc.Value, mutationPattern);
        foreach (Match match in matches)
        {
            var foundMutation = match.Value;
            string[] foundMutParts = foundMutation.Split(' ');
            long mutStartPos = posStart.Value + Int32.Parse(foundMutParts[0]);
            var mutNuc = foundMutParts[1];
            mutationPositions.Add(mutStartPos, mutNuc);
        }
        string mutValue;
        if (mutationPositions.TryGetValue(refPos.Value, out mutValue))
        {
            result = new SqlInt32(countNuc.Value.Equals(mutValue) ? 1 : 0);
        }
        else
        {
            result = new SqlInt32(countNuc.Value.Equals(refNuc.Value) ? 1 : 0);
        }
        return result;
    }
}
