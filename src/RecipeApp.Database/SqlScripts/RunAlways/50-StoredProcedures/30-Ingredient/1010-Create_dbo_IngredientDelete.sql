
DECLARE @SprocName nvarchar(255) = 'IngredientDelete'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[IngredientDelete] @Id UNIQUEIDENTIFIER
AS
    BEGIN
        DELETE dbo.Ingredient
        WHERE  Id = @Id;
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

