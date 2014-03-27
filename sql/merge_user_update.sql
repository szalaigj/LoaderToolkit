-- Merge user status updates

-- This scripts records user personal details updates.
-- Creates at least one entry per chunk so it will somewhat
-- reflect how the counters of a user grow with time, even
-- if no profile settings have been changed.

dbcc traceon (610);

WITH s AS
(
	SELECT
		user_id, MAX(tweeted_at) tweeted_at, screen_name, description,
		MAX(favourites_count) favourites_count, MAX(followers_count) followers_count,
		MAX(friends_count) friends_count, MAX(statuses_count) statuses_count,
		geo_enabled, lang,
		location, name, profile_background_color, profile_text_color, utc_offset, verified
	FROM [$loaddb].[dbo].[$tablename]
	GROUP BY user_id, screen_name, description, geo_enabled, lang,
		location, name, profile_background_color, profile_text_color, utc_offset, verified
)
INSERT [$twitterdb].[dbo].[user_update] WITH (TABLOCKX)
SELECT $run_id, user_id, tweeted_at, screen_name, description,
			favourites_count, followers_count, friends_count, statuses_count,
			geo_enabled, lang, location, name,
			profile_background_color, profile_text_color,
			utc_offset, verified
FROM s



