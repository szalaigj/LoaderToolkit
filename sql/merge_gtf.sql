dbcc traceon (610);

INSERT INTO [$targetdb].[dbo].[gtf] WITH(TABLOCKX) ([seqname],[source],[feature],[start],[end],[score],[strand],[frame],[attribute])
SELECT [seqname],[source],[feature],[start],[end],[score],[strand],[frame],[attribute] FROM [$loaddb].[dbo].[$tablename];