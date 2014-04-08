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
CREATE TABLE [dbo].[pileups](
	[run_id] [smallint] NOT NULL,
	[sampleGroup] [varchar](8) NOT NULL,
	[sampleID] [int] NOT NULL,
	[lane] [char] NOT NULL,
	[refSeqID] [varchar](50) NOT NULL,
	[refSeqPos] [bigint] NOT NULL,
	[refNuc] [binary](1) NOT NULL,
	[alignedReadsNO] [int] NOT NULL,
	[bases] [varbinary](8000) NOT NULL,
	[basesQual] [varbinary](8000) NULL,
	[extraNuc] [varchar](8000) NULL,
	[startingSigns] [varchar](8000) NULL,
	[mappingQual] [varchar](8000) NULL,
	[endingSigns] [varchar](8000) NULL
) ON [PRIMARY]

GO