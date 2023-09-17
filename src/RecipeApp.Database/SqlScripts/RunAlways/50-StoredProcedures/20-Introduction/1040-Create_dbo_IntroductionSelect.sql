
DECLARE @SprocName nvarchar(255) = 'IntroductionSelect'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 05-May-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[IntroductionSelect] @Id UNIQUEIDENTIFIER
AS
    BEGIN
        SELECT Id
              ,Title
              ,Comment
              ,CreatedById
              ,CreatedOnUtc
              ,UpdatedById
              ,UpdatedOnUtc
        FROM   dbo.Introduction
        WHERE  Id = @Id;
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

