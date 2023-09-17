
DECLARE @SprocName nvarchar(255) = 'IntroductionInsert'

PRINT 'Processing dbo.' + @SprocName

GO

-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[IntroductionInsert] @Id UNIQUEIDENTIFIER,
                                                     @Title        NVARCHAR(50),
                                                     @Comment      NVARCHAR(255),
                                                     @CreatedById  NVARCHAR(255),
                                                     @CreatedOnUtc DATETIME
AS
    BEGIN
        IF @Id IS NULL
            SET @Id = NEWID();
        IF @Id = '00000000-0000-0000-0000-000000000000'
            SET @Id = NEWID();
        IF @CreatedOnUtc IS NULL
            SET @CreatedOnUtc = GETUTCDATE();
        INSERT INTO dbo.Introduction
        (Id
        ,Title
        ,Comment
        ,CreatedById
        ,CreatedOnUtc
        )
        VALUES
        (@Id
        ,@Title
        ,@Comment
        ,@CreatedById
        ,@CreatedOnUtc
        );
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

