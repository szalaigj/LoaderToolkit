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
using BinaryCodec;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString DetDecRefSeq(SqlInt32 posRadius, SqlInt64 pos, SqlInt64 refPosStart, SqlBinary refSeq)
    {
        if (posRadius.Value > 128)
        {
            throw new InvalidExpressionException("The posRadius is too long. It should be less than 129.");
        }
        else if (posRadius.Value < 0)
        {
            throw new InvalidExpressionException("The posRadius should be non-negative.");
        }
        long relatedPosStartValue = pos.Value - posRadius.Value;
        long relatedPosEndValue = pos.Value + posRadius.Value - 1;
        int relatedByteSeqLength = posRadius.Value;
        // If the relatedPosStartValue is not aligned to the beginning of the given byte exactly
        // then the size of byte array should be increased:
        relatedByteSeqLength = (Math.Abs(relatedPosStartValue - refPosStart.Value) % 2 == 1) 
            ? (relatedByteSeqLength + 1) : relatedByteSeqLength;
        string decodedRelatedRefSeqBlock = BinaryNucleotideCodecUtil.DetermineDecodedRelatedRefSeqBlock(relatedByteSeqLength,
            relatedPosStartValue, refPosStart.Value, refSeq.Value);
        decodedRelatedRefSeqBlock = BinaryNucleotideCodecUtil.ComplementSucceedingNuc(relatedPosEndValue, refPosStart.Value, 
            refSeq.Value, decodedRelatedRefSeqBlock);
        return new SqlString(decodedRelatedRefSeqBlock);
    }
}
