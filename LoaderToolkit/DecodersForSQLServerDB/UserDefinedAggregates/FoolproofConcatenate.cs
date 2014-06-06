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
using System.Collections.Generic;
using System.Text;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    Format.UserDefined, //representing the serialization format of this aggregate
    //this format gives the developer full control over the binary format
    //through the IBinarySerialize.Write and IBinarySerialize.Read methods.
    IsInvariantToNulls = true, //indicates whether the aggregate is invariant to nulls
    IsInvariantToDuplicates = false, //indicates whether the aggregate is invariant to duplicates
    MaxByteSize = 8000 //the maximum size, in bytes, of the aggregate instance
    )]
public struct FoolproofConcatenate : IBinarySerialize
{
    private Dictionary<long, char> intermediateResult;

    public Dictionary<long, char> IntermediateResult
    {
        get { return intermediateResult; }
    }

    public void Init()
    {
        this.intermediateResult = new Dictionary<long, char>();
    }

    public void Accumulate(SqlString currentRefNuc, SqlInt64 currentPos)
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
        intermediateResult[currentPos.Value] = refNucChar;
    }

    public void Merge(FoolproofConcatenate other)
    {
        if (this.intermediateResult.Count + other.IntermediateResult.Count > 4000)
        {
            throw new InvalidDataException("The merged concatenated part is too long.");
        }
        foreach (var entry in other.IntermediateResult)
        {
            this.intermediateResult[entry.Key] = entry.Value;
        }
    }

    public SqlString Terminate()
    {
        string output = DetermineOutput();
        return new SqlString(output);
    }

    public void Read(BinaryReader r)
    {
        intermediateResult = ParseSerializedObject(r.ReadString());
    }

    public void Write(BinaryWriter w)
    {
        string output = SerializeDictionary();
        w.Write(output);
    }

    private string SerializeDictionary()
    {
        string output = "";
        List<long> listOfKeys = new List<long>(intermediateResult.Keys);
        listOfKeys.Sort();
        foreach (var key in listOfKeys)
        {
            output += key + " " + intermediateResult[key].ToString() + "\t";
        }
        // Eliminate the last TAB character:
        output = output.Substring(0, output.Length - 1);
        return output;
    }

    private Dictionary<long, char> ParseSerializedObject(string input)
    {
        Dictionary<long, char> result = new Dictionary<long,char>();
        string[] inputEntries = input.Split('\t');
        foreach (var entry in inputEntries)
        {
            string[] entryValues = entry.Split(' ');
            result[Int64.Parse(entryValues[0])] = entryValues[1].ToCharArray()[0];
        }
        return result;
    }

    private string DetermineOutput()
    {
        string output = "";
        List<long> listOfKeys = new List<long>(intermediateResult.Keys);
        listOfKeys.Sort();
        foreach (var key in listOfKeys)
        {
            output += intermediateResult[key].ToString();
        }
        return output;
    }
}
