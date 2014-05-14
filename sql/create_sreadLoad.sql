CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[samID] [int] NOT NULL,
	[extID] [varchar](150) NOT NULL,
	[rname] [varchar](50) NOT NULL,
	[dir] [bit] NOT NULL,
	[mapq] [tinyint] NOT NULL,
	[cigar] [varchar](100) NULL,
	[seq] [varchar](1000) NULL,
	[posStart] [bigint] NOT NULL,
    [posEnd] [bigint] NOT NULL,
    [qual] [varchar](50) NOT NULL
) ON [SREADLOAD_FG];