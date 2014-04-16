CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[run_id] [smallint] NOT NULL,
	[sampleName] [varchar](16) NOT NULL,
	[sampleGroup] [varchar](8) NULL,
	[sampleID] [int] NULL,
	[lane] [char] NULL,
	[refSeqID] [varchar](50) NOT NULL,
	[refSeqPos] [bigint] NOT NULL,
	[refNuc] [char] NOT NULL,
	[alignedReadsNO] [int] NOT NULL,
	[bases] [varbinary](8000) NOT NULL,
	[basesQual] [varchar](8000) NULL,
	[extraNuc] [varchar](8000) NULL,
	[missingNuc] [varchar](8000) NULL,
	[startingSigns] [varchar](8000) NULL,
	[mappingQual] [varchar](8000) NULL,
	[endingSigns] [varchar](8000) NULL
) ON [PRIMARY];