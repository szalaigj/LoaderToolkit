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

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString PickOutAPartOfColumnBySeparator(SqlString inputColumn, SqlInt32 nThPart, SqlString separator)
    {
        string inputColumnStr = inputColumn.Value;
        string separatorStr = separator.Value;
        string[] separatedInputColumn = Regex.Split(inputColumnStr, separatorStr);
        string result = separatedInputColumn[nThPart.Value];
        return new SqlString(result);
    }
}
