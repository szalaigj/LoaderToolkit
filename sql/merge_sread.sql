dbcc traceon (610);

WITH
q
AS
(SELECT [samID]
      ,r.[refID]
	  ,[qname]
	  ,[dir]
	  ,[mapq]
	  ,[insPos]
      ,[delPos]
      ,[posStart]
      ,[posEnd]
	  ,[$targetdb].[dbo].[Concatenate]([refNuc]) as [crn]
      ,[seq]
	  ,[qual]
  FROM [$loaddb].[dbo].[$tablename] ld,
  [$targetdb].[dbo].[ref] r,
  [$targetdb].[dbo].[refDesc] d
  WHERE d.extID = ld.rname
  AND r.refID = d.refID
  AND r.pos BETWEEN ld.posStart AND ld.posEnd
  GROUP BY [samID],r.[refID],[qname],[dir],[mapq],[insPos],[delPos],[posStart],[posEnd],[seq],[qual])
INSERT INTO [$targetdb].[dbo].[sread] WITH(TABLOCKX) ([samID],[refID],[qname],[dir],[mapq],[posStart],[posEnd],[misMNuc],[indel],[qual])
SELECT [samID]
     ,[refID]
	 ,[qname]
	 ,[dir]
	 ,[mapq]
	 ,[posStart]
     ,[posEnd]
	 ,[mid].[misMNuc]
	 ,[mid].[indel]
	 ,[qual]
 FROM (SELECT * FROM q) innerTbl
 CROSS APPLY [$targetdb].[dbo].[MisIndel](crn, seq, insPos, delPos) as [mid];