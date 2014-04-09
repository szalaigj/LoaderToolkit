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
using BinaryCodec;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString RefNucColumnDecoder(SqlBinary originalEncodedValue)
    {
        byte[] bytesOfEncodedValue = originalEncodedValue.Value;
        var result = BinaryCodecUtil.DecodeInputBytes(bytesOfEncodedValue, Constants.CodecDomainNames.RefNuc);
        return new SqlString(result);
    }
}
