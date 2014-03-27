-- Merge tweet_url
dbcc traceon (610);

WITH s AS
(
	SELECT
		run_id, url_id, tweet_id, user_id, created_at, expanded_url,
		ROW_NUMBER() OVER (PARTITION BY run_id, url_id, tweet_id ORDER BY user_id) rn
	FROM [$loaddb].[dbo].[$tablename]
)
MERGE [$twitterdb].[dbo].[tweet_url] WITH (TABLOCKX) AS t
USING (SELECT * FROM s WHERE rn = 1) s
	ON s.run_id = t.run_id AND s.url_id = t.url_id AND s.tweet_id = t.tweet_id
WHEN NOT MATCHED THEN
	INSERT (run_id, url_id, tweet_id, user_id, created_at, expanded_url)
	VALUES (s.run_id, s.url_id, s.tweet_id, s.user_id, created_at, expanded_url);