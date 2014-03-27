CREATE TABLE [dbo].[run](
	[run_id] [smallint] NOT NULL,
	[started_at] [datetime] NOT NULL,
	[stopped_at] [datetime] NOT NULL,
 CONSTRAINT [PK_run] PRIMARY KEY CLUSTERED
 (
	[run_id] ASC
 ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

--

CREATE TABLE [dbo].[tweet](
	[run_id] [smallint] NOT NULL,
	[tweet_id] [bigint] NOT NULL,
	[created_at] [datetime] NOT NULL,
	[utc_offset] [int] NULL,
	[user_id] [bigint] NOT NULL,
	[place_id] [char](16) NULL,
	[lon] [float] NULL,
	[lat] [float] NULL,
	[cx] [float] NOT NULL,
	[cy] [float] NOT NULL,
	[cz] [float] NOT NULL,
	[htm_id] [bigint] NOT NULL,
	[in_reply_to_tweet_id] [bigint] NULL,
	[in_reply_to_user_id] [bigint] NULL,
	[possibly_sensitive] [bit] NULL,
	[possibly_sensitive_editable] [bit] NULL,
	[retweet_count] [int] NOT NULL,
	[text] [nvarchar](150) NOT NULL,
	[truncated] [bit] NOT NULL,
	[lang] [varchar](5) NOT NULL,
	[lang_word_count] [tinyint] NOT NULL,
	[lang_guess1] [char](2) NOT NULL,
	[lang_guess2] [char](2) NOT NULL,
	[unique_id] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_tweet] PRIMARY KEY CLUSTERED 
(
	[run_id] ASC,
	[tweet_id] ASC
)WITH (
	PAD_INDEX  = OFF,
	STATISTICS_NORECOMPUTE  = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS  = ON,
	ALLOW_PAGE_LOCKS  = ON,
	DATA_COMPRESSION = PAGE,
	FILLFACTOR = 98
	) ON [TWEET]
) ON [TWEET]



GO

----------

CREATE TABLE [dbo].[tweet_hour](
	[run_id] [smallint] NOT NULL,
	[time] [datetime] NOT NULL,
	[tweet_id] [bigint] NOT NULL,
 CONSTRAINT [PK_tweet_hour] PRIMARY KEY CLUSTERED 
(
	[run_id] ASC,
	[time] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


----------

CREATE TABLE [dbo].[tweet_hashtag](
	[run_id] [smallint] NOT NULL,
	[tag] [nvarchar](50) NOT NULL,
	[tweet_id] [bigint] NOT NULL,
	[user_id] [bigint] NOT NULL,
	[created_at] [datetime] NOT NULL,
 CONSTRAINT [PK_tweet_hashtag] PRIMARY KEY CLUSTERED 
(
	[run_id] ASC,
	[tag] ASC,
	[tweet_id] ASC,
	[user_id] ASC
)WITH (
	PAD_INDEX  = OFF,
	STATISTICS_NORECOMPUTE  = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS  = ON,
	ALLOW_PAGE_LOCKS  = ON,
	DATA_COMPRESSION = PAGE,
	FILLFACTOR = 80
) ON [PRIMARY]
) ON [PRIMARY]

GO

----------

CREATE TABLE [dbo].[user_hashtag](
	[run_id] [smallint] NOT NULL,
	[tag] [nvarchar](50) NOT NULL,
	[user_id] [bigint] NOT NULL
 CONSTRAINT [PK_user_hashtag] PRIMARY KEY CLUSTERED 
(
	[run_id] ASC,
	[user_id] ASC,
	[tag] ASC
)WITH (
	PAD_INDEX  = OFF,
	STATISTICS_NORECOMPUTE  = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS  = ON,
	ALLOW_PAGE_LOCKS  = ON,
	DATA_COMPRESSION = PAGE,
	FILLFACTOR = 80
) ON [PRIMARY]
) ON [PRIMARY]

GO

----------

CREATE TABLE [dbo].[tweet_retweet](
	[run_id] [bigint] NOT NULL,
	[tweet_id] [bigint] NOT NULL,
	[user_id] [bigint] NOT NULL,
	[retweeted_tweet_id] [bigint] NOT NULL,
	[retweeted_user_id] [bigint] NOT NULL,
	[created_at] [datetime] NOT NULL,
	[retweeted_at] [datetime] NOT NULL,
 CONSTRAINT [PK_tweet_retweet] PRIMARY KEY CLUSTERED 
(
	[run_id] ASC,
	[user_id] ASC,
	[retweeted_user_id] ASC,
	[tweet_id] ASC
)WITH (
	PAD_INDEX  = OFF,
	STATISTICS_NORECOMPUTE  = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS  = ON,
	ALLOW_PAGE_LOCKS  = ON,
	DATA_COMPRESSION = PAGE,
	FILLFACTOR = 80
) ON [PRIMARY]
) ON [PRIMARY]

GO

----------

CREATE TABLE [dbo].[tweet_user_mention](
	[run_id] [smallint] NOT NULL,
	[tweet_id] [bigint] NOT NULL,
	[user_id] [bigint] NOT NULL,
	[mentioned_user_id] [bigint] NOT NULL,
 CONSTRAINT [PK_user_mentions] PRIMARY KEY CLUSTERED 
(
	[run_id] ASC,
	[user_id] ASC,
	[mentioned_user_id] ASC,
	[tweet_id] ASC
)WITH (
	PAD_INDEX  = OFF,
	STATISTICS_NORECOMPUTE  = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS  = ON,
	ALLOW_PAGE_LOCKS  = ON,
	DATA_COMPRESSION = PAGE,
	FILLFACTOR = 80
) ON [PRIMARY]
) ON [PRIMARY]

GO

--------

CREATE TABLE [dbo].[tweet_url]
(
	[run_id] [smallint] NOT NULL,
	[tweet_id] [bigint] NOT NULL,
	[user_id] [bigint] NOT NULL,
	[url_id] [char](8) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[expanded_url] [varchar](8000),
CONSTRAINT [PK_tweet_url] PRIMARY KEY CLUSTERED 
(
	[run_id] ASC,
	[url_id] ASC,
	[tweet_id] ASC
)WITH (
	PAD_INDEX  = OFF,
	STATISTICS_NORECOMPUTE  = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS  = ON,
	ALLOW_PAGE_LOCKS  = ON,
	DATA_COMPRESSION = PAGE,
	FILLFACTOR = 80
) ON [URL]
) ON [URL]

GO


--------

CREATE TABLE [dbo].[user](
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
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (
	PAD_INDEX  = OFF,
	STATISTICS_NORECOMPUTE  = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS  = ON,
	ALLOW_PAGE_LOCKS  = ON,
	DATA_COMPRESSION = PAGE,
	FILLFACTOR = 50
	) ON [USER]
) ON [USER]

GO

--------

CREATE TABLE [dbo].[user_update](
	[run_id] [smallint] NOT NULL,
	[user_id] [bigint] NOT NULL,
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
	[utc_offset] [int] NULL,
	[verified] [bit] NOT NULL
) ON [USERUPDATE]

GO

----------

/*
CREATE TABLE [dbo].[user_friend](
	[run_id] [smallint] NOT NULL,
	[user_id] [bigint] NOT NULL,
	[friend_user_id] [bigint] NOT NULL,
	CONSTRAINT [PK_user_friend] PRIMARY KEY CLUSTERED
	(
		[run_id] ASC,
		[user_id] ASC,
		[friend_user_id] ASC
	) WITH (
		STATISTICS_NORECOMPUTE  = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS  = ON,
		ALLOW_PAGE_LOCKS  = ON,
		DATA_COMPRESSION = PAGE
	) ON [USER]
) ON [USER]

GO*/

----------

CREATE TABLE [dbo].[user_follower](
	[run_id] [smallint] NOT NULL,
	[user_id] [bigint] NOT NULL,
	[follower_user_id] [bigint] NOT NULL,
	CONSTRAINT [PK_user_follower] PRIMARY KEY CLUSTERED
	(
		[run_id] ASC,
		[user_id] ASC,
		[follower_user_id] ASC
	) WITH (
		STATISTICS_NORECOMPUTE  = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS  = ON,
		ALLOW_PAGE_LOCKS  = ON,
		DATA_COMPRESSION = PAGE,
		FILLFACTOR = 80
	) ON [FOLLOWER]
) ON [FOLLOWER]

GO

----------

CREATE TABLE [dbo].[user_location_cluster](
	[run_id] [smallint] NOT NULL,
	[user_id] [bigint] NOT NULL,
	[cluster_id] [tinyint] NOT NULL,
	[location_count] [int] NOT NULL,
	[location_count_trimmed] [int] NOT NULL,
	[sigma] [float] NOT NULL,
	[lat] [float] NOT NULL,
	[lon] [float] NOT NULL,
	[cx] [float] NOT NULL,
	[cy] [float] NOT NULL,
	[cz] [float] NOT NULL,
	[htm_id] [bigint] NOT NULL,
	[is_day] [bit] NULL,
	[day_count] [int] NOT NULL,
	[night_count] [int] NOT NULL,
	[iterations] [tinyint] NOT NULL,
 CONSTRAINT [PK_user_location] PRIMARY KEY CLUSTERED 
(
	[run_id] ASC,
	[user_id] ASC,
	[cluster_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [USER]
) ON [USER]

GO

----------

-- Create views

CREATE VIEW [dbo].[hashtag]
AS
	SELECT run_id, tag, COUNT(*) AS count
	FROM user_hashtag
	GROUP BY run_id, tag

GO

--

CREATE VIEW [dbo].[user_hashtag_any]
AS
	SELECT a.run_id, a.user_id AS user_a_id, b.user_id AS user_b_id, COUNT(*) AS count
	FROM user_hashtag a WITH (INDEX(PK_user_hashtag))
	INNER LOOP JOIN user_hashtag b WITH (INDEX(IX_user_hashtag))
		ON a.run_id = b.run_id AND a.tag = b.tag AND  a.user_id < b.user_id
	GROUP BY a.run_id, b.run_id, a.user_id, b.user_id

GO

--

CREATE VIEW [dbo].[user_location]
AS
	SELECT
		run_id, user_id, tweet_id, created_at, lon, lat, cx, cy, cz, htm_id
	FROM tweet
	WHERE lon IS NOT NULL AND lat IS NOT NULL

GO

--

CREATE VIEW [dbo].[user_mention]
AS
	SELECT run_id, user_id, mentioned_user_id, COUNT(*) AS count
	FROM tweet_user_mention
	GROUP BY run_id, user_id, mentioned_user_id

GO

--

CREATE VIEW [dbo].[user_mention_any]
AS
	WITH q AS
	(
		SELECT run_id, user_id AS user_a_id, mentioned_user_id AS user_b_id
		FROM tweet_user_mention
		
		UNION ALL
		
		SELECT run_id, mentioned_user_id AS user_a_id, user_id AS user_b_id
		FROM tweet_user_mention
	)
	SELECT run_id, user_a_id, user_b_id, COUNT(*) AS count
	FROM q
	WHERE user_a_id < user_b_id
	GROUP BY run_id, user_a_id, user_b_id

GO

--

CREATE VIEW [dbo].[user_mention_mutual]
AS
	-- might need to use inner loop join
	SELECT DISTINCT a.run_id, a.user_id AS user_a_id, b.user_id AS user_b_id
	FROM tweet_user_mention a
	INNER JOIN tweet_user_mention b
		ON a.run_id = b.run_id AND a.user_id = b.mentioned_user_id AND a.mentioned_user_id = b.user_id
	WHERE a.user_id < b.user_id

GO

--

CREATE VIEW [dbo].[user_reply]
AS
	SELECT
		run_id, user_id, in_reply_to_user_id, COUNT(*) AS count
	FROM tweet
	WHERE in_reply_to_user_id IS NOT NULL
	GROUP BY run_id, user_id, in_reply_to_user_id

GO

--

CREATE VIEW [dbo].[user_reply_any]
AS
	WITH q AS
	(
		SELECT run_id, user_id AS user_a_id, in_reply_to_user_id AS user_b_id
		FROM tweet
		WHERE in_reply_to_user_id IS NOT NULL
		
		UNION ALL
		
		SELECT run_id, in_reply_to_user_id AS user_a_id, user_id AS user_b_id
		FROM tweet
		WHERE in_reply_to_user_id IS NOT NULL
	)
	SELECT run_id, user_a_id, user_b_id, COUNT(*) AS count
	FROM q
	WHERE user_a_id < user_b_id
	GROUP BY run_id, user_a_id, user_b_id

GO

--

CREATE VIEW [dbo].[user_reply_mutual]
AS
	SELECT DISTINCT a.run_id, a.user_id AS user_a_id, b.user_id AS user_b_id
	FROM tweet a
	INNER JOIN tweet b
		ON a.user_id = b.in_reply_to_user_id AND a.in_reply_to_user_id = b.user_id AND a.run_id = b.run_id
	WHERE a.user_id < b.user_id

GO

--

CREATE VIEW [dbo].[user_retweet]
AS
	SELECT run_id, user_id, retweeted_user_id, COUNT(*) AS count
	FROM tweet_retweet
	GROUP BY run_id, user_id, retweeted_user_id

GO

--

CREATE VIEW [dbo].[user_retweet_any]
AS
	WITH q AS
	(
		SELECT run_id, user_id AS user_a_id, retweeted_user_id AS user_b_id
		FROM tweet_retweet

		UNION ALL
		
		SELECT run_id, retweeted_user_id AS user_a_id, user_id AS user_b_id
		FROM tweet_retweet
	)
	SELECT run_id, user_a_id, user_b_id, COUNT(*) as count
	FROM q
	WHERE user_a_id < user_b_id
	GROUP BY run_id, user_a_id, user_b_id

GO

--

CREATE VIEW [dbo].[user_retweet_mutual]
AS

	SELECT DISTINCT a.run_id, a.user_id AS user_a_id, b.user_id AS user_b_id
	FROM tweet_retweet a
	INNER JOIN tweet_retweet b
		ON a.user_id = b.retweeted_user_id AND a.retweeted_user_id = b.user_id AND a.run_id = b.run_id
	WHERE a.user_id < b.user_id

GO

--

CREATE VIEW [dbo].[tweet_lang_en] WITH SCHEMABINDING 
AS
	SELECT tweet.run_id, tweet.tweet_id, tweet.created_at, tweet.utc_offset, tweet.user_id, tweet.place_id,
			tweet.lon, tweet.lat, cx, cy, cz, htm_id,
			tweet.in_reply_to_tweet_id, tweet.in_reply_to_user_id,
			tweet.possibly_sensitive, tweet.possibly_sensitive_editable, tweet.retweet_count, tweet.text, tweet.truncated, tweet.lang,
			unique_id
	FROM dbo.tweet
	WHERE tweet.lang = 'en'
GO