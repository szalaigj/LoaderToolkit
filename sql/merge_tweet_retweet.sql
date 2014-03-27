/*
-- Merge retweets
dbcc traceon (610);

-- Create new table for merged data
CREATE TABLE [$twitterdb].[dbo].[tweet_retweet_new](
	[run_id] [bigint] NOT NULL,
	[tweet_id] [bigint] NOT NULL,
	[user_id] [bigint] NOT NULL,
	[retweeted_tweet_id] [bigint] NOT NULL,
	[retweeted_user_id] [bigint] NOT NULL,
	[created_at] [datetime] NOT NULL,
	[retweeted_at] [datetime] NOT NULL,
 CONSTRAINT [PK_tweet_retweet_new] PRIMARY KEY CLUSTERED 
(
	[run_id] ASC,
	[user_id] ASC,
	[retweeted_user_id] ASC,
	[tweet_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

-- Merge data into new table
INSERT [$twitterdb].[dbo].[tweet_retweet_new] WITH (TABLOCKX)
SELECT
	run_id, tweet_id, user_id, retweeted_tweet_id, retweeted_user_id, created_at, retweeted_at
FROM [$twitterdb].[dbo].[tweet_retweet]
UNION
SELECT DISTINCT
	run_id, tweet_id, user_id, retweeted_tweet_id, retweeted_user_id, created_at, retweeted_at
FROM [$loaddb].[dbo].[$tablename]

-- Drop old table and rename new

USE [$twitterdb]

DROP TABLE [$twitterdb].[dbo].[tweet_retweet]

EXEC sp_rename 'tweet_retweet_new', 'tweet_retweet';
EXEC sp_rename 'PK_tweet_retweet_new', 'PK_tweet_retweet';
*/



-- Merge retweets
dbcc traceon (610);

WITH s AS
(
	SELECT DISTINCT
		run_id, tweet_id, user_id, retweeted_tweet_id, retweeted_user_id, created_at, retweeted_at
	FROM [$loaddb].[dbo].[$tablename]
)
MERGE [$twitterdb].[dbo].[tweet_retweet] WITH (TABLOCKX) AS t
USING s
	ON s.run_id = t.run_id AND s.tweet_id = t.tweet_id
WHEN NOT MATCHED THEN
	INSERT
	VALUES (s.run_id, s.tweet_id, s.user_id, s.retweeted_tweet_id, s.retweeted_user_id, created_at, retweeted_at);