
DECLARE @SprocName nvarchar(255) = 'IngredientInsert'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[IngredientInsert] @Id             UNIQUEIDENTIFIER,
                                                   @IntroductionId UNIQUEIDENTIFIER,
                                                   @SortOrder	   INT,
                                                   @Measurement    NVARCHAR(50),
                                                   @Description    NVARCHAR(255),
                                                   @CreatedById    NVARCHAR(255),
                                                   @CreatedOnUtc   DateTime
AS
    BEGIN
        INSERT INTO dbo.Ingredient
        (Id
        ,IntroductionId
        ,SortOrder
        ,Measurement
        ,Description
        ,CreatedById
        ,CreatedOnUtc
        )
        VALUES
        (@Id
        ,@IntroductionId
        ,@SortOrder
        ,@Measurement
        ,@Description
        ,@CreatedById
        ,@CreatedOnUtc
        );

    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

