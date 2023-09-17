--USE Redigent
--GO


SET NOCOUNT ON


DECLARE @TableName nvarchar(255) = 'Introduction'

PRINT 'Processing dbo.' + @TableName
IF OBJECT_ID (N'dbo.' + @TableName, N'U') IS NOT NULL
	BEGIN
		PRINT '-- dbo.' + @TableName + ' exists'
	END
ELSE
	BEGIN

		PRINT '++ Creating dbo.' + @TableName + ' table'

		CREATE TABLE [dbo].[Introduction](
			[Id] [uniqueidentifier] NOT NULL,
			[Title] [nvarchar](50) NOT NULL,
			[Comment] [nvarchar](255) NULL,
			[CreatedById] [nvarchar](255) NULL,
			[CreatedOnUtc] [datetime] NULL,
			[UpdatedById] [nvarchar](255) NULL,
			[UpdatedOnUtc] [datetime] NULL,
		 CONSTRAINT [PK_Introduction] PRIMARY KEY CLUSTERED
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY];

		ALTER TABLE [dbo].[Introduction] ADD  CONSTRAINT [DF_Introduction_Id]  DEFAULT (newid()) FOR [Id]

	END

PRINT REPLICATE('*',60) + CHAR(13)

