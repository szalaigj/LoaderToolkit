CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[samID] [int] NOT NULL,
	[qname] [varchar](150) NOT NULL,
	[refID] [int] NOT NULL,
	[dir] [bit] NOT NULL,
	[mapq] [tinyint] NOT NULL,
	[seq] [varchar](8000) NOT NULL,
	[insPos] [varchar](8000) NULL,
	[delPos] [varchar](8000) NULL,
	[posStart] [bigint] NOT NULL,
    [posEnd] [bigint] NOT NULL,
    [qual] [varchar](8000) NOT NULL
) ON [SREADLOAD_FG];