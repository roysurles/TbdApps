USE [Recipe]
GO

/****** Object:  Table [dbo].[ApiLog]    Script Date: 5/5/2021 3:59:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ApiLog](
	[Id] [uniqueidentifier] NOT NULL,
	[ConnectionId] [nvarchar](255) NULL,
	[TraceId] [nvarchar](255) NULL,
	[Claims] [nvarchar](max) NULL,
	[LocalIpAddress] [nvarchar](50) NULL,
	[RemoteIpAddress] [nvarchar](50) NULL,
	[AssemblyName] [nvarchar](255) NULL,
	[Url] [nvarchar](255) NULL,
	[ControllerName] [nvarchar](100) NULL,
	[ActionName] [nvarchar](100) NULL,
	[ActionDateTimeOffset] [datetimeoffset](7) NULL,
	[HttpProtocol] [nvarchar](25) NULL,
	[HttpMethod] [nvarchar](10) NULL,
	[HttpStatusCode] [int] NULL,
	[ExceptionData] [nvarchar](max) NULL,
	[ElapsedMilliseconds] [int] NULL,
	[HttpRequestBody] [nvarchar](max) NULL,
	[HttpResponseBody] [nvarchar](max) NULL,
 CONSTRAINT [PK_ApiLog] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


