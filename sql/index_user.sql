CREATE CLUSTERED INDEX [IX_$tablename] ON [$dbname].[dbo].[$tablename]
(
	--[run_id] ASC,
	[user_id] ASC,
	[tweeted_at] DESC,
	[screen_name] ASC,
	[description] ASC,
	[geo_enabled] ASC,
	[lang] ASC,
	[location] ASC,
	[name] ASC,
	[profile_background_color] ASC,
	[profile_text_color] ASC,
	[utc_offset] ASC,
	[verified] ASC
) ON [PRIMARY];