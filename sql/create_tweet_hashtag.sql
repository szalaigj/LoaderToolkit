CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[run_id] [smallint] NOT NULL,
	[tweet_id] [bigint] NOT NULL,
	[user_id] [bigint] NOT NULL,
	[tag] [nvarchar](50) NOT NULL,
	[created_at] [datetime] NOT NULL
) ON [PRIMARY]
