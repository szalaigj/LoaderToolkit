CREATE CLUSTERED INDEX [IX_$tablename] ON [$dbname].[dbo].[$tablename] 
(
	[run_id] ASC,
	[tag] ASC,
	[tweet_id] ASC,
	[user_id] ASC
) ON [PRIMARY];


CREATE NONCLUSTERED INDEX [IX_$tablename_2] ON [$dbname].[dbo].[$tablename] 
(
	[run_id] ASC,
	[user_id] ASC,
	[tag] ASC
) 
INCLUDE
(
	[tweet_id],
	[created_at] )
ON [PRIMARY];