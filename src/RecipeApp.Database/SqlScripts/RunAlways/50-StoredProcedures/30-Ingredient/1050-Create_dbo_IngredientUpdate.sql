
DECLARE @SprocName nvarchar(255) = 'IngredientUpdate'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[IngredientUpdate] @Id           UNIQUEIDENTIFIER,
                                                   @SortOrder	 INT,
                                                   @Measurement  NVARCHAR(50),
                                                   @Description  NVARCHAR(255),
                                                   @UpdatedById  NVARCHAR(255),
                                                   @UpdatedOnUtc DateTime
AS
    BEGIN
        UPDATE dbo.Ingredient
          SET
              SortOrder    = @SortOrder,
              Measurement  = @Measurement,
              Description  = @Description,
              UpdatedById  = @UpdatedById,
              UpdatedOnUtc = @UpdatedOnUtc
        WHERE  Id = @Id;
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

