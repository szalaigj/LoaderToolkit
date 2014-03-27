CREATE VIEW [dbo].[hashtag]
(
--------------------------------------------------------------------------------
--/ <summary>Returns all hashtags with the number of times they were mentioned.</summary>
--/ <remarks></remarks>
--/ <example>The following query returns the most popular hashtags.
--/ <query>SELECT TOP 100 * FROM hashtag WHERE run_id = 2004
--/ ORDER BY [count] DESC</query></example>
--------------------------------------------------------------------------------
	run_id,		--/ <column>Run identifier.</column>
	tag,		--/ <column>Hashtag word without the leading #.</column>
	count		--/ <column>Number of times the tag was mentioned.</column>
)
AS
	SELECT run_id, tag, COUNT(*) AS count
	FROM user_hashtag
	GROUP BY run_id, tag

GO

CREATE VIEW [dbo].[user_hashtag_any]
(
--------------------------------------------------------------------------------
--/ <summary>Returns user pairs using the same hashtag.</summary>
--/ <remarks>User identifiers are ordered that user_a_id is always less than
--/ user_b_id. Slow view, use carefully.</remarks>
--------------------------------------------------------------------------------
	run_id,
	user_a_id,
	user_b_id,
	count
)
AS
	SELECT a.run_id, a.user_id AS user_a_id, b.user_id AS user_b_id, COUNT(*) AS count
	FROM user_hashtag a WITH (INDEX(PK_user_hashtag))
	INNER LOOP JOIN user_hashtag b WITH (INDEX(IX_user_hashtag))
		ON a.run_id = b.run_id AND a.tag = b.tag AND  a.user_id < b.user_id
	GROUP BY a.run_id, b.run_id, a.user_id, b.user_id

GO

CREATE VIEW [dbo].[user_location]
(
--------------------------------------------------------------------------------
--/ <summary>Returns user locations if GPS coordinates are available.</summary>
--------------------------------------------------------------------------------
	run_id,			--/ <column>Run identifier</column>
	user_id,		--/ <column>Identifier of the tweeting user.</column>
	tweet_id,		--/ <column>Tweet identifier. Unique within each run.</column>
	created_at,		--/ <column>UTC time when tweet was tweeted.</column>
	lon,			--/ <column>GPS longitude.</column>
	lat,			--/ <column>GPS lattitude.</column>
	cx,				--/ <column>Cartesian X coordinate of position unit vector.</column>
	cy,				--/ <column>Cartesian Y coordinate of position unit vector.</column>
	cz,				--/ <column>Cartesian Z coordinate of position unit vector.</column>
	htm_id			--/ <column>HTMID of position unit vector.</column>
)
AS
	SELECT
		run_id, user_id, tweet_id, created_at, lon, lat, cx, cy, cz, htm_id
	FROM tweet
	WHERE lon IS NOT NULL AND lat IS NOT NULL

GO

CREATE VIEW [dbo].[user_mention]
(
--------------------------------------------------------------------------------
--/ <summary>Returns the directed edges of the user mention graph.</summary>
--------------------------------------------------------------------------------
	run_id,				--/ <column>Run identifier.</column>
	user_id,			--/ <column>Identified of the user mentioning the other.</column>
	mentioned_user_id,  --/ <column>Identifier of the mentioned user.</column>
	count				--/ <column>Number of occurances.</column>
)
AS
	SELECT run_id, user_id, mentioned_user_id, COUNT(*) AS count
	FROM tweet_user_mention
	GROUP BY run_id, user_id, mentioned_user_id

GO

CREATE VIEW [dbo].[user_mention_any]
(
--------------------------------------------------------------------------------
--/ <summary>Returns the undirected edges of the user mention graph.</summary>
--/ <remarks>User identifiers are ordered that user_a_id is always less than user_b_id.</remarks>
--------------------------------------------------------------------------------
	run_id,				--/ <column>Run identifier.</column>
	user_a_id,			--/ <column>Identifier of the first user.</column>
	user_b_id,			--/ <column>Identifier of the other user.</column>
	count				--/ <column>Number of all mentions.</column>
)
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

CREATE VIEW [dbo].[user_mention_mutual]
(
--------------------------------------------------------------------------------
--/ <summary>Returns the edges of the mutual user mention graph.</summary>
--/ <remarks>User identifiers are ordered such that user_a_id is always less than user_b_id.</remarks>
--------------------------------------------------------------------------------
	run_id,				--/ <column>Run identifier.</column>
	user_a_id,			--/ <column>Identifier of the first user.</column>
	user_b_id			--/ <column>Identifier of the other user.</column>
)
AS
	-- might need to use inner loop join
	SELECT DISTINCT a.run_id, a.user_id AS user_a_id, b.user_id AS user_b_id
	FROM tweet_user_mention a
	INNER JOIN tweet_user_mention b
		ON a.run_id = b.run_id AND a.user_id = b.mentioned_user_id AND a.mentioned_user_id = b.user_id
	WHERE a.user_id < b.user_id

