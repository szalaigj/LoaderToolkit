CREATE CLUSTERED INDEX [IX_$tablename] ON [$dbname].[dbo].[$tablename] 
(
    [samID] ASC,
    [posStart] ASC
) ON [PRIMARY];