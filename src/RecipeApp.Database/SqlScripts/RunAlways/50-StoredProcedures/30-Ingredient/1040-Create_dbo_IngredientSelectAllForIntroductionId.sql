
DECLARE @SprocName nvarchar(255) = 'IngredientSelectAllForIntroductionId'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 28-May-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[IngredientSelectAllForIntroductionId] @IntroductionId UNIQUEIDENTIFIER
AS
    BEGIN
        SELECT Id
              ,IntroductionId
              ,SortOrder
              ,Measurement
              ,Description
              ,CreatedById
              ,CreatedOnUtc
              ,UpdatedById
              ,UpdatedOnUtc
        FROM   dbo.Ingredient
        WHERE  IntroductionId = @IntroductionId
        ORDER BY SortOrder;
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

