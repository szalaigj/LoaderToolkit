CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[run_id] [smallint] NOT NULL,
	[user_id] [bigint] NOT NULL,
	[created_at] [datetime] NOT NULL,
	[tweeted_at] [datetime] NOT NULL,
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
	[verified] [bit] NOT NULL
) ON [PRIMARY];
