SELECT TOP 10 * FROM sys.dm_fts_index_keywords(DB_ID('twitter_cold'), OBJECT_ID('tweet'))
WHERE document_count > 200000
WHERE LEFT(display_term,1) <> '^'

SELECT TOP 10000 * FROM sys.dm_fts_index_keywords_by_document(DB_ID('twitter_cold'), OBJECT_ID('tweet'))
WHERE LEFT(display_term,1) <> '^'

SELECT text FROM tweet WHERE unique_id = 1213054690

SELECT TOP 1 * FROM sys.dm_fts_index_keywords(DB_ID('twitter'), OBJECT_ID('tweet'))
WHERE keyword = CAST(N'帀帀愀氀漀爀猀' AS varbinary(100))

---
SELECT * FROM sys.dm_fts_parser(N'"Muhun ujang patek [̲̅̅h̲̅][̲̅̅a̲̅][̲̅̅h̲̅][̲̅̅a̲̅][̲̅̅a̲̅][̲̅̅h̲̅][̲̅̅a̲̅] RT@yndiMESINTEMPUR: @BobiSetiawan3 mang bobbi"', 1038, 0, 0)
SELECT * FROM sys.dm_fts_parser(N'"AaaA"', 1038, 0, 0)
SELECT * FROM sys.dm_fts_parser(N'"Perras salvajes, AaAaA con cuerpo de pelopincho AaAaA hay pija en tu concha AaAaA pero igual tenes una tanga fluo AaAaA http://t.co/TsE8xTMU"', 1038, 0, 0)
SELECT * FROM sys.dm_fts_parser(N'"to a̶̲̥̅̊"', 1038, 0, 0)
SELECT * FROM sys.dm_fts_parser(N'"Cagmano kabar di asrama bebp *colek* @caa_ocaa @witaanggrainiA asikk dag (ˆˆʃƪ)"', 1038, 0, 0)



---

SELECT * FROM sys.dm_fts_parser(N'"alma"', 1038, 0, 0)


DECLARE @w  nvarchar(100) = CAST(0x006100610061006500000E as nvarchar(100))

SELECT text FROM tweet WHERE CONTAINS(*, 'password')



SELECT CAST(N'[̲̅̅h̲̅][̲̅̅a̲̅][̲̅̅h̲̅][̲̅̅a̲̅][̲̅̅a̲̅][̲̅̅h̲̅][̲̅̅a̲̅]' as varbinary(100))
SELECT CAST(N'to a̶̲̥̅̊' as varbinary(100))

SELECT CAST(0x0061000028 as nvarchar(100))




SELECT TOP 1000 text, lang FROM tweet WHERE lang NOT IN ('en', 'es', 'pt', 'id', 'ja', 'fr', 'tr', 'it', 'ar', 'ru', 'de', 'nl', 'ko', 'th', 'sv', 'no', 'msa', 'zh-tw', 'zh-cn', 'ca', 'fa')
AND run_id = 2004


SELECT TOP 10 * FROM tweet WHERE lang = 'ar' AND run_id = 2004
UNION ALL
SELECT TOP 10 * FROM tweet WHERE lang = 'ja' AND run_id = 2004
UNION ALL
SELECT TOP 10 * FROM tweet WHERE lang = 'th' AND run_id = 2004
UNION ALL
SELECT TOP 10 * FROM tweet WHERE lang = 'ko' AND run_id = 2004
UNION ALL
SELECT TOP 10 * FROM tweet WHERE lang =  'zh-cn' AND run_id = 2004
UNION ALL
SELECT TOP 10 * FROM tweet WHERE lang =  'zh-tw' AND run_id = 2004
UNION ALL
SELECT TOP 10 * FROM tweet WHERE lang =  'he' AND run_id = 2004




SELECT TOP 1000 text, lang FROM tweet WHERE CONTAINS(*, 'drunk')