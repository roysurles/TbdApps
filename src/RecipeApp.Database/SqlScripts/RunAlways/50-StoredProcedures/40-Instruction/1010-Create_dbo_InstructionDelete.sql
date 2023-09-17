
DECLARE @SprocName nvarchar(255) = 'InstructionDelete'

PRINT 'Processing dbo.' + @SprocName

GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InstructionDelete] @Id UNIQUEIDENTIFIER
AS
    BEGIN
        DELETE dbo.Instruction
        WHERE  Id = @Id;
    END;

GO
PRINT REPLICATE('*',60) + CHAR(13)

