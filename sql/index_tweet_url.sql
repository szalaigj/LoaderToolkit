CREATE CLUSTERED INDEX [IX_$tablename] ON [$dbname].[dbo].[$tablename]
(
	[run_id] ASC,
	[url_id] ASC,
	[tweet_id] ASC
) ON [PRIMARY];