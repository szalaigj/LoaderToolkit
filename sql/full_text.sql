exec sp_fulltext_service 'load_os_resources', 1
exec sp_fulltext_service 'verify_signature', 0     -- to load unsigned dll's

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tweet_unique_id] ON [dbo].[tweet]
(
	[unique_id] ASC
)
WITH (
	DATA_COMPRESSION = PAGE,
	PAD_INDEX = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	SORT_IN_TEMPDB = OFF,
	IGNORE_DUP_KEY = OFF,
	DROP_EXISTING = OFF,
	ONLINE = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON)
ON [TWEETIDX]
GO

-- 15:49


CREATE FULLTEXT CATALOG [tweet_ft] WITH ACCENT_SENSITIVITY = ON
AS DEFAULT
AUTHORIZATION [dbo]

--
