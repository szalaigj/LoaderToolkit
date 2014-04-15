dbcc traceon (610);

INSERT INTO [$twitterdb].[dbo].[sample_units] (sampleGroup, sampleID, lane)
SELECT DISTINCT sampleGroup, sampleID, lane
FROM [$loaddb].[dbo].[$tablename];

INSERT INTO [$twitterdb].[dbo].[reference_sequences] (refSeqID, gi, accessionNO)
SELECT refSeqID
	,[$twitterdb].[dbo].PickOutAPartOfColumnBySeparator(refSeqID, 1, N'\|') AS gi
	,[$twitterdb].[dbo].PickOutAPartOfColumnBySeparator(refSeqID, 3, N'\|') AS accessionNO
FROM (SELECT DISTINCT refSeqID
               FROM [$loaddb].[dbo].[$tablename]) AS refSeqIDs;

INSERT INTO [$twitterdb].[dbo].[pileups] (run_id, sample_unit_id, reference_sequence_id, refSeqPos, refNuc, alignedReadsNO, bases, basesQual, extraNuc, startingSigns, mappingQual, endingSigns)
SELECT pu.run_id, su.sample_unit_id, rs.reference_sequence_id, pu.refSeqPos, pu.refNuc, pu.alignedReadsNO, pu.bases, pu.basesQual, pu.extraNuc, pu.startingSigns, pu.mappingQual, pu.endingSigns
  FROM [$loaddb].[dbo].[$tablename] as pu, [$twitterdb].[dbo].[sample_units] as su, [$twitterdb].[dbo].[reference_sequences] as rs 
  WHERE pu.sampleGroup = su.sampleGroup
  AND pu.sampleID = su.sampleID
  AND pu.lane = su.lane
  AND pu.refSeqID = rs.refSeqID;