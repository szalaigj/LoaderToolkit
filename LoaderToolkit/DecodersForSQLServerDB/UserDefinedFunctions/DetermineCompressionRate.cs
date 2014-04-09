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
    public static SqlDouble DetermineCompressionRate(SqlBinary originalEncodedValue, SqlString decodedValue)
    {
        int decodedValueLength = decodedValue.ToString().Length;
        int encodedValueLength = originalEncodedValue.Value.Length;
        double rate = (double)encodedValueLength / (double)decodedValueLength;
        return new SqlDouble(rate);
    }
}
