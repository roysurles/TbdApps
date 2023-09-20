use recipe
go
 -- select * from [dbo].[Introduction]

select * from ApiLog
order by ActionDateTimeOffset desc

/*

truncate table dbo.ApiLog
truncate table dbo.Ingredient
truncate table dbo.Instruction
truncate table dbo.Introduction

DBCC SHRINKDATABASE (Recipe, 10);
GO

*/


