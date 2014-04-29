CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[sampleName] [varchar](50) NOT NULL,
	[chr] [varchar](20) NOT NULL,
	[pos] [bigint] NOT NULL,
	[refNuc] [char] NOT NULL,
	[A] [int] NOT NULL,
	[C] [int] NOT NULL,
	[G] [int] NOT NULL,
	[T] [int] NOT NULL,
	[triplet] [varchar](100)
) ON [BASESDISTLOAD_FG];