/*
-- Copy users to the final DB

-- Merge user status updates

-- This scripts records user personal details updates.
-- Creates at least one entry per chunk so it will somewhat
-- reflect how the counters of a user grow with time, even
-- if no profile settings have been changed.

dbcc traceon (610);

-- Create new table for merged user data

CREATE TABLE [$targetdb].[dbo].[user_new](
	[user_id] [bigint] NOT NULL,
	[created_at] [datetime] NOT NULL,
	[last_update_at] [datetime] NOT NULL,
	[screen_name] [nvarchar](50) NOT NULL,
	[description] [nvarchar](160) NOT NULL,
	[favourites_count] [int] NOT NULL,
	[followers_count] [int] NOT NULL,
	[friends_count] [int] NOT NULL,
	[statuses_count] [int] NOT NULL,
	[geo_enabled] [bit] NOT NULL,
	[lang] [char](5) NOT NULL,
	[location] [nvarchar](100) NULL,
	[name] [nvarchar](30) NOT NULL,
	[profile_background_color] [char](6) NOT NULL,
	[profile_text_color] [char](6) NOT NULL,
	[protected] [bit] NOT NULL,
	[show_all_inline_media] [bit] NOT NULL,
	[utc_offset] [int] NULL,
	[verified] [bit] NOT NULL,
 CONSTRAINT [PK_user_new] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [USER]
) ON [USER];

-- Merge users into new table
WITH q AS
(
	SELECT	user_id, created_at, tweeted_at, screen_name, description, favourites_count, followers_count,
			friends_count, statuses_count, geo_enabled, lang, location, name,
			profile_background_color, profile_text_color, protected,
			show_all_inline_media, utc_offset, verified,
			ROW_NUMBER() OVER (PARTITION BY user_id ORDER BY user_id ASC, tweeted_at DESC) AS rn
	FROM  [$loaddb].[dbo].[$tablename]
)
	INSERT [$targetdb].[dbo].[user_new] WITH (TABLOCKX)
	SELECT 
		user_id, created_at, last_update_at, screen_name, description, favourites_count, followers_count,
		friends_count, statuses_count, geo_enabled, lang, location, name,
		profile_background_color, profile_text_color, protected,
		show_all_inline_media, utc_offset, verified
	FROM [$targetdb].[dbo].[user] old
	WHERE user_id NOT IN (SELECT user_id FROM  [$loaddb].[dbo].[$tablename])

	UNION ALL

	SELECT user_id, created_at, tweeted_at, screen_name, description, favourites_count, followers_count,
				friends_count, statuses_count, geo_enabled, lang, location, name,
				profile_background_color, profile_text_color, protected,
				show_all_inline_media, utc_offset, verified
	FROM q
	WHERE rn = 1;

-- Drop old table and rename new
DROP TABLE [$targetdb].[dbo].[user];

USE [$targetdb]

EXEC sp_rename 'user_new' , 'user'
EXEC sp_rename 'PK_user_new', 'PK_user'
*/

-- Copy users to the final DB

-- Always reflect the last status of user profile

dbcc traceon (610);

WITH q AS
(
	SELECT	user_id, created_at, tweeted_at, screen_name, description, favourites_count, followers_count,
			friends_count, statuses_count, geo_enabled, lang, location, name,
			profile_background_color, profile_text_color, protected,
			show_all_inline_media, utc_offset, verified,
			ROW_NUMBER() OVER (PARTITION BY run_id, user_id ORDER BY user_id ASC, tweeted_at DESC) AS rn
	FROM [$loaddb].[dbo].[$tablename]
)
MERGE [$targetdb].[dbo].[user] WITH (TABLOCKX) AS t
USING q AS s
	ON s.user_id = t.user_id AND rn = 1
/*WHEN MATCHED AND s.tweeted_at > t.last_update_at THEN
	UPDATE
	SET	t.last_update_at = s.tweeted_at,
		t.screen_name = s.screen_name,
		t.description = s.description,
		t.favourites_count = s.favourites_count,
		t.followers_count = s.followers_count,
		t.friends_count = s.friends_count,
		t.statuses_count = s.statuses_count,
		t.geo_enabled = s.geo_enabled,
		t.lang = s.lang,
		t.location = s.location,
		t.name = s.name,
		t.profile_background_color = s.profile_background_color,
		t.profile_text_color = s.profile_text_color,
		t.protected = s.protected,
		t.show_all_inline_media = s.show_all_inline_media,
		t.utc_offset = s.utc_offset,
		t.verified = s.verified*/
WHEN NOT MATCHED AND rn = 1 THEN
	INSERT
	VALUES (user_id, created_at, tweeted_at, screen_name, description, favourites_count, followers_count,
			friends_count, statuses_count, geo_enabled, lang, location, name,
			profile_background_color, profile_text_color, protected,
			show_all_inline_media, utc_offset, verified);
