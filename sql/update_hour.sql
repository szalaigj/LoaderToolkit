TRUNCATE TABLE tweet_hour;


WITH q AS
(
	SELECT run_id, created_at, tweet_id,
		ROW_NUMBER() OVER (PARTITION BY CAST(created_at AS date), DATEPART(hour, created_at) ORDER BY created_at) rn
	FROM tweet
	--WHERE run_id = 4
)
INSERT tweet_hour
	(run_id, time, tweet_id)
SELECT
	run_id, DATEADD(hour, DATEPART(hour, created_at), CAST(CAST(created_at as date) as datetime)), tweet_id
FROM q
WHERE rn = 1



SELECT * FROM tweet_hour