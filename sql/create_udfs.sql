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

CREATE FUNCTION [dbo].[CollectNucsFromNeighborhoodOfRefSeqPos](@pupID [int], @pos [bigint], @posRadius [int])
RETURNS [nvarchar](100) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [DecodersForSQLServerDB].[UserDefinedFunctions].[CollectNucsFromNeighborhoodOfRefSeqPos]
GO

CREATE AGGREGATE [dbo].[Concatenate](@startPos [bigint], @currentRefNuc [nvarchar](1), @currentPos [bigint])
RETURNS [nvarchar](4000)
EXTERNAL NAME [DecodersForSQLServerDB].[SafeConcatenate]
GO

CREATE FUNCTION [dbo].[MisIndel](@refSeq [nvarchar](4000), @sreadSeq [nvarchar](4000), @insPos [nvarchar](4000), @delPos [nvarchar](4000))
RETURNS table 
(
    misMNuc nvarchar(4000), indel nvarchar(4000)
) 
AS
EXTERNAL NAME [DecodersForSQLServerDB].[UserDefinedFunctions].[ObtainMismatchAndInDel];
GO

CREATE FUNCTION [dbo].[IsDel](@posStart [bigint], @indel [nvarchar](4000), @refPos [bigint])
RETURNS [bit] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [DecodersForSQLServerDB].[UserDefinedFunctions].[IsDel]
GO

CREATE FUNCTION [dbo].[IsNucX](@posStart [bigint], @misMNuc [nvarchar](4000), @refPos [bigint], @refNuc [nvarchar](1), @countNuc [nvarchar](1))
RETURNS [int] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [DecodersForSQLServerDB].[UserDefinedFunctions].[IsNucX]
GO

CREATE FUNCTION [dbo].[DetermineInDel](@posStart [bigint], @indel [nvarchar](4000))
RETURNS table 
(
    inDelStartPos bigint, inDel bit, chainLen int, nucChain nvarchar(100)
)
AS 
EXTERNAL NAME [DecodersForSQLServerDB].[UserDefinedFunctions].[DetermineInDel]
GO