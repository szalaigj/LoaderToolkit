CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[sampleName] [varchar](16) NOT NULL,
	[speciesID] [int] NOT NULL,
	[extID] [varchar](80) NOT NULL,
	[pos] [bigint] NOT NULL,
	[refNuc] [char] NOT NULL,
	[coverage] [int] NOT NULL,
	[bases] [varbinary](8000) NOT NULL,
	[basesQual] [varchar](8000) NULL
) ON [FDPLOAD_FG];