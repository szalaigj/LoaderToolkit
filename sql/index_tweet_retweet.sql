CREATE CLUSTERED INDEX [IX_$tablename] ON [$dbname].[dbo].[$tablename] 
(
	[run_id] ASC,
	[user_id] ASC,
	[retweeted_user_id] ASC,
	[tweet_id] ASC
) ON [PRIMARY];