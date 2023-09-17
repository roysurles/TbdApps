
DECLARE @SprocName nvarchar(255) = 'IntroductionSearch'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 21-May-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[IntroductionSearch] @SearchText NVARCHAR(50),
                                                     @Offset     INT,
                                                     @Fetch      INT
AS
    BEGIN
        IF RTRIM(@SearchText) = ''
            SET @SearchText = NULL

        SELECT COUNT(Id) AS TotalItemCount
        FROM   dbo.Introduction
        WHERE  @SearchText IS NULL
               OR @SearchText IS NOT NULL
               AND Title LIKE '%' + @SearchText + '%'
               OR @SearchText IS NOT NULL
               AND Comment LIKE '%' + @SearchText + '%';

        SELECT    IntroductionResult.Id
                 ,IntroductionResult.Title
                 ,IntroductionResult.Comment
                 ,IngredientsCount = ( SELECT COUNT(*) FROM dbo.Ingredient WHERE IntroductionId = IntroductionResult.Id )
                 ,InstructionsCount = ( SELECT COUNT(*) FROM dbo.Instruction WHERE IntroductionId = IntroductionResult.Id )
        FROM (
                SELECT    Id
                         ,Title
                         ,Comment
                FROM dbo.Introduction
                WHERE @SearchText IS NULL
                      OR @SearchText IS NOT NULL
                      AND Title LIKE '%' + @SearchText + '%'
                      OR @SearchText IS NOT NULL
                      AND Comment LIKE '%' + @SearchText + '%'
                ORDER BY Title
                OFFSET @Offset ROWS FETCH NEXT @Fetch ROWS ONLY
             ) IntroductionResult
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

