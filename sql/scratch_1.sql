SELECT TOP 100 * FROM tweet





DBCC sqlperf(logspace)

DBCC CheckAlloc(np_coadd)


SELECT TOP 100 * FROM user_location
WHERE user_id = 4104111
ORDER BY created_at


SELECT * FROM tweet
WHERE user_id = 4104111
ORDER BY created_at


SELECT * FROM [user]
WHERE id = 4104111





BULK INSERT [TwitterLoader].[dbo].[Sample_120412_093259_user]
FROM 'C:\Data0\dobos\project\twitter\bulk\Sample_120412_093259\user.txt'
WITH (
	CODEPAGE = 1200,				--	 unicode
	DATAFILETYPE = N'widechar',
	FIELDTERMINATOR = N'\0',
	ROWTERMINATOR = N'\0\r',
	KEEPNULLS,
	TABLOCK
	)
	
	
EXEC sp_fulltext_database 'enable'
	
SELECT TOP 100 * FROM tweet WHERE CONTAINS(text, 'sleep')