dbcc traceon (610);

INSERT INTO [$twitterdb].[dbo].[pileups] (run_id, sampleGroup, sampleID, lane, refSeqID, refSeqPos, refNuc, alignedReadsNO, bases, basesQual, extraNuc, startingSigns, mappingQual, endingSigns)
SELECT run_id, sampleGroup, sampleID, lane, refSeqID, refSeqPos, refNuc, alignedReadsNO, bases, basesQual, extraNuc, startingSigns, mappingQual, endingSigns
FROM [$loaddb].[dbo].[$tablename];