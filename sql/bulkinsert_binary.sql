-- sp_dboption [$dbname], 'SELECT INTO/BULKCOPY', TRUE

BULK INSERT [$dbname].[dbo].[$tablename]
FROM '$filename'
WITH (
	DATAFILETYPE = N'widenative',
	TABLOCK
	)