GO

CREATE VIEW [dbo].[user_reply]
(
--------------------------------------------------------------------------------
--/ <summary>Returns the directed edges of the user reply graph.</summary>
--/ <remarks></remarks>
--------------------------------------------------------------------------------
	run_id,					--/ <column>Run identifier.</column>
	user_id,				--/ <column>Identifier of the user replying the other.</column>
	in_reply_to_user_id,	--/ <column>Identifier of the user sending the original tweet.</column>
	count					--/ <column>Number of occurances.</column>
)
AS
	SELECT
		run_id, user_id, in_reply_to_user_id, COUNT(*) AS count
	FROM tweet
	WHERE in_reply_to_user_id IS NOT NULL
	GROUP BY run_id, user_id, in_reply_to_user_id

GO

--

CREATE VIEW [dbo].[user_reply_any]
(
--------------------------------------------------------------------------------
--/ <summary>Returns the undirected edges of the user reply graph.</summary>
--/ <remarks>User identifiers are ordered such that user_a_id is always less than user_b_id.</remarks>
--------------------------------------------------------------------------------
	run_id,				--/ <column>Run identifier.</column>
	user_a_id,			--/ <column>Identified of the first user.</column>
	user_b_id,			--/ <column>Identifier of the other user.</column>
	count				--/ <column>Number of occurances.</column>
)
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
(
--------------------------------------------------------------------------------
--/ <summary>Returns the edges of the mutual user reply graph.</summary>
--/ <remarks>User identifiers are ordered such that user_a_id is always less than user_b_id.</remarks>
--------------------------------------------------------------------------------
	run_id,				--/ <column>Run identifier.</column>
	user_a_id,			--/ <column>Identifier of the first user.</column>
	user_b_id			--/ <column>Identifier of the other user.</column>
)
AS
	SELECT DISTINCT a.run_id, a.user_id AS user_a_id, b.user_id AS user_b_id
	FROM tweet a
	INNER JOIN tweet b
		ON a.user_id = b.in_reply_to_user_id AND a.in_reply_to_user_id = b.user_id AND a.run_id = b.run_id
	WHERE a.user_id < b.user_id

GO

--

CREATE VIEW [dbo].[user_retweet]
(
--------------------------------------------------------------------------------
--/ <summary>Returns the directed edges of the user retweet graph.</summary>
--/ <remarks></remarks>
--------------------------------------------------------------------------------
	run_id,					--/ <column>Run identifier.</column>
	user_id,				--/ <column>Identifier of the user retweeting the other.</column>
	retweeted_user_id,		--/ <column>Identifier of the user sending the original tweet.</column>
	count					--/ <column>Number of occurances.</column>
)
AS
	SELECT run_id, user_id, retweeted_user_id, COUNT(*) AS count
	FROM tweet_retweet
	GROUP BY run_id, user_id, retweeted_user_id

GO

--

CREATE VIEW [dbo].[user_retweet_any]
(
--------------------------------------------------------------------------------
--/ <summary>Returns the undirected edges of the user retweet graph.</summary>
--/ <remarks>User identifiers are ordered such that user_a_id is always less than user_b_id.</remarks>
--------------------------------------------------------------------------------
	run_id,				--/ <column>Run identifier.</column>
	user_a_id,			--/ <column>Identified of the first user.</column>
	user_b_id,			--/ <column>Identifier of the other user.</column>
	count				--/ <column>Number of occurances.</column>
)
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
(
--------------------------------------------------------------------------------
--/ <summary>Returns the edges of the mutual user retweet graph.</summary>
--/ <remarks>User identifiers are ordered such that user_a_id is always less than user_b_id.</remarks>
--------------------------------------------------------------------------------
	run_id,				--/ <column>Run identifier.</column>
	user_a_id,			--/ <column>Identifier of the first user.</column>
	user_b_id			--/ <column>Identifier of the other user.</column>
)
AS

	SELECT DISTINCT a.run_id, a.user_id AS user_a_id, b.user_id AS user_b_id
	FROM tweet_retweet a
	INNER JOIN tweet_retweet b
		ON a.user_id = b.retweeted_user_id AND a.retweeted_user_id = b.user_id AND a.run_id = b.run_id
	WHERE a.user_id < b.user_id

GO
