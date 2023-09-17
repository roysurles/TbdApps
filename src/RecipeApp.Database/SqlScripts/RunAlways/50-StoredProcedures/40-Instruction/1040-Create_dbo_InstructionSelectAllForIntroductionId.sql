
DECLARE @SprocName nvarchar(255) = 'InstructionSelectAllForIntroductionId'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 28-May-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InstructionSelectAllForIntroductionId] @IntroductionId UNIQUEIDENTIFIER
AS
    BEGIN
        SELECT Id
              ,IntroductionId
              ,SortOrder
              ,Description
              ,CreatedById
              ,CreatedOnUtc
              ,UpdatedById
              ,UpdatedOnUtc
        FROM   dbo.Instruction
        WHERE  IntroductionId = @IntroductionId;
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

