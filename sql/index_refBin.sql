CREATE CLUSTERED INDEX [IX_$tablename] ON [$dbname].[dbo].[$tablename] 
(
    [refID] ASC,
	[posStart] ASC
) ON [PRIMARY];