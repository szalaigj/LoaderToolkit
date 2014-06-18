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

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlInt64 DetRelRefPosStart(SqlInt64 pos)
    {
        long posValue = pos.Value;
        long relatedRefPosStartValue = (long)(Math.Floor((posValue - 1) / 256.0) * 256 + 1);
        return new SqlInt64(relatedRefPosStartValue);
    }
}
