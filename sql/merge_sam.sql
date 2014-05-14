dbcc traceon (610);

INSERT INTO [$targetdb].[dbo].[sam] WITH(TABLOCKX) ([samID],[line],[type],[tags])
SELECT [samID],[line],[type],[tags] FROM [$loaddb].[dbo].[$tablename];