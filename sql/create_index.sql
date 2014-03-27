USE Twitter

CREATE NONCLUSTERED INDEX [IX_tweet_location] ON [dbo].[tweet]
(
	[run_id] ASC,
	[user_id] ASC,
	[tweet_id] ASC
)
INCLUDE ( 	[created_at],
	[lon],
	[lat],
	[cx],
	[cy],
	[cz],
	[htm_id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [TWEETIDX]


GO


CREATE NONCLUSTERED INDEX [IX_tweet_htm] ON [dbo].[tweet]
(
	[run_id] ASC,
	[htm_id] ASC
)
INCLUDE ( 	
	[user_id],
	[created_at],
	[lon],
	[lat],
	[cx],
	[cy],
	[cz]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [TWEETIDX]


GO


CREATE NONCLUSTERED INDEX [IX_tweet_reply] ON [dbo].[tweet]
(
	[run_id] ASC,
	[user_id] ASC,
	[in_reply_to_user_id] ASC
)WITH (
--MAXDOP = 2,
DATA_COMPRESSION=PAGE,
SORT_IN_TEMPDB = ON,
PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [TWEETIDX]


GO

CREATE NONCLUSTERED INDEX [IX_tweet_reply_reverse] ON [dbo].[tweet]
(
	[run_id] ASC,
	[in_reply_to_user_id] ASC,
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [TWEETIDX]

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tweet_unique_id] ON [dbo].[tweet]
(
	[unique_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [TWEETIDX]


GO

------------------------------------

CREATE NONCLUSTERED INDEX [IX_user_hashtag] ON [dbo].[user_hashtag]
(
	[run_id] ASC,
	[tag] ASC,
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


GO

------------------------------------

CREATE NONCLUSTERED INDEX [IX_tweet_retweet_reverse] ON [dbo].[tweet_retweet]
(
	[run_id] ASC,
	[retweeted_user_id] ASC,
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


GO

------------------------------------

CREATE NONCLUSTERED INDEX [IX_user_mentions_reverse] ON [dbo].[tweet_user_mention]
(
	[run_id] ASC,
	[mentioned_user_id] ASC,
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


GO

-------------------------------------

CREATE NONCLUSTERED INDEX [IX_user_update] ON [dbo].[user_update] 
(
	[run_id] ASC,
	[user_id] ASC,
	[tweeted_at] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [USERUPDATE]


GO