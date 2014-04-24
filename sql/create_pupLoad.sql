CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[sampleName] [varchar](16) NOT NULL,
	[sampleGroup] [varchar](8) NULL,
	[sampleExtID] [int] NULL,
	[lane] [char] NULL,
	[refSeqID] [varchar](50) NOT NULL,
	[refSeqPos] [bigint] NOT NULL,
	[refNuc] [char] NOT NULL,
	[coverage] [int] NOT NULL,
	[bases] [varbinary](8000) NOT NULL,
	[basesQual] [varchar](8000) NULL,
	[exNuc] [varchar](8000) NULL,
	[missNuc] [varchar](8000) NULL,
	[startSg] [varchar](8000) NULL,
	[mapQual] [varchar](8000) NULL,
	[endSg] [varchar](8000) NULL
) ON [PUPLOAD_FG];