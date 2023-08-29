USE [master]
GO
/****** Object:  Database [Recipe]    Script Date: 2/15/2022 4:29:36 PM ******/
CREATE DATABASE [Recipe]
 CONTAINMENT = NONE
 ON  PRIMARY
( NAME = N'Recipe', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Recipe.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON
( NAME = N'Recipe_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Recipe_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Recipe] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Recipe].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Recipe] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [Recipe] SET ANSI_NULLS OFF
GO
ALTER DATABASE [Recipe] SET ANSI_PADDING OFF
GO
ALTER DATABASE [Recipe] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [Recipe] SET ARITHABORT OFF
GO
ALTER DATABASE [Recipe] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [Recipe] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [Recipe] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [Recipe] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [Recipe] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [Recipe] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [Recipe] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [Recipe] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [Recipe] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [Recipe] SET  DISABLE_BROKER
GO
ALTER DATABASE [Recipe] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [Recipe] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [Recipe] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [Recipe] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [Recipe] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [Recipe] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [Recipe] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [Recipe] SET RECOVERY SIMPLE
GO
ALTER DATABASE [Recipe] SET  MULTI_USER
GO
ALTER DATABASE [Recipe] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [Recipe] SET DB_CHAINING OFF
GO
ALTER DATABASE [Recipe] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF )
GO
ALTER DATABASE [Recipe] SET TARGET_RECOVERY_TIME = 60 SECONDS
GO
ALTER DATABASE [Recipe] SET DELAYED_DURABILITY = DISABLED
GO
ALTER DATABASE [Recipe] SET ACCELERATED_DATABASE_RECOVERY = OFF
GO
ALTER DATABASE [Recipe] SET QUERY_STORE = OFF
GO
USE [Recipe]
GO
/****** Object:  Table [dbo].[ApiLog]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiLog](
    [Id] [uniqueidentifier] NOT NULL,
    [ConnectionId] [nvarchar](255) NULL,
    [TraceId] [nvarchar](255) NULL,
    [MachineName] [nvarchar](255) NULL,
    [UserAgent] [nvarchar](255) NULL,
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
/****** Object:  Table [dbo].[Ingredient]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingredient](
    [Id] [uniqueidentifier] NOT NULL,
    [IntroductionId] [uniqueidentifier] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [Measurement] [nvarchar](50) NOT NULL,
    [Description] [nvarchar](255) NOT NULL,
    [CreatedById] [nvarchar](255) NULL,
    [CreatedOnUtc] [datetime] NULL,
    [UpdatedById] [nvarchar](255) NULL,
    [UpdatedOnUtc] [datetime] NULL,
 CONSTRAINT [PK_Ingredient] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Instruction]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Instruction](
    [Id] [uniqueidentifier] NOT NULL,
    [IntroductionId] [uniqueidentifier] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [Description] [nvarchar](255) NOT NULL,
    [CreatedById] [nvarchar](255) NULL,
    [CreatedOnUtc] [datetime] NULL,
    [UpdatedById] [nvarchar](255) NULL,
    [UpdatedOnUtc] [datetime] NULL,
 CONSTRAINT [PK_Instruction] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Introduction]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Introduction](
    [Id] [uniqueidentifier] NOT NULL,
    [Title] [nvarchar](50) NOT NULL,
    [Comment] [nvarchar](255) NULL,
    [CreatedById] [nvarchar](255) NULL,
    [CreatedOnUtc] [datetime] NULL,
    [UpdatedById] [nvarchar](255) NULL,
    [UpdatedOnUtc] [datetime] NULL,
 CONSTRAINT [PK_Introduction] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Ingredient] ADD  CONSTRAINT [DF_Ingredient_SortOrder]  DEFAULT ((0)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[Instruction] ADD  CONSTRAINT [DF_Instruction_SortOrder]  DEFAULT ((0)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[Instruction] ADD  CONSTRAINT [DF_Instruction_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOnUtc]
GO
ALTER TABLE [dbo].[Introduction] ADD  CONSTRAINT [DF_Introduction_Id]  DEFAULT (newid()) FOR [Id]
GO
/****** Object:  StoredProcedure [dbo].[IngredientDelete]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE PROCEDURE [dbo].[IngredientDelete] @Id UNIQUEIDENTIFIER
AS
    BEGIN
        DELETE dbo.Ingredient
        WHERE  Id = @Id;
    END;
GO
/****** Object:  StoredProcedure [dbo].[IngredientInsert]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE PROCEDURE [dbo].[IngredientInsert] @Id             UNIQUEIDENTIFIER,
                                      @IntroductionId UNIQUEIDENTIFIER,
                                      @SortOrder	  INT,
                                      @Measurement    NVARCHAR(50),
                                      @Description    NVARCHAR(255),
                                      @CreatedById NVARCHAR(255),
                                      @CreatedOnUtc DateTime
AS
    BEGIN
        INSERT INTO dbo.Ingredient
        (Id
        ,IntroductionId
        ,SortOrder
        ,Measurement
        ,Description
        ,CreatedById
        ,CreatedOnUtc
        )
        VALUES
        (@Id
        ,@IntroductionId
        ,@SortOrder
        ,@Measurement
        ,@Description
        ,@CreatedById
        ,@CreatedOnUtc
        );

    END;
GO
/****** Object:  StoredProcedure [dbo].[IngredientSelect]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 05-May-2021
-- Description:	Initial Creation
-- =============================================

CREATE PROCEDURE [dbo].[IngredientSelect] @Id UNIQUEIDENTIFIER
AS
    BEGIN
        SELECT Id
              ,IntroductionId
              ,SortOrder
              ,Measurement
              ,Description
              ,CreatedById
              ,CreatedOnUtc
              ,UpdatedById
              ,UpdatedOnUtc
        FROM   dbo.Ingredient
        WHERE  Id = @Id;
    END;
GO
/****** Object:  StoredProcedure [dbo].[IngredientSelectAllForIntroductionId]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 28-May-2021
-- Description:	Initial Creation
-- =============================================
CREATE PROCEDURE [dbo].[IngredientSelectAllForIntroductionId] @IntroductionId UNIQUEIDENTIFIER
AS
    BEGIN
        SELECT Id
              ,IntroductionId
              ,SortOrder
              ,Measurement
              ,Description
              ,CreatedById
              ,CreatedOnUtc
              ,UpdatedById
              ,UpdatedOnUtc
        FROM   dbo.Ingredient
        WHERE  IntroductionId = @IntroductionId;
    END;
GO
/****** Object:  StoredProcedure [dbo].[IngredientUpdate]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE PROCEDURE [dbo].[IngredientUpdate] @Id          UNIQUEIDENTIFIER,
                                     @SortOrder	  INT,
                                     @Measurement NVARCHAR(50),
                                     @Description NVARCHAR(255),
                                     @UpdatedById NVARCHAR(255),
                                     @UpdatedOnUtc DateTime
AS
    BEGIN
        UPDATE dbo.Ingredient
          SET
              SortOrder    = @SortOrder,
              Measurement  = @Measurement,
              Description  = @Description,
              UpdatedById  = @UpdatedById,
              UpdatedOnUtc = @UpdatedOnUtc
        WHERE  Id = @Id;
    END;
GO
/****** Object:  StoredProcedure [dbo].[InstructionDelete]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE PROCEDURE [dbo].[InstructionDelete] @Id UNIQUEIDENTIFIER
AS
    BEGIN
        DELETE dbo.Instruction
        WHERE  Id = @Id;
    END;
GO
/****** Object:  StoredProcedure [dbo].[InstructionInsert]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE PROCEDURE [dbo].[InstructionInsert] @Id           UNIQUEIDENTIFIER,
                                       @SortOrder		INT,
                                       @IntroductionId  UNIQUEIDENTIFIER,
                                       @Description		NVARCHAR(255),
                                       @CreatedById		NVARCHAR(255),
                                       @CreatedOnUtc	DateTime
AS
    BEGIN
        INSERT INTO dbo.Instruction
        (Id
        ,IntroductionId
        ,SortOrder
        ,Description
        ,CreatedById
        ,CreatedOnUtc
        )
        VALUES
        (@Id
        ,@IntroductionId
        ,@SortOrder
        ,@Description
        ,@CreatedById
        ,@CreatedOnUtc
        );
    END;
GO
/****** Object:  StoredProcedure [dbo].[InstructionSelect]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 05-May-2021
-- Description:	Initial Creation
-- =============================================

CREATE PROCEDURE [dbo].[InstructionSelect] @Id UNIQUEIDENTIFIER
AS
    BEGIN
        SELECT Id
              ,IntroductionId
              ,SortOrder
              ,Description
              ,CreatedById
              ,CreatedOnUtc
              ,UpdatedById
              ,UpdatedOnUtc
        FROM   dbo.Instruction
        WHERE  Id = @Id;
    END;
GO
/****** Object:  StoredProcedure [dbo].[InstructionSelectAllForIntroductionId]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 28-May-2021
-- Description:	Initial Creation
-- =============================================
CREATE PROCEDURE [dbo].[InstructionSelectAllForIntroductionId] @IntroductionId UNIQUEIDENTIFIER
AS
    BEGIN
        SELECT Id
              ,IntroductionId
              ,SortOrder
              ,Description
              ,CreatedById
              ,CreatedOnUtc
              ,UpdatedById
              ,UpdatedOnUtc
        FROM   dbo.Instruction
        WHERE  IntroductionId = @IntroductionId;
    END;
GO
/****** Object:  StoredProcedure [dbo].[InstructionUpdate]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE PROCEDURE [dbo].[InstructionUpdate] @Id          UNIQUEIDENTIFIER,
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
/****** Object:  StoredProcedure [dbo].[IntroductionDelete]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE PROCEDURE [dbo].[IntroductionDelete] @Id UNIQUEIDENTIFIER
AS
    BEGIN
        DELETE dbo.Ingredient
        WHERE  IntroductionId = @Id;

        DELETE dbo.Instruction
        WHERE  IntroductionId = @Id;

        DELETE dbo.Introduction
        WHERE  Id = @Id;
    END;
GO
/****** Object:  StoredProcedure [dbo].[IntroductionInsert]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE PROCEDURE [dbo].[IntroductionInsert] @Id           UNIQUEIDENTIFIER,
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
/****** Object:  StoredProcedure [dbo].[IntroductionSearch]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 21-May-2021
-- Description:	Initial Creation
-- =============================================

CREATE PROCEDURE [dbo].[IntroductionSearch] @SearchText NVARCHAR(50),
                                            @Offset     INT,
                                            @Fetch      INT
AS
    BEGIN
        IF RTRIM(@SearchText) = ''
            SET @SearchText = NULL

        SELECT COUNT(Id) AS TotalItemCount
        FROM   dbo.Introduction
        WHERE  @SearchText IS NULL
               OR @SearchText IS NOT NULL
               AND Title LIKE '%' + @SearchText + '%'
               OR @SearchText IS NOT NULL
               AND Comment LIKE '%' + @SearchText + '%';

        SELECT    IntroductionResult.Id
                 ,IntroductionResult.Title
                 ,IntroductionResult.Comment
                 ,IngredientsCount = ( SELECT COUNT(*) FROM dbo.Ingredient WHERE IntroductionId = IntroductionResult.Id )
                 ,InstructionsCount = ( SELECT COUNT(*) FROM dbo.Instruction WHERE IntroductionId = IntroductionResult.Id )
        FROM (
                SELECT    Id
                         ,Title
                         ,Comment
                FROM dbo.Introduction
                WHERE @SearchText IS NULL
                      OR @SearchText IS NOT NULL
                      AND Title LIKE '%' + @SearchText + '%'
                      OR @SearchText IS NOT NULL
                      AND Comment LIKE '%' + @SearchText + '%'
                ORDER BY Title
                OFFSET @Offset ROWS FETCH NEXT @Fetch ROWS ONLY
             ) IntroductionResult
    END;
GO
/****** Object:  StoredProcedure [dbo].[IntroductionSelect]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 05-May-2021
-- Description:	Initial Creation
-- =============================================

CREATE PROCEDURE [dbo].[IntroductionSelect] @Id UNIQUEIDENTIFIER
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
/****** Object:  StoredProcedure [dbo].[IntroductionUpdate]    Script Date: 2/15/2022 4:29:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Surles
-- Create date: 23-Apr-2021
-- Description:	Initial Creation
-- =============================================
CREATE PROCEDURE [dbo].[IntroductionUpdate] @Id           UNIQUEIDENTIFIER,
                                       @Title        NVARCHAR(50),
                                       @Comment      NVARCHAR(255),
                                       @UpdatedById  NVARCHAR(255),
                                       @UpdatedOnUtc DATETIME
AS
    BEGIN
        IF @UpdatedOnUtc IS NULL
            SET @UpdatedOnUtc = GETUTCDATE();
        UPDATE dbo.Introduction
          SET
              Title = @Title,
              Comment = @Comment,
              UpdatedById = @UpdatedById,
              UpdatedOnUtc = @UpdatedOnUtc
        WHERE  Id = @Id;
    END;
GO
USE [master]
GO
ALTER DATABASE [Recipe] SET  READ_WRITE
GO
