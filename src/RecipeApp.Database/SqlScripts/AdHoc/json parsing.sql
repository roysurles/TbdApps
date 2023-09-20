
/*  parse ApiLog.HttpRequestBody json for
	POST https://10.0.2.2:44350/api/v1.0/Introduction/search
*/

select id
, url
, HttpMethod
, HttpRequestBody AS JSON
, d.searchText
, d.pageNumber
, d.pageSize
, d.propertyName
, d.direction
from dbo.ApiLog ApiLog
--OUTER APPLY [dbo].[GetTableFromJson](ApiLog.HttpRequestBody) d
outer apply (
	SELECT searchText
	, pageNumber
	, pageSize
	, propertyName
	, direction
	FROM OPENJSON( ApiLog.HttpRequestBody ) WITH (
		searchText nvarchar(255) '$.searchText',
		pageNumber int '$.pageNumber',
		pageSize int '$.pageSize',
		orderByClauses NVARCHAR(MAX) '$.orderByClause' AS JSON
		)
	OUTER APPLY OPENJSON(orderByClauses) WITH (propertyName NVARCHAR(max) '$.propertyName', direction nvarchar(10) '$.direction')
) d

-- could create a function(s) to parse json for specific schemas
-- ================================================
-- Template generated from Template Explorer using:
-- Create Inline Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters
-- command (Ctrl-Shift-M) to fill in the parameter
-- values below.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION GetTableFromJson
(
	-- Add the parameters for the function here
	@json nvarchar(max)
)
RETURNS TABLE
AS
RETURN
(
	-- Add the SELECT statement with parameter references here
	SELECT searchText
	, pageNumber
	, pageSize
	, propertyName
	, direction
	FROM OPENJSON( @json ) WITH (
		searchText nvarchar(255) '$.searchText',
		pageNumber int '$.pageNumber',
		pageSize int '$.pageSize',
		orderByClauses NVARCHAR(MAX) '$.orderByClause' AS JSON
		)
	OUTER APPLY OPENJSON(orderByClauses) WITH (propertyName NVARCHAR(max) '$.propertyName', direction nvarchar(10) '$.direction')
)
GO

