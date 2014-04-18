USE [szalaigj]
GO

SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE FUNCTION [dbo].[BasesColumnDecoder](@originalEncodedValue [varbinary](8000))
RETURNS [nvarchar](4000) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [DecodersForSQLServerDB].[UserDefinedFunctions].[BasesColumnDecoder]
GO

CREATE FUNCTION [dbo].[PickOutAPartOfColumnBySeparator](@inputColumn [nvarchar](4000), @nThPart [int], @separator [nvarchar](20))
RETURNS [nvarchar](4000) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [DecodersForSQLServerDB].[UserDefinedFunctions].[PickOutAPartOfColumnBySeparator]
GO

CREATE FUNCTION [dbo].[CountBasesSeparately](@bases [nvarchar](4000), @refNuc [nvarchar](1))
RETURNS table 
(
    A int, C int, G int, T int
) 
AS
EXTERNAL NAME [DecodersForSQLServerDB].[UserDefinedFunctions].[CountBasesSeparately];
GO

CREATE FUNCTION [dbo].[CollectNucsFromNeighborhoodOfRefSeqPos](@runID [smallint], @sampleUnitID [bigint],
@refSeqID [bigint], @refSeqPos [bigint], @posRadius [int])
RETURNS [nvarchar](100) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [DecodersForSQLServerDB].[UserDefinedFunctions].[CollectNucsFromNeighborhoodOfRefSeqPos]
GO