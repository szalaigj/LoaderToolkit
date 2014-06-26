CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[seqname] [varchar](150) NOT NULL,
	[source] [varchar](150) NOT NULL,
	[feature] [varchar](150) NOT NULL,
	[start] [bigint] NOT NULL,
	[end] [bigint] NOT NULL,
	[score] [varchar](50) NOT NULL,
	[strand] [char] NOT NULL,
	[frame] [tinyint] NOT NULL,
	[attribute] [varchar](8000) NOT NULL
) ON [PRIMARY]