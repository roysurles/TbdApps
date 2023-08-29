use recipe
go
 -- select * from [dbo].[Introduction]

select * from ApiLog
order by ActionDateTimeOffset desc

/*

truncate table ApiLog

DBCC SHRINKDATABASE (Recipe, 10);
GO

*/


