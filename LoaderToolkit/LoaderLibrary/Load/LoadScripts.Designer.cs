﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LoaderLibrary.Load {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class LoadScripts {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal LoadScripts() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LoaderLibrary.Load.LoadScripts", typeof(LoadScripts).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- sp_dboption [$dbname], &apos;SELECT INTO/BULKCOPY&apos;, TRUE
        ///
        ///BULK INSERT [$dbname].[dbo].[$tablename]
        ///FROM &apos;$filename&apos;
        ///WITH (
        ///	CODEPAGE = 1200,				--	 unicode
        ///	DATAFILETYPE = N&apos;widechar&apos;,
        ///	FIELDTERMINATOR = N&apos;\0&apos;,
        ///	ROWTERMINATOR = N&apos;\0\r&apos;,
        ///	KEEPNULLS,
        ///	--BATCHSIZE = 1000000,		Avoid this option to prevent logging
        ///	TABLOCK
        ///	).
        /// </summary>
        public static string bulkinsert {
            get {
                return ResourceManager.GetString("bulkinsert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- sp_dboption [$dbname], &apos;SELECT INTO/BULKCOPY&apos;, TRUE
        ///
        ///BULK INSERT [$dbname].[dbo].[$tablename]
        ///FROM &apos;$filename&apos;
        ///WITH (
        ///	DATAFILETYPE = N&apos;widenative&apos;,
        ///	TABLOCK
        ///	).
        /// </summary>
        public static string bulkinsert_binary {
            get {
                return ResourceManager.GetString("bulkinsert_binary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE [$dbname].[dbo].[$tablename]
        ///(
        ///	[run_id] [smallint] NOT NULL,
        ///	[tweet_id] [bigint] NOT NULL,
        ///	[user_id] [bigint] NOT NULL	
        ///) ON [PRIMARY]
        ///.
        /// </summary>
        public static string create_delete_tweet {
            get {
                return ResourceManager.GetString("create_delete_tweet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE [$dbname].[dbo].[$tablename]
        ///(
        ///	[run_id] [smallint] NOT NULL,
        ///	[user_id] [bigint] NOT NULL,
        ///	[up_to_status_id] [bigint] NOT NULL
        ///) ON [PRIMARY]
        ///.
        /// </summary>
        public static string create_scrub_geo {
            get {
                return ResourceManager.GetString("create_scrub_geo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE [$dbname].[dbo].[$tablename]
        ///(
        ///	[run_id] [smallint] NOT NULL,
        ///	[tweet_id] [bigint] NOT NULL,
        ///	[created_at] [datetime] NOT NULL,
        ///	[utc_offset] [int] NULL,
        ///	[user_id] [bigint] NOT NULL,
        ///	[place_id] [char](16) NULL,
        ///	[lon] [float] NULL,
        ///	[lat] [float] NULL,
        ///	[cx] [float] NOT NULL,
        ///	[cy] [float] NOT NULL,
        ///	[cz] [float] NOT NULL,
        ///	[htm_id] [bigint] NOT NULL,
        ///	[in_reply_to_tweet_id] [bigint] NULL,
        ///	[in_reply_to_user_id] [bigint] NULL,
        ///	[possibly_sensitive] [bit] NULL,
        ///	[possibly_sens [rest of string was truncated]&quot;;.
        /// </summary>
        public static string create_tweet {
            get {
                return ResourceManager.GetString("create_tweet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE [$dbname].[dbo].[$tablename]
        ///(
        ///	[run_id] [smallint] NOT NULL,
        ///	[tweet_id] [bigint] NOT NULL,
        ///	[user_id] [bigint] NOT NULL,
        ///	[tag] [nvarchar](50) NOT NULL,
        ///	[created_at] [datetime] NOT NULL
        ///) ON [PRIMARY]
        ///.
        /// </summary>
        public static string create_tweet_hashtag {
            get {
                return ResourceManager.GetString("create_tweet_hashtag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE [$dbname].[dbo].[$tablename]
        ///(
        ///	[run_id] [smallint] NOT NULL,
        ///	[tweet_id] [bigint] NOT NULL,
        ///	[user_id] [bigint] NOT NULL,
        ///	[retweeted_tweet_id] [bigint] NOT NULL,
        ///	[retweeted_user_id] [bigint] NOT NULL,
        ///	[created_at] [datetime] NOT NULL,
        ///	[retweeted_at] [datetime] NOT NULL
        ///) ON [PRIMARY]
        ///.
        /// </summary>
        public static string create_tweet_retweet {
            get {
                return ResourceManager.GetString("create_tweet_retweet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE [$dbname].[dbo].[$tablename]
        ///(
        ///	[run_id] [smallint] NOT NULL,
        ///	[tweet_id] [bigint] NOT NULL,
        ///	[user_id] [bigint] NOT NULL,
        ///	[url_id] [char](8) NOT NULL,
        ///	[created_at] [datetime] NOT NULL,
        ///	[expanded_url] [varchar](8000)
        ///) ON [PRIMARY].
        /// </summary>
        public static string create_tweet_url {
            get {
                return ResourceManager.GetString("create_tweet_url", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE [$dbname].[dbo].[$tablename]
        ///(
        ///	[run_id] [smallint] NOT NULL,
        ///	[tweet_id] [bigint] NOT NULL,
        ///	[user_id] [bigint] NOT NULL,
        ///	[mentioned_user_id] [bigint] NOT NULL
        ///) ON [PRIMARY]
        ///
        ///.
        /// </summary>
        public static string create_tweet_user_mention {
            get {
                return ResourceManager.GetString("create_tweet_user_mention", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE [$dbname].[dbo].[$tablename]
        ///(
        ///	[run_id] [smallint] NOT NULL,
        ///	[user_id] [bigint] NOT NULL,
        ///	[created_at] [datetime] NOT NULL,
        ///	[tweeted_at] [datetime] NOT NULL,
        ///	[screen_name] [nvarchar](50) NOT NULL,
        ///	[description] [nvarchar](160) NOT NULL,
        ///	[favourites_count] [int] NOT NULL,
        ///	[followers_count] [int] NOT NULL,
        ///	[friends_count] [int] NOT NULL,
        ///	[statuses_count] [int] NOT NULL,
        ///	[geo_enabled] [bit] NOT NULL,
        ///	[lang] [char](5) NOT NULL,
        ///	[location] [nvarchar](100) NULL,
        ///	[name] [n [rest of string was truncated]&quot;;.
        /// </summary>
        public static string create_user {
            get {
                return ResourceManager.GetString("create_user", resourceCulture);
            }
        }

        public static string create_pupLoad
        {
            get
            {
                return ResourceManager.GetString("create_pupLoad", resourceCulture);
            }
        }

        public static string create_basesDistLoad
        {
            get
            {
                return ResourceManager.GetString("create_basesDistLoad", resourceCulture);
            }
        }

        public static string create_ref
        {
            get
            {
                return ResourceManager.GetString("create_ref", resourceCulture);
            }
        }

        public static string create_sam
        {
            get
            {
                return ResourceManager.GetString("create_sam", resourceCulture);
            }
        }

        public static string create_sreadLoad
        {
            get
            {
                return ResourceManager.GetString("create_sreadLoad", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to IF OBJECT_ID (N&apos;[$dbname].dbo.[$tablename]&apos;, N&apos;U&apos;) IS NOT NULL 
        ///DROP TABLE [$dbname].dbo.[$tablename].
        /// </summary>
        public static string drop_table {
            get {
                return ResourceManager.GetString("drop_table", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string index_delete_tweet {
            get {
                return ResourceManager.GetString("index_delete_tweet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string index_scrub_geo {
            get {
                return ResourceManager.GetString("index_scrub_geo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE CLUSTERED INDEX [IX_$tablename] ON [$dbname].[dbo].[$tablename] 
        ///(
        ///	[run_id] ASC,
        ///	[tweet_id] ASC,
        ///	[created_at] DESC
        ///) ON [PRIMARY];.
        /// </summary>
        public static string index_tweet {
            get {
                return ResourceManager.GetString("index_tweet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE CLUSTERED INDEX [IX_$tablename] ON [$dbname].[dbo].[$tablename] 
        ///(
        ///	[run_id] ASC,
        ///	[tag] ASC,
        ///	[tweet_id] ASC,
        ///	[user_id] ASC
        ///) ON [PRIMARY];
        ///
        ///
        ///CREATE NONCLUSTERED INDEX [IX_$tablename_2] ON [$dbname].[dbo].[$tablename] 
        ///(
        ///	[run_id] ASC,
        ///	[user_id] ASC,
        ///	[tag] ASC
        ///) 
        ///INCLUDE
        ///(
        ///	[tweet_id],
        ///	[created_at] )
        ///ON [PRIMARY];.
        /// </summary>
        public static string index_tweet_hashtag {
            get {
                return ResourceManager.GetString("index_tweet_hashtag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE CLUSTERED INDEX [IX_$tablename] ON [$dbname].[dbo].[$tablename] 
        ///(
        ///	[run_id] ASC,
        ///	[user_id] ASC,
        ///	[retweeted_user_id] ASC,
        ///	[tweet_id] ASC
        ///) ON [PRIMARY];.
        /// </summary>
        public static string index_tweet_retweet {
            get {
                return ResourceManager.GetString("index_tweet_retweet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE CLUSTERED INDEX [IX_$tablename] ON [$dbname].[dbo].[$tablename]
        ///(
        ///	[run_id] ASC,
        ///	[url_id] ASC,
        ///	[tweet_id] ASC
        ///) ON [PRIMARY];.
        /// </summary>
        public static string index_tweet_url {
            get {
                return ResourceManager.GetString("index_tweet_url", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///CREATE CLUSTERED INDEX [$ixname] ON [$dbname].[dbo].[$tablename]
        ///(
        ///	[run_id] ASC,
        ///	[user_id] ASC,
        ///	[mentioned_user_id] ASC,
        ///	[tweet_id] ASC
        ///)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ///.
        /// </summary>
        public static string index_tweet_user_mention {
            get {
                return ResourceManager.GetString("index_tweet_user_mention", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE CLUSTERED INDEX [IX_$tablename] ON [$dbname].[dbo].[$tablename]
        ///(
        ///	--[run_id] ASC,
        ///	[user_id] ASC,
        ///	[tweeted_at] DESC,
        ///	[screen_name] ASC,
        ///	[description] ASC,
        ///	[geo_enabled] ASC,
        ///	[lang] ASC,
        ///	[location] ASC,
        ///	[name] ASC,
        ///	[profile_background_color] ASC,
        ///	[profile_text_color] ASC,
        ///	[utc_offset] ASC,
        ///	[verified] ASC
        ///) ON [PRIMARY];.
        /// </summary>
        public static string index_user {
            get {
                return ResourceManager.GetString("index_user", resourceCulture);
            }
        }

        public static string index_pupLoad
        {
            get
            {
                return ResourceManager.GetString("index_pupLoad", resourceCulture);
            }
        }

        public static string index_basesDistLoad
        {
            get
            {
                return ResourceManager.GetString("index_basesDistLoad", resourceCulture);
            }
        }

        public static string index_ref
        {
            get
            {
                return ResourceManager.GetString("index_ref", resourceCulture);
            }
        }

        public static string index_sam
        {
            get
            {
                return ResourceManager.GetString("index_sam", resourceCulture);
            }
        }

        public static string index_sreadLoad
        {
            get
            {
                return ResourceManager.GetString("index_sreadLoad", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to -- Merge tweets
        ///dbcc traceon (610);
        ///
        ///WITH q AS
        ///(
        ///	SELECT	run_id, tweet_id, created_at, utc_offset, user_id, place_id, lon, lat, 
        ///			cx, cy, cz, htm_id,
        ///			in_reply_to_tweet_id, in_reply_to_user_id,
        ///			possibly_sensitive, possibly_sensitive_editable,
        ///			retweet_count, text, truncated, lang, lang_word_count, lang_guess1, lang_guess2,
        ///			ROW_NUMBER() OVER (PARTITION BY run_id, tweet_id ORDER BY created_at DESC) AS rn
        ///	FROM [$loaddb].[dbo].[$tablename]
        ///)
        ///MERGE [$targetdb].[dbo].[tweet] WITH (TABLO [rest of string was truncated]&quot;;.
        /// </summary>
        public static string merge_tweet {
            get {
                return ResourceManager.GetString("merge_tweet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- Merge hashtags
        ///dbcc traceon (610);
        ///
        ///WITH s AS
        ///(
        ///	SELECT DISTINCT
        ///		run_id, tweet_id, tag, user_id, created_at
        ///	FROM [$loaddb].[dbo].[$tablename]
        ///)
        ///MERGE [$targetdb].[dbo].[tweet_hashtag] WITH (TABLOCKX) AS t
        ///USING s
        ///	ON s.run_id = t.run_id AND s.tag = t.tag AND s.tweet_id = t.tweet_id AND s.user_id = t.user_id
        ///WHEN NOT MATCHED THEN
        ///	INSERT (run_id, tag, tweet_id, user_id, created_at)
        ///	VALUES (s.run_id, s.tag, s.tweet_id, s.user_id, created_at);.
        /// </summary>
        public static string merge_tweet_hashtag {
            get {
                return ResourceManager.GetString("merge_tweet_hashtag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /*
        ///-- Merge retweets
        ///dbcc traceon (610);
        ///
        ///-- Create new table for merged data
        ///CREATE TABLE [$targetdb].[dbo].[tweet_retweet_new](
        ///	[run_id] [bigint] NOT NULL,
        ///	[tweet_id] [bigint] NOT NULL,
        ///	[user_id] [bigint] NOT NULL,
        ///	[retweeted_tweet_id] [bigint] NOT NULL,
        ///	[retweeted_user_id] [bigint] NOT NULL,
        ///	[created_at] [datetime] NOT NULL,
        ///	[retweeted_at] [datetime] NOT NULL,
        /// CONSTRAINT [PK_tweet_retweet_new] PRIMARY KEY CLUSTERED 
        ///(
        ///	[run_id] ASC,
        ///	[user_id] ASC,
        ///	[retweeted_user_id] ASC,
        ///	[t [rest of string was truncated]&quot;;.
        /// </summary>
        public static string merge_tweet_retweet {
            get {
                return ResourceManager.GetString("merge_tweet_retweet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- Merge tweet_url
        ///dbcc traceon (610);
        ///
        ///WITH s AS
        ///(
        ///	SELECT
        ///		run_id, url_id, tweet_id, user_id, created_at, expanded_url,
        ///		ROW_NUMBER() OVER (PARTITION BY run_id, url_id, tweet_id ORDER BY user_id) rn
        ///	FROM [$loaddb].[dbo].[$tablename]
        ///)
        ///MERGE [$targetdb].[dbo].[tweet_url] WITH (TABLOCKX) AS t
        ///USING (SELECT * FROM s WHERE rn = 1) s
        ///	ON s.run_id = t.run_id AND s.url_id = t.url_id AND s.tweet_id = t.tweet_id
        ///WHEN NOT MATCHED THEN
        ///	INSERT (run_id, url_id, tweet_id, user_id, created_at, expande [rest of string was truncated]&quot;;.
        /// </summary>
        public static string merge_tweet_url {
            get {
                return ResourceManager.GetString("merge_tweet_url", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- Merge tweet user mention
        ///dbcc traceon (610);
        ///
        ///WITH s AS
        ///(
        ///	SELECT DISTINCT
        ///		run_id, tweet_id, user_id, mentioned_user_id
        ///	FROM [$loaddb].[dbo].[$tablename]
        ///)
        ///MERGE [$targetdb].[dbo].[tweet_user_mention] WITH (TABLOCKX) AS t
        ///USING s
        ///	ON  s.run_id = t.run_id AND s.tweet_id = t.tweet_id AND s.user_id = t.user_id AND s.mentioned_user_id = t.mentioned_user_id
        ///WHEN NOT MATCHED THEN
        ///	INSERT
        ///	VALUES (s.run_id, s.tweet_id, s.user_id, s.mentioned_user_id);.
        /// </summary>
        public static string merge_tweet_user_mention {
            get {
                return ResourceManager.GetString("merge_tweet_user_mention", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /*
        ///-- Copy users to the final DB
        ///
        ///-- Merge user status updates
        ///
        ///-- This scripts records user personal details updates.
        ///-- Creates at least one entry per chunk so it will somewhat
        ///-- reflect how the counters of a user grow with time, even
        ///-- if no profile settings have been changed.
        ///
        ///dbcc traceon (610);
        ///
        ///-- Create new table for merged user data
        ///
        ///CREATE TABLE [$targetdb].[dbo].[user_new](
        ///	[user_id] [bigint] NOT NULL,
        ///	[created_at] [datetime] NOT NULL,
        ///	[last_update_at] [datetime] NOT NULL,        /// [rest of string was truncated]&quot;;.
        /// </summary>
        public static string merge_user {
            get {
                return ResourceManager.GetString("merge_user", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- Merge hashtags
        ///dbcc traceon (610);
        ///
        ///WITH s AS
        ///(
        ///	SELECT DISTINCT
        ///		run_id, tag, user_id
        ///	FROM [$loaddb].[dbo].[$tablename]
        ///)
        ///MERGE [$targetdb].[dbo].[user_hashtag] WITH (TABLOCKX/*, FORCESEEK*/) AS t
        ///USING s
        ///	ON s.run_id = t.run_id AND s.tag = t.tag AND s.user_id = t.user_id
        ///WHEN NOT MATCHED THEN
        ///	INSERT (run_id, tag, user_id)
        ///	VALUES (s.run_id, s.tag, s.user_id);.
        /// </summary>
        public static string merge_user_hashtag {
            get {
                return ResourceManager.GetString("merge_user_hashtag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- Merge user status updates
        ///
        ///-- This scripts records user personal details updates.
        ///-- Creates at least one entry per chunk so it will somewhat
        ///-- reflect how the counters of a user grow with time, even
        ///-- if no profile settings have been changed.
        ///
        ///dbcc traceon (610);
        ///
        ///WITH s AS
        ///(
        ///	SELECT
        ///		user_id, MAX(tweeted_at) tweeted_at, screen_name, description,
        ///		MAX(favourites_count) favourites_count, MAX(followers_count) followers_count,
        ///		MAX(friends_count) friends_count, MAX(statuses_count) status [rest of string was truncated]&quot;;.
        /// </summary>
        public static string merge_user_update {
            get {
                return ResourceManager.GetString("merge_user_update", resourceCulture);
            }
        }

        public static string merge_pileup
        {
            get
            {
                return ResourceManager.GetString("merge_pileup", resourceCulture);
            }
        }

        public static string merge_basesDist
        {
            get
            {
                return ResourceManager.GetString("merge_basesDist", resourceCulture);
            }
        }

        public static string merge_ref
        {
            get
            {
                return ResourceManager.GetString("merge_ref", resourceCulture);
            }
        }

        public static string merge_sam
        {
            get
            {
                return ResourceManager.GetString("merge_sam", resourceCulture);
            }
        }

        public static string merge_sread
        {
            get
            {
                return ResourceManager.GetString("merge_sread", resourceCulture);
            }
        }
    }
}
