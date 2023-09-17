
DECLARE @SprocName nvarchar(255) = 'InstructionSelect'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 05-May-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InstructionSelect] @Id UNIQUEIDENTIFIER
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
        WHERE  Id = @Id;
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

