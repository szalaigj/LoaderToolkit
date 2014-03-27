-- Merge hashtags
dbcc traceon (610);

WITH s AS
(
	SELECT DISTINCT
		run_id, tweet_id, tag, user_id, created_at
	FROM [$loaddb].[dbo].[$tablename]
)
MERGE [$twitterdb].[dbo].[tweet_hashtag] WITH (TABLOCKX) AS t
USING s
	ON s.run_id = t.run_id AND s.tag = t.tag AND s.tweet_id = t.tweet_id AND s.user_id = t.user_id
WHEN NOT MATCHED THEN
	INSERT (run_id, tag, tweet_id, user_id, created_at)
	VALUES (s.run_id, s.tag, s.tweet_id, s.user_id, created_at);