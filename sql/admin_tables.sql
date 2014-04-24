CREATE TABLE [dbo].[batch](
	[batch_id] [int] IDENTITY(1,1) NOT NULL,
	--[run_id] [smallint] NOT NULL,
	[target_db] [nvarchar](512) NOT NULL,
	[loader_db] [nvarchar](512) NOT NULL,
	[source_path] [nvarchar](512) NOT NULL,
	[file_suffix] [nvarchar](30) NULL,
	[bulk_path] [nvarchar](512) NOT NULL,
	[binary] [bit] NOT NULL,
 CONSTRAINT [PK_batch] PRIMARY KEY CLUSTERED 
(
	[batch_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[chunk](
	[batch_id] [int] NOT NULL,
	[chunk_id] [nvarchar](128) NOT NULL,
	[prepare_start] [datetime] NULL,
	[prepare_end] [datetime] NULL,
	[load_start] [datetime] NULL,
	[load_end] [datetime] NULL,
	[merge_start] [datetime] NULL,
	[merge_end] [datetime] NULL,
	[cleanup_start] [datetime] NULL,
	[cleanup_end] [datetime] NULL,
 CONSTRAINT [PK_chunk_1] PRIMARY KEY CLUSTERED 
(
	[batch_id] ASC,
	[chunk_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

