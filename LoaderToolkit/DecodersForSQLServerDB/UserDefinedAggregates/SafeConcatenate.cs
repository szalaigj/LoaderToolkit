//------------------------------------------------------------------------------
// <copyright file="CSSqlAggregate.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    Format.UserDefined, //representing the serialization format of this aggregate
    //this format gives the developer full control over the binary format
    //through the IBinarySerialize.Write and IBinarySerialize.Read methods.
    IsInvariantToNulls = true, //indicates whether the aggregate is invariant to nulls
    IsInvariantToDuplicates = false, //indicates whether the aggregate is invariant to duplicates
    MaxByteSize = 8000 //the maximum size, in bytes, of the aggregate instance
    )]
public struct SafeConcatenate : IBinarySerialize
{

    private char[] intermediateResult;
    private int realSize;

    public int RealSize
    {
        get { return realSize; }
    }

    public char[] IntermediateResult
    {
        get { return intermediateResult; }
    }

    public void Init()
    {
        this.intermediateResult = new char[4000];
        this.realSize = 0;
        for (int index = 0; index < intermediateResult.Length; index++)
        {
            intermediateResult[index] = '\0';
        }
    }

    public void Accumulate(SqlInt64 startPos, SqlString currentRefNuc, SqlInt64 currentPos)
    {
        var refNucChars = currentRefNuc.Value.ToCharArray();
        char refNucChar;
        if (refNucChars.Length == 1)
        {
            refNucChar = refNucChars[0];
        }
        else
        {
            throw new InvalidDataException("The input reference nucleotide contains only one character.");
        }
        intermediateResult[currentPos.Value - startPos.Value] = refNucChar;
        realSize++;
    }

    public void Merge(SafeConcatenate other)
    {
        if (this.realSize + other.RealSize > intermediateResult.Length)
        {
            throw new InvalidDataException("The merged concatenated part is too long.");
        }
        for (int index = this.realSize; index < intermediateResult.Length; index++)
        {
            intermediateResult[index] = other.IntermediateResult[index - this.realSize];
        }
    }

    public SqlString Terminate()
    {
        string output = new string(intermediateResult);
        return new SqlString(output);
    }

    public void Read(BinaryReader r)
    {
        intermediateResult = r.ReadString().ToCharArray();
    }

    public void Write(BinaryWriter w)
    {
        string output = new string(intermediateResult);
        w.Write(output);
    }
}
