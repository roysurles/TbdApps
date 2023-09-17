
DECLARE @SprocName nvarchar(255) = 'IngredientSelect'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 05-May-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[IngredientSelect] @Id UNIQUEIDENTIFIER
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
        WHERE  Id = @Id;
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

