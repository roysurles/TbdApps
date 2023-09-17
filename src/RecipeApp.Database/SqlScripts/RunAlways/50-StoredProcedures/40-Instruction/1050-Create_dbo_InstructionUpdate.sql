
DECLARE @SprocName nvarchar(255) = 'InstructionUpdate'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InstructionUpdate] @Id          UNIQUEIDENTIFIER,
                                                    @SortOrder   INT,
                                                    @Description NVARCHAR(255),
                                                    @UpdatedById NVARCHAR(255),
                                                    @UpdatedOnUtc DateTime
AS
    BEGIN
        UPDATE dbo.Instruction
          SET
              SortOrder    = @SortOrder,
              Description  = @Description,
              UpdatedById  = @UpdatedById,
              UpdatedOnUtc = @UpdatedOnUtc
        WHERE  Id = @Id;
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

