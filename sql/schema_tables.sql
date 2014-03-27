CREATE TABLE [dbo].[run]
(
--------------------------------------------------------------------------------
--/ <summary>Contains a list of data collection runs.</summary>
--/ <remarks>This table is maintained manually and start and stop
--/ times usually contain some buffer period.</remarks>
--------------------------------------------------------------------------------
	[run_id] [smallint] NOT NULL,				--/ <column content="meta.id">Unique ID of the data collection run.</column>
	[started_at] [datetime] NOT NULL,			--/ <column content="time.start">Start of data collection.</column>
	[stopped_at] [datetime] NOT NULL,			--/ <column content="time.end">End of data collection.</column>
	CONSTRAINT [PK_run] PRIMARY KEY CLUSTERED
	(
		[run_id] ASC
	) WITH (
		PAD_INDEX  = OFF,
		STATISTICS_NORECOMPUTE  = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS  = ON,
		ALLOW_PAGE_LOCKS  = ON
	) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[tweet]
(
--------------------------------------------------------------------------------
--/ <summary>Contains individual tweets.</summary>
--/
--/ <remarks>Tweets are organized into data collection runs identified by the
--/ column run_id. Every index on the table contains run_id as the first column.</remarks>
--/
--/ <example>
--/ When querying the tweet table an explicit constraint on the column run_id must be set for
--/ efficiency since run_id is the first indexed column in all indexes. For example the query
--/ <query>SELECT TOP 100 * FROM tweet WHERE lon IS NOT NULL</query>
--/ will run slowly because the beginning of the table might not contain tweets with GPS
--/ coordinates. Restricting to a run_id with mostly geotagged data will return results
--/ quickly:
--/ <query>SELECT TOP 100 * FROM tweet WHERE run_id = 1004 lon IS NOT NULL</query>
--/ </example>
--------------------------------------------------------------------------------
	[run_id] [smallint] NOT NULL,				--/ <column content="meta.id">Run identifier</column>
	[tweet_id] [bigint] NOT NULL,				--/ <column content="meta.id">Tweet identifier. Unique within each run.</column>
	[created_at] [datetime] NOT NULL,			--/ <column content="time.epoch">UTC time when tweet was tweeted.</column>
	[utc_offset] [int] NULL,					--/ <column unit="s" content="time.offset">Offset from UTC in seconds to local time of tweeter.</column>
	[user_id] [bigint] NOT NULL,				--/ <column>Identifier of the tweeting user.</column>
	[place_id] [char](16) NULL,					--/ <column>Identifier of a place associated with the twitter user. This is not geo data but a custom set property.</column>
	[lon] [float] NULL,							--/ <column unit="deg">GPS longitude.</column>
	[lat] [float] NULL,							--/ <column unit="deg">GPS lattitude.</column>
	[cx] [float] NOT NULL,						--/ <column>Cartesian X coordinate of position unit vector.</column>
	[cy] [float] NOT NULL,						--/ <column>Cartesian Y coordinate of position unit vector.</column>
	[cz] [float] NOT NULL,						--/ <column>Cartesian Z coordinate of position unit vector.</column>
	[htm_id] [bigint] NOT NULL,					--/ <column>HTMID of position unit vector.</column>
	[in_reply_to_tweet_id] [bigint] NULL,		--/ <column>ID of the tweet this tweet is a reply to.</column>
	[in_reply_to_user_id] [bigint] NULL,		--/ <column>ID of user whose tweet this tweet is a reply to.</column>
	[possibly_sensitive] [bit] NULL,			--/ <column>Flag marking possibly sensitive tweet.</column>
	[possibly_sensitive_editable] [bit] NULL,	--/ <column>Flag marking of possibly_sensitive flag is editable when retweeted.</column>
	[retweet_count] [int] NOT NULL,				--/ <column>Number of times the tweet had been retweeted when it was added to the database the first time. This counter is not updated after the record is added to the database.</column>
	[text] [nvarchar](150) NOT NULL,			--/ <column>Contents of the tweet.</column>
	[truncated] [bit] NOT NULL,					--/ <column>Flag is 1 if text is truncated.</column>
	[lang] [varchar](5) NOT NULL,				--/ <column>Language of tweet, as specified by the tweeter.</column>
	[lang_word_count] [tinyint] NOT NULL,		--/ <column>Number of words used for automatic language detection.</column>
	[lang_guess1] [char](2) NOT NULL,			--/ <column>Automatic language detection best guess.</column>
	[lang_guess2] [char](2) NOT NULL,			--/ <column>Automatic language detection second best guess.</column>
	[unique_id] [bigint] IDENTITY(1,1) NOT NULL,--/ <column>Unique sequential ID of tweet. Might change, don't use.</column>
	CONSTRAINT [PK_tweet] PRIMARY KEY CLUSTERED 
	(
		[run_id] ASC,
		[tweet_id] ASC
	) WITH (
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

CREATE TABLE [dbo].[tweet_hour]
(
--------------------------------------------------------------------------------
--/ <summary>Lookup table to figure out tweet_id from time.</summary>
--/
--/ <remarks>Tweet idenfiers (tweet_id) are assigned incrementally by Twitter.
--/ As tables are indexes by tweet_id instead of created_at, when constaining queries
--/ by time, this lookup table is to be used for faster query execution. Time values
--/ are stored in UTC and quantized to whole hours.</remarks>
--/
--/ <example>
--/ The following query is very inefficient because the table is organized by tweet_id
--/ but the server doesn't have the knowledge that ordering by tweet_id is actually
--/ the same ordering by created_at:
--/ <query>SELECT COUNT(*) FROM tweet
--/ WHERE run_id=2004 AND created_at BETWEEN '01/01/2013' AND '02/01/2013'</query>
--/ The fastest way to execute the query is the following.
--/ <query>DECLARE @start bigint;
--/ DECLARE @end bigint;
--/ 
--/ SELECT @start = tweet_id FORM tweet_hour WHERE time = '01/01/2013' AND run_id = 2004
--/ SELECT @end = tweet_id FROM tweet_hour WHERE time = '02/01/2013' AND run_id = 2004
--/ 
--/ SELECT COUNT(*) FROM tweet
--/ WHERE run_id = 2004 AND tweet_id BETWEEN @start AND @end</query>
--/ </example>
--------------------------------------------------------------------------------
	[run_id] [smallint] NOT NULL,	--/ <column>Run identifier.</column>
	[time] [datetime] NOT NULL,		--/ <column>Time in UTC, always whole hour.</column>
	[tweet_id] [bigint] NOT NULL,	--/ <column>Tweet identifier at time.</column>
	CONSTRAINT [PK_tweet_hour] PRIMARY KEY CLUSTERED 
	(
		[run_id] ASC,
		[time] ASC
	) WITH (
		PAD_INDEX  = OFF,
		STATISTICS_NORECOMPUTE  = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS  = ON,
		ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[tweet_hashtag]
(
--------------------------------------------------------------------------------
--/ <summary>Contains hashtags mentioned in tweets.</summary>
--/
--/ <remarks>Hashtags are words appearing in tweets in the form of #hashtag.
--/ This table contains all mentioned hashtags along with the tweet they appeared
--/ in and user used by. Please note, that the full-text index built on the tweet
--/ table ignores hashtags and using this table is the only way to find tags.
--/ See the user_hashtag table also.</remarks>
--------------------------------------------------------------------------------
	[run_id] [smallint] NOT NULL,		--/ <column>Run identifier.</column>
	[tag] [nvarchar](50) NOT NULL,		--/ <column>Hashtag word without the leading #.</column>
	[tweet_id] [bigint] NOT NULL,		--/ <column>ID of tweet this hashtag was mentioned in.</column>
	[user_id] [bigint] NOT NULL,		--/ <column>ID of user this hashtag was used by.</column>
	[created_at] [datetime] NOT NULL,	--/ <column>Time this hashtag was mentioned at.</column>
	CONSTRAINT [PK_tweet_hashtag] PRIMARY KEY CLUSTERED 
	(
		[run_id] ASC,
		[tag] ASC,
		[tweet_id] ASC,
		[user_id] ASC
	) WITH (
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

CREATE TABLE [dbo].[user_hashtag]
(
--------------------------------------------------------------------------------
--/ <summary>Contains hashtags mentioned by users.</summary>
--/
--/ <remarks>Hashtags are words appearing in tweets in the form of #hashtag.
--/ This table contains hashtags organized by users who mentioned them (at any time).
--/ See the tweet_hashtag table also.</remarks>
--------------------------------------------------------------------------------
	[run_id] [smallint] NOT NULL,		--/ <column>Run identifier.</column>
	[tag] [nvarchar](50) NOT NULL,		--/ <column>Hashtag word without the leading #.</column>
	[user_id] [bigint] NOT NULL			--/ <column>ID of user this hashtag was used by.</column>
	CONSTRAINT [PK_user_hashtag] PRIMARY KEY CLUSTERED 
	(
		[run_id] ASC,
		[user_id] ASC,
		[tag] ASC
	) WITH (
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

CREATE TABLE [dbo].[tweet_retweet]
(
--------------------------------------------------------------------------------
--/ <summary>Contains tweet-retweet pairs.</summary>
--/
--/ <remarks>Older tweets can be retweeted (i.e. forwarded). Whenever a retweet is
--/ received on the stream, the original tweet is embedded and stored in the
--/ database as well using the same run_id. This is why very old tweets, way before
--/ the beginning of data collection, appear in the data.</remarks>
--------------------------------------------------------------------------------
	[run_id] [bigint] NOT NULL,					--/ <column>Run identifier.</column>
	[tweet_id] [bigint] NOT NULL,				--/ <column>New tweet idenfifier.</column>
	[user_id] [bigint] NOT NULL,				--/ <column>User identifier the retweet was sent by.</column>
	[retweeted_tweet_id] [bigint] NOT NULL,		--/ <column>Identifier of the original tweet.</column>
	[retweeted_user_id] [bigint] NOT NULL,		--/ <column>User identifier the original tweet was sent by.</column>
	[created_at] [datetime] NOT NULL,			--/ <column>UTC time the original tweet was sent at.</column>
	[retweeted_at] [datetime] NOT NULL,			--/ <column>UTC time the tweet was retweeted at.</column>
	CONSTRAINT [PK_tweet_retweet] PRIMARY KEY CLUSTERED 
	(
		[run_id] ASC,
		[user_id] ASC,
		[retweeted_user_id] ASC,
		[tweet_id] ASC
	) WITH (
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

CREATE TABLE [dbo].[tweet_user_mention]
(
--------------------------------------------------------------------------------
--/ <summary>Contains information about one user mentioning the other.</summary>
--/
--/ <remarks>Tweeters can mention each other by prefixing screen names with an
--/ at sign in the form of @screen_name. These mentions are parsed and stored
--/ in this table. User mentions are a one way relationship between two users.</remarks>
--------------------------------------------------------------------------------
	[run_id] [smallint] NOT NULL,			--/ <column>Run identifier.</column>
	[tweet_id] [bigint] NOT NULL,			--/ <column>Identifier of the tweet the mentioning happened in.</column>
	[user_id] [bigint] NOT NULL,			--/ <column>Identifier of the mentioning user.</column>
	[mentioned_user_id] [bigint] NOT NULL,	--/ <column>Identifier of the mentioned user.</column>
	CONSTRAINT [PK_user_mentions] PRIMARY KEY CLUSTERED 
	(
		[run_id] ASC,
		[user_id] ASC,
		[mentioned_user_id] ASC,
		[tweet_id] ASC
	) WITH (
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

CREATE TABLE [dbo].[tweet_url]
(
--------------------------------------------------------------------------------
--/ <summary>Contains URLs mentioned in tweets.</summary>
--/
--/ <remarks>Twitter substitutes original URLs with shortened ones in the form
--/ of http://t.co/0004FhC6. The resolutions of these are stored in this table. The
--/ same URL might be mentioned multiple times.</remarks>
--/
--/ <example> This table can be used to find reference to youtube videos. For example,
--/ the YouTube of the gangnam style video is 9bZkp7q19f0, appearing in the original
--/ URL http://www.youtube.com/watch?v=9bZkp7q19f0. The following query, though not too
--/ quickly, finds all tweets mentioning the video.
--/ <query>SELECT tweet_id FROM tweet_url
--/ WHERE expanded_url LIKE '%9bZkp7q19f0%'</query></example>
--------------------------------------------------------------------------------
	[run_id] [smallint] NOT NULL,			--/ <column>Run identifier.</column>
	[tweet_id] [bigint] NOT NULL,			--/ <column>Identifier of the tweet the URL was mentioned in.</column>
	[user_id] [bigint] NOT NULL,			--/ <column>Identifier of the user the URL was sent by.</column>
	[url_id] [char](8) NOT NULL,			--/ <column>Identifier of the URL (portion following http://t.co/.)</column>
	[created_at] [datetime] NOT NULL,		--/ <column>Time the tweet was sent at.</column>
	[expanded_url] [varchar](8000),			--/ <column>Original URL.</column>
	CONSTRAINT [PK_tweet_url] PRIMARY KEY CLUSTERED 
	(
		[run_id] ASC,
		[url_id] ASC,
		[tweet_id] ASC
	) WITH (
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

CREATE TABLE [dbo].[user]
(
--------------------------------------------------------------------------------
--/ <summary>Contains information about users.</summary>
--/
--/ <remarks>Information about all users tweeting in the sampling period is
--/ stored here. Retweeted users also stored while users only mentined by screen name don't
--/ necessarily appear here. The table contains the state of user profiles when users
--/ appeared the first time in the stream. No further updates to this table are made
--/ for performance reasons. See user_update table for profile change details.</remarks>
--------------------------------------------------------------------------------
	[user_id] [bigint] NOT NULL,					--/ <column>Run identifier.</column>
	[created_at] [datetime] NOT NULL,				--/ <column>Time of registration.</column>
	[last_update_at] [datetime] NOT NULL,			--/ <column>Time the user profile in the database was last updated at.</column>
	[screen_name] [nvarchar](50) NOT NULL,			--/ <column>User name.</column>
	[description] [nvarchar](160) NOT NULL,			--/ <column>Self description.</column>
	[favourites_count] [int] NOT NULL,				--/ <column>Number of followed users at the time of last profile update.</column>
	[followers_count] [int] NOT NULL,				--/ <column>Number of followers at the time of last profile update.</column>
	[friends_count] [int] NOT NULL,					--/ <column>Number of friends at the time of last profile update.</column>
	[statuses_count] [int] NOT NULL,				--/ <column>Number of tweets till the last profile update. All tweets, usually much more than tweets in the database.</column>
	[geo_enabled] [bit] NOT NULL,					--/ <column>Indicates if GPS coordinates are tweeted.</column>
	[lang] [char](5) NOT NULL,						--/ <column>Language as specified by the users themself.</column>
	[location] [nvarchar](100) NULL,				--/ <column>Location as specified by the user. Often just funny and useless, sometimes accurate to city level.</column>
	[name] [nvarchar](30) NOT NULL,					--/ <column>Civil name of the user.</column>
	[profile_background_color] [char](6) NOT NULL,	--/ <column>Twitter user interface setting, screen backgroun color.</column>
	[profile_text_color] [char](6) NOT NULL,		--/ <column>Twitter user interface setting, text color.</column>
	[protected] [bit] NOT NULL,						--/ <column>Protected users' tweets only appear to friends.</column>
	[show_all_inline_media] [bit] NOT NULL,			--/ <column>User profile setting.</column>
	[utc_offset] [int] NULL,						--/ <column unit="s">Offset from UTC. Time zone data is usually inaccurate.</column>
	[verified] [bit] NOT NULL,						--/ <column>Indicates whether twitter has confirmed user's identity.</column>
	CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
	(
		[user_id] ASC
	) WITH (
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

CREATE TABLE [dbo].[user_update]
(
--------------------------------------------------------------------------------
--/ <summary>Contains information about user profile updates.</summary>
--/
--/ <remarks>While the user table only contains profile data available at the first time
--/ a user appears in the stream, this table contains all profile updates, usually on
--/ a daily basis. Use carefully, as this table is not ordered for data loading performance
--/ reasons.</remarks>
--------------------------------------------------------------------------------
	[run_id] [smallint] NOT NULL,					--/ <column>Run identifier.</column>
	[user_id] [bigint] NOT NULL,					--/ <column>Identifier of the user.</column>
	[tweeted_at] [datetime] NOT NULL,				--/ <column>Time of profile update.</column>
	[screen_name] [nvarchar](50) NOT NULL,			--/ <column>Screen name of user. Might change from time to time, but user_id remains the same.</column>
	[description] [nvarchar](160) NOT NULL,			--/ <column>Self description.</column>
	[favourites_count] [int] NOT NULL,				--/ <column>Number of favourites at the time of status update.</column>
	[followers_count] [int] NOT NULL,				--/ <column>Number of followers at the time of status update.</column>
	[friends_count] [int] NOT NULL,					--/ <column>Number of friends at the time of status update.</column>
	[statuses_count] [int] NOT NULL,				--/ <column>Number of tweets till the time of status update.</column>
	[geo_enabled] [bit] NOT NULL,					--/ <column>Indicates if GPS coordinates are tweeted.</column>
	[lang] [char](5) NOT NULL,						--/ <column>Language as specified by the users themself.</column>
	[location] [nvarchar](100) NULL,				--/ <column>Location as specified by the user. Often just funny and useless, sometimes accurate to city level.</column>
	[name] [nvarchar](30) NOT NULL,					--/ <column>Civil name of the user.</column>
	[profile_background_color] [char](6) NOT NULL,	--/ <column>Twitter user interface setting, screen backgroun color.</column>
	[profile_text_color] [char](6) NOT NULL,		--/ <column>Twitter user interface setting, text color.</column>
	[utc_offset] [int] NULL,						--/ <column unit="s">Offset from UTC. Time zone data is usually inaccurate.</column>
	[verified] [bit] NOT NULL						--/ <column>Indicates whether twitter has confirmed user's identity.</column>
) ON [USERUPDATE]

GO

CREATE TABLE [dbo].[user_location_cluster]
(
--------------------------------------------------------------------------------
--/ <summary>This computed table contains estimated location data for each user with coordinates.</summary>
--/
--/ <remarks>User's GPS coordinates are organized into clusters and coordinates in each cluster are averaged
--/ to get a single coordinate with error estimate. Clusters are ranked by coordinate number and only the
--/ three more numerous are kept.</remarks>
--------------------------------------------------------------------------------
	[run_id] [smallint] NOT NULL,				--/ <column>Run identifier.</column>
	[user_id] [bigint] NOT NULL,				--/ <column>Identifier of the user.</column>
	[cluster_id] [tinyint] NOT NULL,			--/ <column>Identifier of the cluster, 0, 1 or 2, 0th being the location with most tweets from.</column>
	[location_count] [int] NOT NULL,			--/ <column>Number of GPS coordinates in this cluster, accummulated for the entire run.</column>
	[location_count_trimmed] [int] NOT NULL,	--/ <column>Number of GPS coordinates left in the cluster after pruning outliers.</column>
	[sigma] [float] NOT NULL,					--/ <column unit="deg">Standard deviation of coordinates in the cluster after pruning.</column>
	[lat] [float] NOT NULL,						--/ <column unit="deg">GPS latitude.</column>
	[lon] [float] NOT NULL,						--/ <column unit="deg">GPS longitude.</column>
	[cx] [float] NOT NULL,						--/ <column>Cartesian X coordinate of position unit vector.</column>
	[cy] [float] NOT NULL,						--/ <column>Cartesian Y coordinate of position unit vector.</column>
	[cz] [float] NOT NULL,						--/ <column>Cartesian X coordinate of position unit vector.</column>
	[htm_id] [bigint] NOT NULL,					--/ <column>HTMID of position unit vector.</column>
	[is_day] [bit] NULL,						--/ <column>Indicates if most of the tweets in this cluster are sent between 8AM-8PM. NULL means undecidable.</column>
	[day_count] [int] NOT NULL,					--/ <column>Number of tweets sent between 8AM-8PM hours.</column>
	[night_count] [int] NOT NULL,				--/ <column>Number of tweets sent between 8PM-8AM hours.</column>
	[iterations] [tinyint] NOT NULL,			--/ <column>Number of iterations during outlier pruning.</column>
	CONSTRAINT [PK_user_location] PRIMARY KEY CLUSTERED 
	(
		[run_id] ASC,
		[user_id] ASC,
		[cluster_id] ASC
	) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON) ON [USER]
) ON [USER]

GO

CREATE TABLE [dbo].[user_region]
(
--------------------------------------------------------------------------------
--/ <summary>This computed table contains adminitrative region classification for each user with coordinates.</summary>
--/
--/ <remarks>User's GPS coordinates are organized into clusters and coordinates in each cluster are averaged
--/ to get a single coordinate with error estimate. Clusters are ranked by coordinate number and only the
--/ three more numerous are kept. Region for each cluster is looked up using a geo indexing technique. As exact
--/ containment detection in the geo library is expensive, users near administrative region borders might be classified
--/ into the wrong region. Some users might not be classified at all. Duplicate classifications are eliminated. The
--/ column 'exact' indicates whether the user is classified into a single region originally. exact = 0 means the user
--/ was classified into more than one regions originally and randomly assigned to one of those.</remarks>
--------------------------------------------------------------------------------
	[run_id] [smallint] NOT NULL,				--/ <column>Run identifier.</column>
	[geo_level] [tinyint] NOT NULL,				--/ <column>Geo level used to determine regions</column>
	[user_id] [bigint] NOT NULL,				--/ <column>Identifier of the user.</column>
	[cluster_id] [tinyint] NOT NULL,			--/ <column>Identifier of the cluster, 0, 1 or 2, 0th being the location with most tweets from.</column>
	[region_id] [int] NOT NULL,					--/ <column>Identifier of the administrative region the user's cluster belongs to</column>
	[exact] [bit] NOT NULL,						--/ <column>Indicates whether region match is exact</column>
	CONSTRAINT [PK_user_region] PRIMARY KEY CLUSTERED
	(
		[run_id] ASC,
		[geo_level] ASC,
		[user_id] ASC,
		[cluster_id] ASC
	) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON) ON [USER]
) ON [USER]

GO