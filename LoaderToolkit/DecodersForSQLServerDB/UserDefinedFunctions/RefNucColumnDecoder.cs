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
using DecodersForSQLServerDB;

public partial class UserDefinedFunctions
{
    private static readonly int encoderBitStringLength = 3;

    private static readonly Dictionary<BitArray, string> mapBitsToStrings;

    static UserDefinedFunctions()
    {
        mapBitsToStrings = new Dictionary<BitArray, string>(new BitArrayEqualityComparer());
        mapBitsToStrings.Add(EncodeZeroOneStringToBitArrays("001"), "A");
        mapBitsToStrings.Add(EncodeZeroOneStringToBitArrays("010"), "C");
        mapBitsToStrings.Add(EncodeZeroOneStringToBitArrays("011"), "G");
        mapBitsToStrings.Add(EncodeZeroOneStringToBitArrays("100"), "T");
    }

    private static BitArray EncodeZeroOneStringToBitArrays(string input)
    {
        char[] inputChars = input.ToCharArray();
        // The following is important because the BitArray will reverse the natural representation of bits.
        // E.g.: new BitArray(new bool[] {false, false, false, true}) -> 1000 when the content of bit array is copied to byte arrays.
        Array.Reverse(inputChars);
        bool[] bits = new bool[inputChars.Length];
        int index = 0;
        foreach (char ch in inputChars)
        {
            switch (ch)
            {
                case '0':
                    bits[index] = false;
                    index++;
                    break;
                case '1':
                    bits[index] = true;
                    index++;
                    break;
                default:
                    throw new ArgumentException("The input should contain only zero or one!");
            }
        }
        return new BitArray(bits);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString RefNucColumnDecoder(SqlBinary originalEncodedValue)
    {
        byte[] bytesOfEncodedValue = originalEncodedValue.Value;
        BitArray bitArray = new BitArray(bytesOfEncodedValue);
        var result = DecodeInputBits(bitArray);
        return new SqlString(result);
    }

    private static string DecodeInputBits(BitArray bitArray)
    {
        string result = "";
        var bitArrayCount = bitArray.Count;
        var bools = new bool[bitArrayCount];
        bitArray.CopyTo(bools, 0);
        bool[] boolsOfEncodedSign = new bool[encoderBitStringLength];
        int indexOfBoolsOfEncodedSign = 0;
        int falseCounter = 0;
        for (int index = 0; index < bitArrayCount; index++)
        {
            bool actualValue = bools[index];
            boolsOfEncodedSign[indexOfBoolsOfEncodedSign] = actualValue;
            indexOfBoolsOfEncodedSign++;
            if (!actualValue)
            {
                falseCounter++;
            }
            else
            {
                falseCounter = 0;
            }
            if ((indexOfBoolsOfEncodedSign % encoderBitStringLength) == 0)
            {
                if (falseCounter == encoderBitStringLength)
                {
                    // If there are as many 'false's of BitArray as encoderBitStringLength then these are only the trailing zeros.
                    break;
                }
                result = DecodeUnitOfInputBits(result, ref boolsOfEncodedSign, ref indexOfBoolsOfEncodedSign, ref falseCounter);
            }
        }
        return result;
    }

    private static string DecodeUnitOfInputBits(string result, ref bool[] boolsOfEncodedSign, ref int indexOfBoolsOfEncodedSign,
        ref int falseCounter)
    {
        var bitArrayOfEncodedSign = new BitArray(boolsOfEncodedSign);
        string decodedSign;
        if (mapBitsToStrings.TryGetValue(bitArrayOfEncodedSign, out decodedSign))
        {
            result += decodedSign;
        }
        else
        {
            throw new ArgumentException("The encodingDomain does not contain the unit of input bools!");
        }
        indexOfBoolsOfEncodedSign = 0;
        boolsOfEncodedSign = new bool[encoderBitStringLength];
        falseCounter = 0;
        return result;
    }
}
