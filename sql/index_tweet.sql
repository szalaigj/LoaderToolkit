CREATE CLUSTERED INDEX [IX_$tablename] ON [$dbname].[dbo].[$tablename] 
(
	[run_id] ASC,
	[tweet_id] ASC,
	[created_at] DESC
) ON [PRIMARY];