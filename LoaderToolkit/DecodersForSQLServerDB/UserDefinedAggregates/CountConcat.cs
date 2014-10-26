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
    MaxByteSize = -1 //the maximum size, in bytes, of the aggregate instance
    )]
public struct CountConcat : IBinarySerialize
{
    private Dictionary<long, char> intermediateResult;

    private Int32 substrLenValue;

    public Dictionary<long, char> IntermediateResult
    {
        get { return intermediateResult; }
    }

    public Int32 SubstrLenValue
    {
        get { return substrLenValue; }
    }

    public void Init()
    {
        this.intermediateResult = new Dictionary<long, char>();
    }

    public void Accumulate(SqlString currentRefNuc, SqlInt64 currentPos, SqlInt32 substrLen)
    {
        substrLenValue = substrLen.Value;
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

    public void Merge(CountConcat other)
    {
        this.substrLenValue = other.SubstrLenValue;
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
        Int32 substrLenValueOut;
        intermediateResult = ParseSerializedObject(r.ReadString(), out substrLenValueOut);
        substrLenValue = substrLenValueOut;
    }

    public void Write(BinaryWriter w)
    {
        string output = SerializeDictionary();
        w.Write(output);
    }

    private string SerializeDictionary()
    {
        string output = "";
        output += substrLenValue + "\n";
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

    private Dictionary<long, char> ParseSerializedObject(string input, out Int32 substrLenValueOut)
    {
        string[] inputParts = input.Split('\n');
        substrLenValueOut = Int32.Parse(inputParts[0]);
        Dictionary<long, char> result = new Dictionary<long, char>();
        string[] inputEntries = inputParts[1].Split('\t');
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
        string subSeq = "";
        Dictionary<string, int> counterInDict = new Dictionary<string, int>();
        for (int index = 0; index < listOfKeys.Count - substrLenValue + 1; index++)
        {
            for (int subIndex = 0; subIndex < substrLenValue; subIndex++)
            {
                subSeq += intermediateResult[listOfKeys[index + subIndex]].ToString();
            }
            if (!counterInDict.ContainsKey(subSeq))
            {
                counterInDict[subSeq] = 1;
            }
            else
            {
                counterInDict[subSeq]++;
            }
            subSeq = "";
        }
        List<string> listOfCountInDictKeys = new List<string>(counterInDict.Keys);
        listOfCountInDictKeys.Sort();
        foreach (var key in listOfCountInDictKeys)
        {
            output += key + " " + counterInDict[key].ToString() + "\t";
        }
        // Eliminate the last TAB character:
        output = output.Substring(0, output.Length - 1);
        return output;
    }
}
