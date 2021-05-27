/****** Object:  StoredProcedure [dbo].[NP_ProjectResequenceItems]    Script Date: 5/27/2021 1:46:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      <macharya>
-- Create Date: <5/24/2021>
-- Description:
/*
Algorithm

1) Convert Json parameter to table & insert into table variable
2) If count from table variable is not the same as count from tblProject_Det, then exit with 404
3) Update all items for @ProjectId, setting their LineNumber = (LineNumber * -1)
4) Loop thru table variable
5) Process current record from table variable - update tblProject_Det set LineNumber = xxx where ItemId = ItemId
6) Return a success code -- 200 or 1
*/

-- =============================================

ALTER PROCEDURE [dbo].[NP_ProjectResequenceItems]
(
 @ProjectId UNIQUEIDENTIFIER,
 @ItemsJson NVARCHAR(MAX),
 @UpdateUserID UNIQUEIDENTIFIER,
 @UpdateDate datetime


 )
 AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

Declare @ItemsTable TABLE
(ItemId        UNIQUEIDENTIFIER,
NewLineNumber INT
--IsProcessed   BIT
)


/*** Step 1 ***/
INSERT INTO @ItemsTable
SELECT ItemId
      ,NewLineNumber
        --,0
FROM OPENJSON(@ItemsJson)
WITH(
       Items NVARCHAR(MAX) 'lax $.items' AS JSON
)
OUTER APPLY OPENJSON(Items)
WITH(
       ItemId UNIQUEIDENTIFIER 'lax $.itemId',
       NewLineNumber NVARCHAR(MAX) 'lax $.newLineNumber'
);


/*** Step 2 ***/
IF
(
    SELECT COUNT(*)
    FROM @ItemsTable
) !=
(
    SELECT COUNT(*)
    FROM tblProject_Det
    WHERE ProjectId = @ProjectId
)
    BEGIN
        SELECT 404 as StatusCode;
        RETURN;
    END;

/*** Step 3 ***/
UPDATE tblProject_Det
  SET
      tblProject_Det.LineNumber = ItemsTable.NewLineNumber,
       [UpdatedBy]   =@UpdateUserID ,
       [UpdatedOn]  = @UpdateDate
FROM tblProject_Det tblProject_Det
     INNER JOIN @ItemsTable ItemsTable ON ItemsTable.ItemId = tblProject_Det.ItemID
WHERE tblProject_Det.ProjectId = @ProjectId;

/*** Step 4 ***/
UPDATE tblProject
set [UpdateUserID] =@UpdateUserID ,
    [UpdateDate]  =@UpdateDate
    WHERE ProjectID = @ProjectID


/*** Step 5 ***/

SELECT 200  as StatusCode;
END
