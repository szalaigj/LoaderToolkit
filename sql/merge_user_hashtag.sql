-- Merge hashtags
dbcc traceon (610);

WITH s AS
(
	SELECT DISTINCT
		run_id, tag, user_id
	FROM [$loaddb].[dbo].[$tablename]
)
MERGE [$targetdb].[dbo].[user_hashtag] WITH (TABLOCKX/*, FORCESEEK*/) AS t
USING s
	ON s.run_id = t.run_id AND s.tag = t.tag AND s.user_id = t.user_id
WHEN NOT MATCHED THEN
	INSERT (run_id, tag, user_id)
	VALUES (s.run_id, s.tag, s.user_id);