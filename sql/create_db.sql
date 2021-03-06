USE szalaigj
GO

-- For pileups

ALTER DATABASE szalaigj
ADD FILEGROUP COVERAGE_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = coverage_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\coverage_0.ndf',
	SIZE = 20GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = coverage_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\coverage_1.ndf',
	SIZE = 20GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP COVERAGE_FG;
GO

ALTER DATABASE szalaigj
ADD FILEGROUP PUPLOAD_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = pupload_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\pupload_0.ndf',
	SIZE = 20GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = pupload_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\pupload_1.ndf',
	SIZE = 20GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP PUPLOAD_FG;
GO

-- For basesDist

ALTER DATABASE szalaigj
ADD FILEGROUP BASESDIST_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = basesdist_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\basesdist_0.ndf',
	SIZE = 20GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = basesdist_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\basesdist_1.ndf',
	SIZE = 20GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP BASESDIST_FG;
GO

ALTER DATABASE szalaigj
ADD FILEGROUP BASESDISTLOAD_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = basesdistload_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\basesdistload_0.ndf',
	SIZE = 20GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = basesdistload_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\basesdistload_1.ndf',
	SIZE = 20GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP BASESDISTLOAD_FG;
GO

-- For SAM-style

ALTER DATABASE szalaigj
ADD FILEGROUP REF_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = ref_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\ref_0.ndf',
	SIZE = 80GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = ref_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\ref_1.ndf',
	SIZE = 80GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP REF_FG;
GO

ALTER DATABASE szalaigj
ADD FILEGROUP REFLOAD_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = refload_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\refload_0.ndf',
	SIZE = 40GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = refload_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\refload_1.ndf',
	SIZE = 40GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP REFLOAD_FG;
GO

ALTER DATABASE szalaigj
ADD FILEGROUP SREAD_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = sread_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\sread_0.ndf',
	SIZE = 80GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = sread_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\sread_1.ndf',
	SIZE = 80GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP SREAD_FG;
GO

ALTER DATABASE szalaigj
ADD FILEGROUP SREADLOAD_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = sreadload_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\sreadload_0.ndf',
	SIZE = 40GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = sreadload_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\sreadload_1.ndf',
	SIZE = 40GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP SREADLOAD_FG;
GO

