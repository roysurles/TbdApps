--USE Redigent
--GO


SET NOCOUNT ON


DECLARE @TableName nvarchar(255) = 'Instruction'

PRINT 'Processing dbo.' + @TableName
IF OBJECT_ID (N'dbo.' + @TableName, N'U') IS NOT NULL
	BEGIN
		PRINT '-- dbo.' + @TableName + ' exists'
	END
ELSE
	BEGIN

		PRINT '++ Creating dbo.' + @TableName + ' table'

		CREATE TABLE [dbo].[Instruction](
			[Id] [uniqueidentifier] NOT NULL,
			[IntroductionId] [uniqueidentifier] NOT NULL,
			[SortOrder] [int] NOT NULL,
			[Description] [nvarchar](255) NOT NULL,
			[CreatedById] [nvarchar](255) NULL,
			[CreatedOnUtc] [datetime] NULL,
			[UpdatedById] [nvarchar](255) NULL,
			[UpdatedOnUtc] [datetime] NULL,
		 CONSTRAINT [PK_Instruction] PRIMARY KEY CLUSTERED
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[Instruction] ADD  CONSTRAINT [DF_Instruction_SortOrder]  DEFAULT ((0)) FOR [SortOrder]

		ALTER TABLE [dbo].[Instruction] ADD  CONSTRAINT [DF_Instruction_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOnUtc]

	END

PRINT REPLICATE('*',60) + CHAR(13)

