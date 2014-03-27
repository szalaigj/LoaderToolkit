-- sp_dboption [$dbname], 'SELECT INTO/BULKCOPY', TRUE

BULK INSERT [$dbname].[dbo].[$tablename]
FROM '$filename'
WITH (
	CODEPAGE = 1200,				--	 unicode
	DATAFILETYPE = N'widechar',
	FIELDTERMINATOR = N'\0',
	ROWTERMINATOR = N'\0\r',
	KEEPNULLS,
	--BATCHSIZE = 1000000,		Avoid this option to prevent logging
	TABLOCK
	)