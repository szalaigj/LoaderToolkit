CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[run_id] [smallint] NOT NULL,
	[tweet_id] [bigint] NOT NULL,
	[user_id] [bigint] NOT NULL,
	[retweeted_tweet_id] [bigint] NOT NULL,
	[retweeted_user_id] [bigint] NOT NULL,
	[created_at] [datetime] NOT NULL,
	[retweeted_at] [datetime] NOT NULL
) ON [PRIMARY]
