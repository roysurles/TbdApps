
DECLARE @SprocName nvarchar(255) = 'IntroductionDelete'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[IntroductionDelete] @Id UNIQUEIDENTIFIER
AS
    BEGIN
        DELETE dbo.Ingredient
        WHERE  IntroductionId = @Id;

        DELETE dbo.Instruction
        WHERE  IntroductionId = @Id;

        DELETE dbo.Introduction
        WHERE  Id = @Id;
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

