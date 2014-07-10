-- For SAM-style with binary encoded reference genomes:
USE szalaigj
GO

-- This sample query results nucleotide in position 93696 (of reference with refID 65539):
DECLARE @refVar int
SELECT @refVar = 65539
DECLARE @posVar bigint
SELECT @posVar = 93696
DECLARE @refPosStartVar bigint
SELECT @refPosStartVar = [dbo].[DetRelRefPosStart](@posVar)
DECLARE @radius int
SELECT @radius = 0

SELECT [refID]
      ,[posStart]
      ,[seqBlock]
	  ,[dbo].[DetDecRefSeq](@radius, @posVar, [posStart], [seqBlock]) as decSeq
  FROM [dbo].[refBin] rb
WHERE rb.[refID] = @refVar AND rb.[posStart] = @refPosStartVar

-- This sample query finds neighbourhood for position 93696 (of reference with refID 65539) when the radius is two:
DECLARE @refVar int
SELECT @refVar = 65539
DECLARE @posVar bigint
SELECT @posVar = 93696
DECLARE @refPosStartVar bigint
SELECT @refPosStartVar = [dbo].[DetRelRefPosStart](@posVar)
DECLARE @radius int
SELECT @radius = 2

SELECT [refID]
      ,[posStart]
      ,[seqBlock]
	  ,[dbo].[DetDecRefSeq](@radius, @posVar, [posStart], [seqBlock]) as decSeq
  FROM [dbo].[refBin] rb
WHERE rb.[refID] = @refVar AND rb.[posStart] = @refPosStartVar