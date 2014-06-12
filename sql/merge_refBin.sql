dbcc traceon (610);

INSERT INTO [$targetdb].[dbo].[refBin] WITH(TABLOCKX) ([refID],[posStart],[seqBlock])
SELECT [refID],[posStart],[seqBlock] FROM [$loaddb].[dbo].[$tablename];