-- Expand the previous file groups:
ALTER DATABASE szalaigj
ADD FILE
(
	NAME = sread_2,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\sread_2.ndf',
	SIZE = 300GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = sread_3,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\sread_3.ndf',
	SIZE = 300GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP SREAD_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = sreadload_2,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\sreadload_2.ndf',
	SIZE = 300GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = sreadload_3,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\sreadload_3.ndf',
	SIZE = 300GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP SREADLOAD_FG;
GO

-- For SAM-style with binary encoding

ALTER DATABASE szalaigj
ADD FILEGROUP REF_BIN_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = ref_bin_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\ref_bin_0.ndf',
	SIZE = 80GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = ref_bin_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\ref_bin_1.ndf',
	SIZE = 80GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP REF_BIN_FG;
GO

ALTER DATABASE szalaigj
ADD FILEGROUP REFLOAD_BIN_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = refload_bin_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\refload_bin_0.ndf',
	SIZE = 40GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = refload_bin_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\refload_bin_1.ndf',
	SIZE = 40GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP REFLOAD_BIN_FG;
GO

ALTER DATABASE szalaigj
ADD FILEGROUP SREAD_BIN_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = sread_bin_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\sread_bin_0.ndf',
	SIZE = 300GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = sread_bin_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\sread_bin_1.ndf',
	SIZE = 300GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP SREAD_BIN_FG;
GO

ALTER DATABASE szalaigj
ADD FILEGROUP SREADLOAD_BIN_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = sreadload_bin_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\sreadload_bin_0.ndf',
	SIZE = 300GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = sreadload_bin_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\sreadload_bin_1.ndf',
	SIZE = 300GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP SREADLOAD_BIN_FG;
GO

ALTER DATABASE szalaigj
ADD FILEGROUP ALIGN_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = align_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\align_0.ndf',
	SIZE = 200GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = align_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\align_1.ndf',
	SIZE = 200GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP ALIGN_FG;
GO

ALTER DATABASE szalaigj
ADD FILEGROUP BCOVERBIN_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = bcoverbin_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\bcoverbin_0.ndf',
	SIZE = 200GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = bcoverbin_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\bcoverbin_1.ndf',
	SIZE = 200GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP BCOVERBIN_FG;
GO

-- For filtered pileups:
ALTER DATABASE szalaigj
ADD FILEGROUP FDPLOAD_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = fdpload_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\fdpload_0.ndf',
	SIZE = 20GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = fdpload_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\fdpload_1.ndf',
	SIZE = 20GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP FDPLOAD_FG;
GO

ALTER DATABASE szalaigj
ADD FILEGROUP FLTR_COV_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = fltrcov0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\fltrcov0.ndf',
	SIZE = 200GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = fltrcov1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\fltrcov1.ndf',
	SIZE = 200GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP FLTR_COV_FG;
GO

-- For checking:
USE szalaigj
GO
sp_helpfile
GO

SELECT groupName AS FileGroupName FROM sysfilegroups
GO

-- Check which filegroup is used by table coverageEnc:
--sp_help [coverageEnc]
--GO

-- For pileups:
CREATE TABLE [dbo].[sample](
	[sampleID] [int] IDENTITY(1,1)PRIMARY KEY,
	[name] [varchar](16) NOT NULL,
	[group] [varchar](8) NULL,
	[extID] [int] NULL,
	[lane] [char] NULL,
	[desc] [varchar](200) NULL
) ON [PRIMARY]

--
CREATE TABLE [dbo].[reference](
	[refID] [int] IDENTITY(1,1)PRIMARY KEY,
	[extID] [varchar](50) NOT NULL,
	[gi] [bigint] NULL,
	[accNO] [varchar](50) NULL,
	[name] [varchar](100) NULL
) ON [PRIMARY]

--
CREATE TABLE [dbo].[pileup](
	[pupID] [int] IDENTITY(1,1)PRIMARY KEY,
	[sampleID] [int] NOT NULL,
	[refID] [int] NOT NULL
) ON [PRIMARY]

--
CREATE TABLE [dbo].[coverageEnc](
	[pupID] [int] NOT NULL,
	[pos] [bigint] NOT NULL,
	[refNuc] [char] NOT NULL,
	[coverage] [int] NOT NULL,
	[bases] [varbinary](8000) NOT NULL,
	[basesQual] [varchar](8000) NULL,
	[exNuc] [varchar](8000) NULL,
	[missNuc] [varchar](8000) NULL,
	[startSg] [varchar](8000) NULL,
	[mapQual] [varchar](8000) NULL,
	[endSg] [varchar](8000) NULL,
	CONSTRAINT [PK_coverage_enc] PRIMARY KEY CLUSTERED 
	(
		[pupID] ASC,
		[pos] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, DATA_COMPRESSION = PAGE) ON [COVERAGE_FG]
) ON [COVERAGE_FG]

GO

--
CREATE VIEW [dbo].[coverage]
(
		[pupID]
		,[pos]
		,[refNuc]
		,[coverage]
		,[bases]
		,[basesQual]
		,[exNuc]
		,[missNuc]
		,[startSg]
		,[mapQual]
		,[endSg]
)
AS
	SELECT [pupID]
		,[pos]
		,[refNuc]
		,[coverage]
		,[dbo].[BasesColumnDecoder]([bases]) as bases
		,[basesQual]
		,[exNuc]
		,[missNuc]
		,[startSg]
		,[mapQual]
		,[endSg]
	FROM [dbo].[coverageEnc]

GO	
	
--
CREATE VIEW [dbo].[basesCover]
(
		[pupID]
		,[pos]
		,[refNuc]
		,[coverage]
		,[A]
		,[C]
		,[G]
		,[T]
		,[triplet]
)
AS
SELECT [pupID]
      ,[pos]
      ,[refNuc]
      ,[coverage]
	  ,[A]
	  ,[C]
	  ,[G]
	  ,[T]
	  ,CASE
		WHEN lagPos = pos - 1 AND leadPos = pos + 1 THEN lagValue + [refNuc] + leadValue
		WHEN lagPos = pos - 1 AND (leadPos IS NULL OR leadPos != pos + 1) THEN lagValue + [refNuc] + 'x'
		WHEN (lagPos IS NULL OR lagPos != pos - 1) AND leadPos = pos + 1 THEN 'x' + [refNuc] + leadValue
		ELSE 'x' + [refNuc] + 'x'
	   END triplet
FROM
(SELECT [pupID]
      ,[pos]
      ,[refNuc]
      ,[coverage]
	  ,[cbs].[A]
	  ,[cbs].[C]
	  ,[cbs].[G]
	  ,[cbs].[T]
	  ,LAG([pos],1) OVER (ORDER BY [pupID], [pos]) as lagPos
	  ,LAG([refNuc],1) OVER (ORDER BY [pupID], [pos]) as lagValue
	  ,LEAD([pos],1) OVER (ORDER BY [pupID], [pos]) as leadPos
	  ,LEAD([refNuc],1) OVER (ORDER BY [pupID], [pos]) as leadValue
  FROM [dbo].[coverage]
  CROSS APPLY [dbo].[CountBasesSeparately]([bases], [refNuc]) as [cbs]
  ) as counters
  
GO

--
CREATE VIEW [dbo].[inDel]
(
		[pupID]
		,[pos]
		,[coverage]
		,[inDel]
		,[chainLen]
		,[nucChain]
)
AS
SELECT [pupID]
      ,[pos]
      ,[coverage]
	  ,[did].[inDel]
	  ,[did].[chainLen]
	  ,[did].[nucChain]
FROM
(SELECT [pupID]
      ,[pos]
      ,[coverage]
	  ,[exNuc]
	  ,[missNuc]
  FROM [dbo].[coverage]
  WHERE [exNuc] IS NOT NULL OR [missNuc] IS NOT NULL) as [innerTbl] 
  CROSS APPLY [dbo].[DetermineInDel]([exNuc], [missNuc]) as [did]
  
GO

-- For basesDist:
CREATE TABLE [dbo].[sampleBD](
	[sampleID] [int] IDENTITY(1,1)PRIMARY KEY,
	[name] [varchar](50) NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[basesDist](
	[sampleID] [int] NOT NULL,
	[chr] [varchar](20) NOT NULL,
	[pos] [bigint] NOT NULL,
	[refNuc] [char] NOT NULL,
	[A] [int] NOT NULL,
	[C] [int] NOT NULL,
	[G] [int] NOT NULL,
	[T] [int] NOT NULL,
	[triplet] [varchar](100),
	CONSTRAINT [PK_basesDist] PRIMARY KEY CLUSTERED 
	(
		[sampleID] ASC,
		[chr] ASC,
		[pos] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, DATA_COMPRESSION = PAGE) ON [BASESDIST_FG]
) ON [BASESDIST_FG]

-- For SAM-style:
CREATE TABLE [dbo].[refDesc](
	[refID] [int] NOT NULL PRIMARY KEY,-- IDENTITY(1,1)PRIMARY KEY,
	[extID] [varchar](80) NOT NULL,
	[desc] [varchar](200) NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[ref](
	[refID] [int] NOT NULL,
	[pos] [bigint] NOT NULL,
	[refNuc] [char] NULL,
	CONSTRAINT [PK_ref] PRIMARY KEY CLUSTERED 
	(
		[refID] ASC,
		[pos] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, DATA_COMPRESSION = PAGE) ON [REF_FG]
) ON [REF_FG]

-- For the storage of headers of sam files:
CREATE TABLE [dbo].[sam](
	[samID] [int] NOT NULL, -- sam file identifier
	[line] [int] NOT NULL, -- 1-based row number of the header line in the file
	[type] [varchar](2) NOT NULL,
	[tags] [varchar](8000) NOT NULL,
	CONSTRAINT [PK_sam] PRIMARY KEY CLUSTERED 
	(
		[samID] ASC,
		[line] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, DATA_COMPRESSION = PAGE) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE sread
(
	[samID] [int] NOT NULL,
	[sreadID] [int] NOT NULL IDENTITY(1,1),
	[refID] [int] NOT NULL,
	[qname] [varchar](150) NOT NULL,
	[dir] [bit] NOT NULL, -- the 0 means the normal and 1 means the reversed direction
	[mapq] [tinyint] NOT NULL,
	[posStart] [bigint] NOT NULL,
    [posEnd] [bigint] NOT NULL,
    [misMNuc] [varchar](8000) NULL, -- bases which did not match the reference with offset
	[indel] [varchar](8000) NULL, -- sequences of insertions/deletions with offset
    [qual] [varchar](8000) NOT NULL,
	CONSTRAINT [PK_sread] PRIMARY KEY CLUSTERED 
	(
		[samID] ASC,
		[sreadID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, DATA_COMPRESSION = PAGE) ON [SREAD_FG]
) ON [SREAD_FG]

CREATE VIEW [dbo].[basesCover]
(
         [samID]
		,[refID]
		,[pos]
		,[refNuc]
		,[coverage]
		,[A]
		,[C]
		,[G]
		,[T]
		,[triplet]
)
AS
SELECT [samID]
      ,[refID]
      ,[pos]
      ,[refNuc]
      ,[coverage]
	  ,[A]
	  ,[C]
	  ,[G]
	  ,[T]
	  ,CASE
		WHEN lagPos = pos - 1 AND leadPos = pos + 1 THEN lagValue + [refNuc] + leadValue
		WHEN lagPos = pos - 1 AND (leadPos IS NULL OR leadPos != pos + 1) THEN lagValue + [refNuc] + 'x'
		WHEN (lagPos IS NULL OR lagPos != pos - 1) AND leadPos = pos + 1 THEN 'x' + [refNuc] + leadValue
		ELSE 'x' + [refNuc] + 'x'
	   END triplet
FROM
(SELECT s.samID
,r.refID
,r.pos
,r.refNuc
,COUNT(*) as coverage
,SUM([dbo].[IsNucX](s.posStart, s.misMNuc, r.pos, r.refNuc, 'A')) as A
,SUM([dbo].[IsNucX](s.posStart, s.misMNuc, r.pos, r.refNuc, 'C')) as C
,SUM([dbo].[IsNucX](s.posStart, s.misMNuc, r.pos, r.refNuc, 'G')) as G
,SUM([dbo].[IsNucX](s.posStart, s.misMNuc, r.pos, r.refNuc, 'T')) as T
,LAG(r.pos,1) OVER (ORDER BY s.samID, r.refID, r.pos) as lagPos
,LAG(r.refNuc,1) OVER (ORDER BY s.samID, r.refID, r.pos) as lagValue
,LEAD(r.pos,1) OVER (ORDER BY s.samID, r.refID, r.pos) as leadPos
,LEAD(r.refNuc,1) OVER (ORDER BY s.samID, r.refID, r.pos) as leadValue
FROM dbo.ref r
INNER JOIN [dbo].sread s ON r.refID = s.refID AND (r.pos BETWEEN s.posStart AND s.posEnd)
AND [dbo].[IsDel](s.posStart, s.indel, r.pos) = 0
GROUP BY s.samID, r.refID, r.pos, r.refNuc) as counters

GO

CREATE VIEW [dbo].[inDel]
(
         [samID]
		,[refID]
		,[sreadID]
		,[inDelStartPos]
		,[inDel]
		,[chainLen]
		,[nucChain]
		,[coverage]
)
AS
WITH
idq
AS
(SELECT [innerTbl].[samID]
       ,[innerTbl].[refID]
       ,[innerTbl].[sreadID]
       ,[did].[inDelStartPos]
	   ,[did].[inDel]
	   ,[did].[chainLen]
	   ,[did].[nucChain]
FROM
(SELECT s.samID
       ,s.refID
       ,s.sreadID
       ,s.indel
       ,s.posStart
FROM [dbo].sread s) as [innerTbl]
CROSS APPLY [dbo].[DetermineInDel]([posStart], [indel]) as [did]),
cov
AS
(SELECT s.samID
       ,r.refID
       ,r.pos
	   ,COUNT(*) as coverage
 FROM [dbo].[ref] r
 INNER JOIN [dbo].sread s ON r.refID = s.refID AND (r.pos BETWEEN s.posStart AND s.posEnd)
 AND [dbo].[IsDel](s.posStart, s.indel, r.pos) = 0
 GROUP BY s.samID, r.refID, r.pos)
SELECT i.*
	  ,c.coverage
FROM idq i
INNER JOIN cov c ON i.samID = c.samID AND i.refID = c.refID AND i.inDelStartPos = c.pos

GO

-- For SAM-style with binary encoded reference genomes:
CREATE TABLE [dbo].[refDesc](-- this is unchanged
	[refID] [int] NOT NULL PRIMARY KEY,-- IDENTITY(1,1)PRIMARY KEY,
	[extID] [varchar](80) NOT NULL,
	[desc] [varchar](200) NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[refBin](
	[refID] [int] NOT NULL,
	[posStart] [bigint] NOT NULL,
	[seqBlock] [binary](256) NOT NULL,
	CONSTRAINT [PK_refBin] PRIMARY KEY CLUSTERED 
	(
		[refID] ASC,
		[posStart] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, DATA_COMPRESSION = PAGE) ON [REF_BIN_FG]
) ON [REF_BIN_FG]

GO

-- For the storage of headers of sam files:
CREATE TABLE [dbo].[sam](-- this is unchanged
	[samID] [int] NOT NULL, -- sam file identifier
	[line] [int] NOT NULL, -- 1-based row number of the header line in the file
	[type] [varchar](2) NOT NULL,
	[tags] [varchar](8000) NOT NULL,
	CONSTRAINT [PK_sam] PRIMARY KEY CLUSTERED 
	(
		[samID] ASC,
		[line] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, DATA_COMPRESSION = PAGE) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE sreadBin
(-- this is unchanged
	[samID] [int] NOT NULL,
	[sreadID] [int] NOT NULL IDENTITY(1,1),
	[refID] [int] NOT NULL,
	[qname] [varchar](150) NOT NULL,
	[dir] [bit] NOT NULL, -- the 0 means the normal and 1 means the reversed direction
	[mapq] [tinyint] NOT NULL,
	[posStart] [bigint] NOT NULL,
    [posEnd] [bigint] NOT NULL,
    [misMNuc] [varchar](8000) NULL, -- bases which did not match the reference with offset
	[indel] [varchar](8000) NULL, -- sequences of insertions/deletions with offset
    [qual] [varchar](8000) NOT NULL,
	CONSTRAINT [PK_sreadBin] PRIMARY KEY CLUSTERED 
	(
		[samID] ASC,
		[sreadID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, DATA_COMPRESSION = PAGE) ON [SREAD_BIN_FG]
) ON [SREAD_BIN_FG]

GO

CREATE VIEW [dbo].[align]
(
       samID
      ,refID
	  ,sreadPosStart
	  ,sreadPosEnd
	  ,misMNuc
	  ,indel
	  ,refPosStart
	  ,refSeq
)
AS
SELECT sb.samID
      ,sb.refID
	  ,sb.posStart as sreadPosStart
	  ,sb.posEnd as sreadPosEnd
	  ,sb.misMNuc
	  ,sb.indel
	  ,rb.posStart as refPosStart
	  ,rb.seqBlock as refSeq
FROM [dbo].sreadBin sb
INNER JOIN [dbo].refBin rb ON sb.refID = rb.refID AND FLOOR((sb.posStart - 1) / 256) * 256 + 1 = rb.posStart

GO

CREATE VIEW [dbo].[basesCoverBin]
(
         [samID]
		,[refID]
		,[pos]
		,[refNuc]
		,[coverage]
		,[A]
		,[C]
		,[G]
		,[T]
		,[triplet]
)
AS
SELECT a.samID
      ,a.refID
	  ,dn.refPos as pos
	  ,dn.refNuc
	  ,SUM(dn.coverage) as coverage
	  ,SUM(dn.A) as A
	  ,SUM(dn.C) as C
	  ,SUM(dn.G) as G
	  ,SUM(dn.T) as T
	  ,dn.triplet
FROM [dbo].[align] a
CROSS APPLY [dbo].[DetNucDistr](a.refPosStart, a.refSeq, a.sreadPosStart, a.sreadPosEnd, a.misMNuc, a.indel) dn
GROUP BY a.samID, a.refID, dn.refPos, dn.refNuc, dn.triplet

GO

CREATE VIEW [dbo].[inDelBin]
(
         [samID]
		,[refID]
		,[sreadID]
		,[inDelStartPos]
		,[inDel]
		,[chainLen]
		,[nucChain]
		,[coverage]
)
AS
WITH
idq
AS
(SELECT [innerTbl].[samID]
       ,[innerTbl].[refID]
       ,[innerTbl].[sreadID]
       ,[did].[inDelStartPos]
	   ,[did].[inDel]
	   ,[did].[chainLen]
	   ,[did].[nucChain]
FROM
(SELECT sb.samID
       ,sb.refID
       ,sb.sreadID
       ,sb.indel
       ,sb.posStart
FROM [dbo].sreadBin sb) as [innerTbl]
CROSS APPLY [dbo].[DetermineInDel]([posStart], [indel]) as [did]),
cov
AS
(SELECT a.samID
       ,a.refID
       ,drpc.refPos
	   ,SUM(drpc.coverage) as coverage
 FROM [dbo].[align] a
 CROSS APPLY [dbo].[DetRefPosCov](a.refPosStart, a.refSeq, a.sreadPosStart, a.sreadPosEnd, a.misMNuc, a.indel) drpc
 GROUP BY a.samID, a.refID, drpc.refPos)
SELECT i.*
	  ,c.coverage
FROM idq i
INNER JOIN cov c ON i.samID = c.samID AND i.refID = c.refID AND i.inDelStartPos = c.refPos

GO

CREATE TABLE tmp_basesCoverBin
(
	[samID] [int] NOT NULL,
	[refID] [int] NOT NULL,
	[pos] [bigint] NOT NULL,
	[refNuc] [nvarchar](1) NOT NULL,
	[coverage] [int] NOT NULL,
	[A] [int] NOT NULL,
	[C] [int] NOT NULL,
	[G] [int] NOT NULL,
	[T] [int] NOT NULL,
	[triplet] [nvarchar](3) NOT NULL,
	CONSTRAINT [PK_tmp_basesCoverBin] PRIMARY KEY CLUSTERED 
	(
		[samID] ASC,
		[refID] ASC,
		[pos] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, DATA_COMPRESSION = PAGE) ON [BCOVERBIN_FG]
) ON [BCOVERBIN_FG]

GO

-- For GTF (General Feature Format) annotations: (see the full specification: http://www.sanger.ac.uk/resources/software/gff/spec.html or http://www.ensembl.org/info/website/upload/gff.html)
CREATE TABLE gtf
(
	[seqname] [varchar](150) NOT NULL, -- name of the chromosome or scaffold
	[source] [varchar](150) NOT NULL, -- name of the program that generated this feature
	[feature] [varchar](150) NOT NULL, -- feature type name, e.g. Gene, Variation, Similarity
	[start] [bigint] NOT NULL, -- start position of the feature (1-based)
	[end] [bigint] NOT NULL, -- end position
	[score] [varchar](50) NOT NULL, -- original this value is floating point value but the character dot '.' indicates that there is no score
	[strand] [char] NOT NULL, -- one of '+' (forward), '-' (reverse) or '.' (strand is not relevant)
	[frame] [char] NOT NULL, -- one of '0' (the first base of the feature is the first base of a codon), '1' (the second base is the first base of a codon), '2'(the third base is the first base of a codon) or '.' (the frame is not relevant)
	[attribute] [varchar](8000) NOT NULL -- a semicolon-separated list of tag-value pairs, providing additional information about each feature
) ON [PRIMARY]

GO

-- For filtered pileups: (related tables from the above approach are [refDesc], [refBin])

CREATE TABLE [dbo].[sample](
	[sampleID] [int] IDENTITY(1,1)PRIMARY KEY,
	[name] [varchar](16) NOT NULL,
	[desc] [varchar](200) NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[pileup](
	[pupID] [int] IDENTITY(1,1)PRIMARY KEY,
	[sampleID] [int] NOT NULL,
	[refID] [int] NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[fltrCov](
	[pupID] [int] NOT NULL,
	[pos] [bigint] NOT NULL,
	[refNuc] [char] NOT NULL,
	[coverage] [int] NOT NULL,
	[bases] [varchar](8000) NOT NULL,
	[basesQual] [varchar](8000) NULL,
	CONSTRAINT [PK_fltr_cov] PRIMARY KEY CLUSTERED 
	(
		[pupID] ASC,
		[pos] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, DATA_COMPRESSION = PAGE) ON [FLTR_COV_FG]
) ON [FLTR_COV_FG]

-- This a schema for ACGT counts

ALTER DATABASE szalaigj
ADD FILEGROUP COVER_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = cover_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\cover_0.ndf',
	SIZE = 500GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = cover_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\cover_1.ndf',
	SIZE = 500GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP COVER_FG;
GO

ALTER DATABASE szalaigj
ADD FILEGROUP COVERLOAD_FG;
GO

ALTER DATABASE szalaigj
ADD FILE
(
	NAME = coverload_0,
	FILENAME = 'C:\Data\Raid6_0\user\sql_db\szalaigj\coverload_0.ndf',
	SIZE = 500GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
),
(
	NAME = coverload_1,
	FILENAME = 'C:\Data\Raid6_1\user\sql_db\szalaigj\coverload_1.ndf',
	SIZE = 500GB,
	MAXSIZE = UNLIMITED,
    FILEGROWTH = 0KB
)
TO FILEGROUP COVERLOAD_FG;
GO

CREATE TABLE [dbo].[cover]
(
	[sampleID] [int] NOT NULL,
	[refID] [int] NOT NULL,
	[pos] [bigint] NOT NULL,
	[refNuc] [nvarchar](1) NOT NULL,
	[coverage] [int] NOT NULL,
	[Acount] [int] NOT NULL,
	[Ccount] [int] NOT NULL,
	[Gcount] [int] NOT NULL,
	[Tcount] [int] NOT NULL,
	[incount] [int] NOT NULL,
	[delcount] [int] NOT NULL,
	CONSTRAINT [PK_cover] PRIMARY KEY CLUSTERED 
	(
		[sampleID] ASC,
		[refID] ASC,
		[pos] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, DATA_COMPRESSION = PAGE) ON [COVER_FG]
) ON [COVER_FG]
GO