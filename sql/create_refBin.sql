CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[refID] [int] NOT NULL,
	[posStart] [bigint] NOT NULL,
	[seqBlock] [binary](256) NOT NULL
) ON [REFLOAD_BIN_FG];