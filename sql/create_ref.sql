CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[refID] [int] NOT NULL,
	[pos] [bigint] NOT NULL,
	[refNuc] [char] NULL
) ON [REFLOAD_FG];