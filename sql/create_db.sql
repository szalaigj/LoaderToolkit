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
CREATE TABLE [dbo].[sample_units](
	[sample_unit_id] [bigint] IDENTITY(1,1)PRIMARY KEY,
	[sampleGroup] [varchar](8) NOT NULL,
	[sampleID] [int] NOT NULL,
	[lane] [char] NOT NULL,
	[description] [varchar](200) NULL
) ON [PRIMARY]

--
CREATE TABLE [dbo].[reference_sequences](
	[reference_sequence_id] [bigint] IDENTITY(1,1)PRIMARY KEY,
	[refSeqID] [varchar](50) NULL,
	[gi] [bigint] NULL,
	[accessionNO] [varchar](50) NULL,
	[shortName] [varchar](100) NULL
) ON [PRIMARY]

--
CREATE TABLE [dbo].[pileups](
	[run_id] [smallint] NOT NULL,
	[sample_unit_id] [bigint] NOT NULL,
	[reference_sequence_id] [bigint] NOT NULL,
	[refSeqPos] [bigint] NOT NULL,
	[refNuc] [char] NOT NULL,
	[alignedReadsNO] [int] NOT NULL,
	[bases] [varbinary](8000) NOT NULL,
	[basesQual] [varchar](8000) NULL,
	[extraNuc] [varchar](8000) NULL,
	[startingSigns] [varchar](8000) NULL,
	[mappingQual] [varchar](8000) NULL,
	[endingSigns] [varchar](8000) NULL
) ON [PRIMARY]

GO