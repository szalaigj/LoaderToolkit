CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[sampleName] [varchar](16) NOT NULL,
	[speciesID] [int] NOT NULL,
	[extID] [varchar](80) NOT NULL,
	[pos] [bigint] NOT NULL,
	[refNuc] [char] NOT NULL,
	[coverage] [int] NOT NULL,
	[Acount] [int] NOT NULL,
	[Ccount] [int] NOT NULL,
	[Gcount] [int] NOT NULL,
	[Tcount] [int] NOT NULL,
	[incount] [int] NOT NULL,
	[delcount] [int] NOT NULL
) ON [COVERLOAD_FG];