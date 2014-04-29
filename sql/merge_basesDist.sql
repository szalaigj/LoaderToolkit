dbcc traceon (610);

INSERT INTO [$targetdb].[dbo].[sampleBD] ([name])
SELECT DISTINCT sampleName
FROM [$loaddb].[dbo].[$tablename] as basesDistLD
WHERE NOT EXISTS
(
	SELECT *
	FROM [$targetdb].[dbo].[sampleBD] as s
	WHERE s.name = basesDistLD.sampleName
);

INSERT INTO [$targetdb].[dbo].[basesDist] ([sampleID], [chr], [pos], [refNuc], [A], [C], [G], [T], [triplet])
SELECT s.[sampleID], basesDistLD.[chr], basesDistLD.[pos], basesDistLD.[refNuc], basesDistLD.[A], basesDistLD.[C], basesDistLD.[G], basesDistLD.[T], basesDistLD.[triplet]
FROM [$loaddb].[dbo].[$tablename] as basesDistLD, [$targetdb].[dbo].[sampleBD] as s
WHERE basesDistLD.sampleName = s.name;