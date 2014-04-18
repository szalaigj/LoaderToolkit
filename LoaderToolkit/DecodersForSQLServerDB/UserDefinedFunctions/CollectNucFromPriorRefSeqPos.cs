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
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlString CollectNucFromPriorRefSeqPos(SqlInt16 runID, SqlInt64 sampleUnitID, SqlInt64 refSeqID, 
        SqlInt64 refSeqPos, SqlInt32 posDiff)
    {
        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            long actualRefPos = refSeqPos.Value - posDiff.Value;
            cmd.CommandText = "SELECT refNuc FROM [dbo].[pileups]"
                + "WHERE [run_id] = " + runID.ToString()
                + "AND [sample_unit_id] = " + sampleUnitID.ToString()
                + "AND [reference_sequence_id] = " + refSeqID.ToString()
                + "AND [refSeqPos] = " + actualRefPos.ToString();
            string result = (string)cmd.ExecuteScalar();
            return new SqlString(result);
        }
    }
}
