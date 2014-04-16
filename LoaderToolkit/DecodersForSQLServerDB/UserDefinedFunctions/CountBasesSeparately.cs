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
using System.Collections.Generic;

public partial class UserDefinedFunctions
{

    private class Counter
    {
        public SqlInt32 A { get; set; }
        public SqlInt32 C { get; set; }
        public SqlInt32 G { get; set; }
        public SqlInt32 T { get; set; }
    }

    public static void FillRowFromBases(object tableTypeObject, out SqlInt32 a, out SqlInt32 c,
        out SqlInt32 g, out SqlInt32 t)
    {
        var tableType = (Counter)tableTypeObject;

        a = tableType.A;
        c = tableType.C;
        g = tableType.G;
        t = tableType.T;
    }

    [Microsoft.SqlServer.Server.SqlFunction(
        DataAccess = DataAccessKind.Read, 
        TableDefinition = "A int, C int, G int, T int", 
        FillRowMethodName = "FillRowFromBases")]
    public static IEnumerable CountBasesSeparately(SqlString bases, SqlString refNuc)
    {
        string basesValue = bases.Value;
        char[] charsOfBases = basesValue.ToCharArray();
        string refNucValue = refNuc.Value;
        Dictionary<string, int> counterInDict = new Dictionary<string, int>()
        {
            {"A", 0}, {"C", 0}, {"G", 0}, {"T", 0}
        };
        foreach (char chr in charsOfBases)
        {
            var chrStr = chr.ToString();
            if (".".Equals(chrStr) || ",".Equals(chrStr))
            {
                counterInDict[refNucValue]++;
            }
            else if (!"*".Equals(chrStr))
            {
                counterInDict[chrStr.ToUpper()]++;
            }
        }
        Counter result = new Counter { A = counterInDict["A"], C = counterInDict["C"],
            G = counterInDict["G"], T = counterInDict["T"]};
        return new ArrayList { result };
    }
}
