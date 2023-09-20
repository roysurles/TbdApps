

TRUNCATE TABLE dbo.ApiLog
TRUNCATE TABLE dbo.Ingredient
TRUNCATE TABLE dbo.Instruction
TRUNCATE TABLE dbo.Introduction

DBCC SHRINKDATABASE (Recipe, 10);
GO

DECLARE @NewIntroductionId UNIQUEIDENTIFIER
DECLARE @Counter INT
SET @Counter=1

WHILE ( @Counter <= 10 )
BEGIN

    SET @NewIntroductionId = NEWID()

    -- Introduction
    INSERT INTO dbo.Introduction (Id, Title, Comment)
    VALUES
        (@NewIntroductionId, 'Recipe Title ' + CONVERT(NVARCHAR(4),@Counter), 'Recipe Comment ' + CONVERT(NVARCHAR(4),@Counter))

    -- Ingredient
    INSERT INTO dbo.Ingredient (Id, IntroductionId, SortOrder, Measurement, Description)
    VALUES
        (NEWID(), @NewIntroductionId, 1, 'Measurement 1', 'Ingredient 1'),
        (NEWID(), @NewIntroductionId, 2, 'Measurement 2', 'Ingredient 2'),
        (NEWID(), @NewIntroductionId, 3, 'Measurement 3', 'Ingredient 3'),
        (NEWID(), @NewIntroductionId, 4, 'Measurement 4', 'Ingredient 4')

    -- Instruction
    INSERT INTO dbo.Instruction (Id, IntroductionId, SortOrder, Description)
    VALUES
        (NEWID(), @NewIntroductionId, 1, 'Instruction 1'),
        (NEWID(), @NewIntroductionId, 2, 'Instruction 2'),
        (NEWID(), @NewIntroductionId, 3, 'Instruction 3'),
        (NEWID(), @NewIntroductionId, 4, 'Instruction 4')

    SET @Counter  = @Counter  + 1
END
