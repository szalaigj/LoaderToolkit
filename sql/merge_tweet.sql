-- Merge tweets
dbcc traceon (610);

WITH q AS
(
	SELECT	run_id, tweet_id, created_at, utc_offset, user_id, place_id, lon, lat, 
			cx, cy, cz, htm_id,
			in_reply_to_tweet_id, in_reply_to_user_id,
			possibly_sensitive, possibly_sensitive_editable,
			retweet_count, text, truncated, lang, lang_word_count, lang_guess1, lang_guess2,
			ROW_NUMBER() OVER (PARTITION BY run_id, tweet_id ORDER BY created_at DESC) AS rn
	FROM [$loaddb].[dbo].[$tablename]
)
MERGE [$targetdb].[dbo].[tweet] WITH (TABLOCKX) AS t
USING (SELECT * FROM q WHERE rn = 1) AS s
	ON s.run_id = t.run_id AND s.tweet_id = t.tweet_id
WHEN MATCHED AND s.retweet_count > t.retweet_count THEN
	UPDATE
	SET	t.retweet_count = s.retweet_count
WHEN NOT MATCHED THEN
	INSERT (run_id, tweet_id, created_at, utc_offset, user_id, place_id, lon, lat, 
			cx, cy, cz, htm_id,
			in_reply_to_tweet_id, in_reply_to_user_id,
			possibly_sensitive, possibly_sensitive_editable,
			retweet_count, text, truncated, lang, lang_word_count, lang_guess1, lang_guess2)
	VALUES (run_id, tweet_id, created_at, utc_offset, user_id, place_id, lon, lat, 
			cx, cy, cz, htm_id,
			in_reply_to_tweet_id, in_reply_to_user_id,
			possibly_sensitive, possibly_sensitive_editable,
			retweet_count, text, truncated, lang, lang_word_count, lang_guess1, lang_guess2);
