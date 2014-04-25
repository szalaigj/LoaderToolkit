dbcc traceon (610);

INSERT INTO [$targetdb].[dbo].[sample] ([name], [group], [extID], [lane])
SELECT DISTINCT sampleName, sampleGroup, sampleExtID, lane
FROM [$loaddb].[dbo].[$tablename] as pupLd
WHERE NOT EXISTS
(
	SELECT *
	FROM [$targetdb].[dbo].[sample] as s
	WHERE s.name = pupLd.sampleName
);

INSERT INTO [$targetdb].[dbo].[reference] (extID, gi, accNO)
SELECT refSeqID
	,[$targetdb].[dbo].PickOutAPartOfColumnBySeparator(refSeqID, 1, N'\|') as gi
	,[$targetdb].[dbo].PickOutAPartOfColumnBySeparator(refSeqID, 3, N'\|') as accessionNO
FROM (SELECT DISTINCT refSeqID
               FROM [$loaddb].[dbo].[$tablename]) as refSeqIDs
WHERE NOT EXISTS
(
	SELECT *
	FROM [$targetdb].[dbo].[reference] as r
	WHERE r.extID = refSeqIDs.refSeqID
);

INSERT INTO [$targetdb].[dbo].[pileup] (sampleID, refID)
SELECT s.sampleID, r.refID
FROM (SELECT DISTINCT sampleName, refSeqID FROM [$loaddb].[dbo].[$tablename]) as pupLd, 
[$targetdb].[dbo].[sample] as s, 
[$targetdb].[dbo].[reference] as r
WHERE pupLd.sampleName = s.name
AND pupLd.refSeqID = r.extID;


INSERT INTO [$targetdb].[dbo].[coverageEnc] (pupID, pos, refNuc, coverage, bases, basesQual, exNuc, missNuc, startSg, mapQual, endSg)
SELECT pup.pupID, pupLd.refSeqPos, pupLd.refNuc, pupLd.coverage, pupLd.bases, pupLd.basesQual, pupLd.exNuc, pupLd.missNuc, pupLd.startSg, pupLd.mapQual, pupLd.endSg
  FROM [$loaddb].[dbo].[$tablename] as pupLd, [$targetdb].[dbo].[sample] as s, [$targetdb].[dbo].[reference] as r, [$targetdb].[dbo].[pileup] as pup
  WHERE pupLd.sampleName = s.name
  AND pupLd.refSeqID = r.extID
  AND pup.sampleID = s.sampleID
  AND pup.refID = r.refID;