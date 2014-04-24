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
    public static IEnumerable DetermineInDel(SqlString exNuc, SqlString missNuc)
    {
        ArrayList resultList = new ArrayList();
        string patternForChars = @"[a-zA-Z]+$";
        if (!exNuc.IsNull)
        {
            AddNewInDelRowToResult(exNuc, resultList, patternForChars, true);
        }
        if (!missNuc.IsNull)
        {
            AddNewInDelRowToResult(missNuc, resultList, patternForChars, false);
        }
        return resultList;
    }

    private static void AddNewInDelRowToResult(SqlString nuc, ArrayList resultList, string patternForChars, bool inDelSwitch)
    {
        string nucValues = nuc.Value;
        string[] nucArray = nucValues.Split('\t');
        // the following variable index runs till length minus one because the last element of the (extra or missing) nucArray is always empty string:
        // e.g.: '0AT\t1TCG\t' --> {'0AT','1TCG',''}
        for (int index = 0; index < nucArray.Length - 1; index++)
        {
            string nucValue = Regex.Match(nucArray[index], patternForChars).Value;
            resultList.Add(new InDelRow { InDel = inDelSwitch, ChainLen = nucValue.Length, NucChain = nucValue });
        }
    }
}
