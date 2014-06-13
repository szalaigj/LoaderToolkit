CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[samID] [int] NOT NULL,
	[qname] [varchar](150) NOT NULL,
	[rname] [varchar](50) NOT NULL,
	[dir] [bit] NOT NULL,
	[mapq] [tinyint] NOT NULL,
	[seq] [varbinary](512) NOT NULL,
	[insPos] [varchar](8000) NULL,
	[delPos] [varchar](8000) NULL,
	[posStart] [bigint] NOT NULL,
    [posEnd] [bigint] NOT NULL,
    [qual] [varchar](8000) NOT NULL
) ON [PRIMARY];