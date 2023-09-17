
DECLARE @SprocName nvarchar(255) = 'InstructionInsert'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InstructionInsert] @Id           UNIQUEIDENTIFIER,
                                                    @SortOrder		INT,
                                                    @IntroductionId  UNIQUEIDENTIFIER,
                                                    @Description		NVARCHAR(255),
                                                    @CreatedById		NVARCHAR(255),
                                                    @CreatedOnUtc	DateTime
AS
    BEGIN
        INSERT INTO dbo.Instruction
        (Id
        ,IntroductionId
        ,SortOrder
        ,Description
        ,CreatedById
        ,CreatedOnUtc
        )
        VALUES
        (@Id
        ,@IntroductionId
        ,@SortOrder
        ,@Description
        ,@CreatedById
        ,@CreatedOnUtc
        );
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

