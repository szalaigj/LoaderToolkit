dbcc traceon (610);

WITH 
q0 AS
(SELECT rd.refID
        ,l.* 
  FROM [genetics].[dbo].[tmp_1.aln_sreadBinLoad] l
  INNER JOIN [genetics].[dbo].[refDesc] rd
  ON rd.extID = l.rname),
q1 AS
(SELECT r.seqBlock
        ,r.posStart as refPosStart
		,q0.*
  FROM q0
  INNER JOIN [genetics].[dbo].[refBin] r
  ON r.refID = q0.refID AND FLOOR((q0.posStart - 1) / 256) * 256 + 1 = r.posStart)
INSERT INTO [$targetdb].[dbo].[sreadBin] WITH(TABLOCKX) ([samID],[refID],[qname],[dir],[mapq],[posStart],[posEnd],[misMNuc],[indel],[qual])
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
  FROM q1
  CROSS APPLY [genetics].[dbo].[MisIndelBin](seqBlock, refPosStart, seq, posStart, insPos, delPos) as [mid];