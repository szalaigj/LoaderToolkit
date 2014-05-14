CREATE TABLE [$dbname].[dbo].[$tablename]
(
	[samID] [int] NOT NULL PRIMARY KEY, -- sam file identifier
	[line] [int] NOT NULL, -- 1-based row number of the header line in the file
	[type] [varchar](2) NOT NULL,
	[tags] [varchar](8000) NOT NULL
) ON [PRIMARY];