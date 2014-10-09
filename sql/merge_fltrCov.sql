dbcc traceon (610);

INSERT INTO [$targetdb].[dbo].[sample] ([name])
SELECT DISTINCT sampleName
FROM [$loaddb].[dbo].[$tablename] as fdpLd
WHERE NOT EXISTS
(
	SELECT *
	FROM [$targetdb].[dbo].[sample] as s
	WHERE s.name = fdpLd.sampleName
);

INSERT INTO [$targetdb].[dbo].[pileup] (sampleID, refID)
SELECT s.sampleID, rd.refID
FROM (SELECT DISTINCT sampleName, speciesID, extID FROM [$loaddb].[dbo].[$tablename]) as fdpLd, 
[$targetdb].[dbo].[sample] as s, 
[$targetdb].[dbo].[refDesc] as rd
WHERE fdpLd.sampleName = s.name
AND fdpLd.speciesID = (rd.refID & 0xFFFF0000) / POWER(2, 16)
AND fdpLd.extID = rd.extID;

INSERT INTO [$targetdb].[dbo].[fltrCov] (pupID, pos, refNuc, coverage, bases, basesQual)
SELECT pup.pupID, fdpLd.pos, fdpLd.refNuc, fdpLd.coverage, fdpLd.bases, fdpLd.basesQual
  FROM [$loaddb].[dbo].[$tablename] as fdpLd, [$targetdb].[dbo].[sample] as s, [$targetdb].[dbo].[refDesc] as rd, [$targetdb].[dbo].[pileup] as pup
  WHERE fdpLd.sampleName = s.name
  AND fdpLd.extID = rd.extID
  AND pup.sampleID = s.sampleID
  AND pup.refID = rd.refID;