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
            cmd.CommandText = "SELECT refNuc FROM [dbo].[pileups]"
                + "WHERE [run_id] = " + runID.ToString()
                + "AND [sample_unit_id] = " + sampleUnitID.ToString()
                + "AND [reference_sequence_id] = " + refSeqID.ToString()
                + "AND [refSeqPos] in (" + posList + ")";

            string result = "";
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull(0))
                    {
                        result += "x";
                    }
                    else
                    {
                        string actualRefNuc = reader.GetString(0);
                        result += actualRefNuc;
                    }
                }
            }

            return new SqlString(result);
        }
    }
}
