
DECLARE @SprocName nvarchar(255) = 'IntroductionUpdate'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[IntroductionUpdate] @Id           UNIQUEIDENTIFIER,
                                                     @Title        NVARCHAR(50),
                                                     @Comment      NVARCHAR(255),
                                                     @UpdatedById  NVARCHAR(255),
                                                     @UpdatedOnUtc DATETIME
AS
    BEGIN
        IF @UpdatedOnUtc IS NULL
            SET @UpdatedOnUtc = GETUTCDATE();
        UPDATE dbo.Introduction
          SET
              Title = @Title,
              Comment = @Comment,
              UpdatedById = @UpdatedById,
              UpdatedOnUtc = @UpdatedOnUtc
        WHERE  Id = @Id;
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

