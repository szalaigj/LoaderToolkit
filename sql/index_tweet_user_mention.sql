
CREATE CLUSTERED INDEX [$ixname] ON [$dbname].[dbo].[$tablename]
(
	[run_id] ASC,
	[user_id] ASC,
	[mentioned_user_id] ASC,
	[tweet_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
