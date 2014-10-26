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
using System.Collections;

public partial class UserDefinedFunctions
{
    private class SubstrCount
    {
        public SqlString Substr { get; set; }
        public SqlInt64 Cnt { get; set; }
    }

    public static void FillRowFromSeqStartEnd(object tableTypeObject, out SqlString substr, out SqlInt64 cnt)
    {
        var tableType = (SubstrCount)tableTypeObject;

        substr = tableType.Substr;
        cnt = tableType.Cnt;
    }

    [Microsoft.SqlServer.Server.SqlFunction(
        DataAccess = DataAccessKind.Read,
        TableDefinition = "substr nvarchar, cnt bigint",
        FillRowMethodName = "FillRowFromSeqStartEnd")]
    public static IEnumerable DetSubstrCount([Microsoft.SqlServer.Server.SqlFacet(MaxSize = -1)] SqlString input)
    {
        var inputValue = input.Value;
        ArrayList resultList = new ArrayList();
        string[] inputEntries = inputValue.Split('\t');
        foreach (var entry in inputEntries)
        {
            string[] entryValues = entry.Split(' ');
            resultList.Add(new SubstrCount() { Substr = entryValues[0], Cnt = SqlInt64.Parse(entryValues[1]) });
        }
        return resultList;
    }
}
