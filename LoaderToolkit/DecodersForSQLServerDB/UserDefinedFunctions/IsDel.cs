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
    public static SqlBoolean IsDel(SqlInt64 posStart, SqlString indel, SqlInt64 refPos)
    {
        List<long> deletionPositions = new List<long>();
        string deletionPattern = @"[0-9]+-[ACGTN]+";
        MatchCollection matches = Regex.Matches(indel.Value, deletionPattern);
        foreach (Match match in matches)
        {
            var foundDeletion = match.Value;
            string[] foundDelParts = foundDeletion.Split('-');
            long delStartPos = posStart.Value + Int32.Parse(foundDelParts[0]) + 1;
            var delLen = foundDelParts[1].Length;
            for (int index = 0; index < delLen; index++)
            {
                deletionPositions.Add(delStartPos + index);
            }
        }
        return new SqlBoolean(deletionPositions.Contains(refPos.Value));
    }
}
