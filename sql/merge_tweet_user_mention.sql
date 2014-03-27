-- Merge tweet user mention
dbcc traceon (610);

WITH s AS
(
	SELECT DISTINCT
		run_id, tweet_id, user_id, mentioned_user_id
	FROM [$loaddb].[dbo].[$tablename]
)
MERGE [$twitterdb].[dbo].[tweet_user_mention] WITH (TABLOCKX) AS t
USING s
	ON  s.run_id = t.run_id AND s.tweet_id = t.tweet_id AND s.user_id = t.user_id AND s.mentioned_user_id = t.mentioned_user_id
WHEN NOT MATCHED THEN
	INSERT
	VALUES (s.run_id, s.tweet_id, s.user_id, s.mentioned_user_id);