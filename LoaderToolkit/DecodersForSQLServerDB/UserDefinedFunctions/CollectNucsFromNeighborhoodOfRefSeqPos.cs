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
using System.Collections.Generic;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlString CollectNucsFromNeighborhoodOfRefSeqPos(SqlInt16 runID, SqlInt64 sampleUnitID, SqlInt64 refSeqID,
        SqlInt64 refSeqPos, SqlInt32 posRadius)
    {
        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            string beginOfSelection = "SELECT refNuc FROM [dbo].[pileups]"
                + "WHERE [run_id] = " + runID.ToString()
                + "AND [sample_unit_id] = " + sampleUnitID.ToString()
                + "AND [reference_sequence_id] = " + refSeqID.ToString();
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            long centerRefPos = refSeqPos.Value;
            int radius = posRadius.Value;
            string posList = "";
            for (long i = centerRefPos - radius; i <= centerRefPos + radius; i++)
            {
                posList += i.ToString() + ",";
            }
            // The last comma is ignored:
            posList = posList.Substring(0, posList.Length - 1);
            long actualRefPos = refSeqPos.Value - posRadius.Value;
            cmd.CommandText = beginOfSelection + "AND [refSeqPos] in (" + posList + ")";

            string result = "";
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string actualRefNuc = reader.GetString(0);
                    result += actualRefNuc;
                }
            }

            if (result.Length != 2 * posRadius + 1)
            {
                result = HandleMissingPos(conn, beginOfSelection, centerRefPos, radius, result);
            }

            return new SqlString(result);
        }
    }

    private static string HandleMissingPos(SqlConnection conn, string beginOfSelection, long centerRefPos, int radius, string result)
    {
        // TODO: More complex examination is needed indeed than the following if the posRadius greater than one.
        long firstPos = centerRefPos - radius;
        SqlCommand checkCmd = new SqlCommand();
        checkCmd.Connection = conn;
        checkCmd.CommandText = beginOfSelection + "AND [refSeqPos] = " + firstPos.ToString();
        object firstNuc = checkCmd.ExecuteScalar();
        if (firstNuc == null)
        {
            result = "x" + result;
        }
        else
        {
            result += "x";
        }
        return result;
    }
}
