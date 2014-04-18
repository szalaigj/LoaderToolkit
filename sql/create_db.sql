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
	[sampleName] [varchar](16) NOT NULL,
	[sampleGroup] [varchar](8) NULL,
	[sampleID] [int] NULL,
	[lane] [char] NULL,
	[description] [varchar](200) NULL
) ON [PRIMARY]

--
CREATE TABLE [dbo].[reference_sequences](
	[reference_sequence_id] [bigint] IDENTITY(1,1)PRIMARY KEY,
	[refSeqID] [varchar](50) NOT NULL,
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
	[missingNuc] [varchar](8000) NULL,
	[startingSigns] [varchar](8000) NULL,
	[mappingQual] [varchar](8000) NULL,
	[endingSigns] [varchar](8000) NULL,
	CONSTRAINT [PK_tweet_hour] PRIMARY KEY CLUSTERED 
	(
		[run_id] ASC,
		[sample_unit_id] ASC,
		[reference_sequence_id] ASC,
		[refSeqPos] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--
CREATE VIEW [dbo].[pileups_view]
(
		[run_id]
		,[sampleName]
		,[refSeqID]
		,[refSeqPos]
		,[refNuc]
		,[alignedReadsNO]
		,[bases]
		,[basesQual]
		,[extraNuc]
		,[missingNuc]
		,[startingSigns]
		,[mappingQual]
		,[endingSigns]
)
AS
	SELECT [run_id]
		,su.[sampleName]
		,rs.[refSeqID]
		,[refSeqPos]
		,[refNuc]
		,[alignedReadsNO]
		,[dbo].[BasesColumnDecoder]([bases]) as bases
		,[basesQual]
		,[extraNuc]
		,[missingNuc]
		,[startingSigns]
		,[mappingQual]
		,[endingSigns]
	FROM [dbo].[pileups] pu, [dbo].[sample_units] su, [dbo].[reference_sequences] rs
	WHERE pu.[sample_unit_id] = su.[sample_unit_id] AND pu.reference_sequence_id = rs.reference_sequence_id

GO	
	
--
CREATE VIEW [dbo].[pileups_base_counters_view]
(
		[run_id]
		,[sampleName]
		,[refSeqID]
		,[refSeqPos]
		,[refNuc]
		,[coverage]
		,[A]
		,[C]
		,[G]
		,[T]
)
AS
SELECT [run_id]
      ,[sampleName]
      ,[refSeqID]
      ,[refSeqPos]
      ,[refNuc]
      ,[alignedReadsNO] as coverage
	  ,[cbs].[A]
	  ,[cbs].[C]
	  ,[cbs].[G]
	  ,[cbs].[T]
  FROM [dbo].[pileups_view]
  CROSS APPLY [dbo].[CountBasesSeparately]([bases], [refNuc]) as [cbs]
  
GO