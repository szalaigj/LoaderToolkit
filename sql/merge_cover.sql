dbcc traceon (610);

INSERT INTO [$targetdb].[dbo].[sample] ([name])
SELECT DISTINCT sampleName
FROM [$loaddb].[dbo].[$tablename] as cvLd
WHERE NOT EXISTS
(
	SELECT *
	FROM [$targetdb].[dbo].[sample] as s
	WHERE s.name = cvLd.sampleName
);

INSERT INTO [$targetdb].[dbo].[fltrCov] (sampleID, refID, pos, refNuc, coverage, Acount, Ccount, Gcount, Tcount, incount, delcount)
SELECT s.sampleID, rd.refID, cvLd.pos, cvLd.refNuc, cvLd.coverage, cvLd.Acount, cvLd.Ccount, cvLd.Gcount, cvLd.Tcount, cvLd.incount, cvLd.delcount
  FROM [$loaddb].[dbo].[$tablename] as cvLd, [$targetdb].[dbo].[sample] as s, [$targetdb].[dbo].[refDesc] as rd
  WHERE cvLd.sampleName = s.name
  AND cvLd.speciesID = (rd.refID & 0xFFFF0000) / POWER(2, 16)
  AND cvLd.extID = rd.extID