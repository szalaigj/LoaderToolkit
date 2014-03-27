CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[run_id] [smallint] NOT NULL,
	[tweet_id] [bigint] NOT NULL,
	[user_id] [bigint] NOT NULL,
	[url_id] [char](8) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[expanded_url] [varchar](8000)
) ON [PRIMARY]