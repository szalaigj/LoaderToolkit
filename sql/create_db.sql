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
SELECT [refID]
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
(SELECT r.refID
,r.pos
,r.refNuc
,COUNT(*) as coverage
,SUM([dbo].[IsNucX](s.posStart, s.misMNuc, r.pos, r.refNuc, 'A')) as A
,SUM([dbo].[IsNucX](s.posStart, s.misMNuc, r.pos, r.refNuc, 'C')) as C
,SUM([dbo].[IsNucX](s.posStart, s.misMNuc, r.pos, r.refNuc, 'G')) as G
,SUM([dbo].[IsNucX](s.posStart, s.misMNuc, r.pos, r.refNuc, 'T')) as T
,LAG(r.pos,1) OVER (ORDER BY r.refID, r.pos) as lagPos
,LAG(r.refNuc,1) OVER (ORDER BY r.refID, r.pos) as lagValue
,LEAD(r.pos,1) OVER (ORDER BY r.refID, r.pos) as leadPos
,LEAD(r.refNuc,1) OVER (ORDER BY r.refID, r.pos) as leadValue
FROM dbo.ref r
INNER JOIN [dbo].sread s ON r.refID = s.refID AND (r.pos BETWEEN s.posStart AND s.posEnd)
AND [dbo].[IsDel](s.posStart, s.indel, r.pos) = 0
GROUP BY r.refID, r.pos, r.refNuc) as counters

GO

CREATE VIEW [dbo].[inDel]
(
		[refID]
		,[pos]
		,[coverage]
		,[inDel]
		,[chainLen]
		,[nucChain]
)
AS
SELECT [refID]
      ,[pos]
      ,[coverage]
	  ,[did].[inDel]
	  ,[did].[chainLen]
	  ,[did].[nucChain]
FROM
(SELECT r.refID
,r.pos
,r.refNuc
,s.indel
,s.posStart
,COUNT(*) as coverage
FROM dbo.ref r
INNER JOIN [dbo].sread s ON r.refID = s.refID AND (r.pos BETWEEN s.posStart AND s.posEnd)
WHERE s.[indel] != ''
GROUP BY r.refID, r.pos, r.refNuc, s.indel, s.posStart) as [innerTbl]
CROSS APPLY [dbo].[DetermineInDel]([posStart], [indel]) as [did]

GO