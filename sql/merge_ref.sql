dbcc traceon (610);

INSERT INTO [$targetdb].[dbo].[ref] WITH(TABLOCKX) ([refID],[pos],[refNuc])
SELECT [refID],[pos],[refNuc] FROM [$loaddb].[dbo].[$tablename];