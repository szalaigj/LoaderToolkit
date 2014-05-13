dbcc traceon (610);

INSERT INTO [$targetdb].[dbo].[ref] ([refID],[pos],[refNuc]) WITH(TABLOCK)
SELECT [refID],[pos],[refNuc] FROM [$loaddb].[dbo].[$tablename];