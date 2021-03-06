USE [BeerPongFederation]
GO
/****** Object:  StoredProcedure [dbo].[PlayerNameSearch]    Script Date: 11/08/2016 23:16:30 ******/
DROP PROCEDURE [dbo].[PlayerNameSearch]
GO
/****** Object:  StoredProcedure [dbo].[GetLastUpdateSignature]    Script Date: 11/08/2016 23:16:30 ******/
DROP PROCEDURE [dbo].[GetLastUpdateSignature]
GO
/****** Object:  StoredProcedure [dbo].[GetAvailableGames]    Script Date: 11/08/2016 23:16:30 ******/
DROP PROCEDURE [dbo].[GetAvailableGames]
GO
ALTER TABLE [dbo].[PlayerGame] DROP CONSTRAINT [FK_PlayerGame_Player]
GO
ALTER TABLE [dbo].[PlayerGame] DROP CONSTRAINT [FK_PlayerGame_Game]
GO
ALTER TABLE [dbo].[GameSetting] DROP CONSTRAINT [FK_GameSetting_Setting]
GO
ALTER TABLE [dbo].[GameSetting] DROP CONSTRAINT [FK_GameSetting_Game]
GO
ALTER TABLE [dbo].[CupTracker] DROP CONSTRAINT [FK_CupTracker_Game]
GO
/****** Object:  Index [NameSearch]    Script Date: 11/08/2016 23:16:30 ******/
DROP INDEX [NameSearch] ON [dbo].[Player]
GO
/****** Object:  Table [dbo].[Setting]    Script Date: 11/08/2016 23:16:30 ******/
DROP TABLE [dbo].[Setting]
GO
/****** Object:  Table [dbo].[PlayerGame]    Script Date: 11/08/2016 23:16:30 ******/
DROP TABLE [dbo].[PlayerGame]
GO
/****** Object:  Table [dbo].[Player]    Script Date: 11/08/2016 23:16:30 ******/
DROP TABLE [dbo].[Player]
GO
/****** Object:  Table [dbo].[GameSetting]    Script Date: 11/08/2016 23:16:30 ******/
DROP TABLE [dbo].[GameSetting]
GO
/****** Object:  Table [dbo].[Game]    Script Date: 11/08/2016 23:16:30 ******/
DROP TABLE [dbo].[Game]
GO
/****** Object:  Table [dbo].[CupTracker]    Script Date: 11/08/2016 23:16:30 ******/
DROP TABLE [dbo].[CupTracker]
GO
/****** Object:  User [BeerPongApi]    Script Date: 11/08/2016 23:16:30 ******/
DROP USER [BeerPongApi]
GO
USE [master]
GO
/****** Object:  Database [BeerPongFederation]    Script Date: 11/08/2016 23:16:30 ******/
DROP DATABASE [BeerPongFederation]
GO
/****** Object:  Database [BeerPongFederation]    Script Date: 11/08/2016 23:16:30 ******/
CREATE DATABASE [BeerPongFederation]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BeerPongFederation', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\BeerPongFederation.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BeerPongFederation_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\BeerPongFederation_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [BeerPongFederation] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BeerPongFederation].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BeerPongFederation] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BeerPongFederation] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BeerPongFederation] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BeerPongFederation] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BeerPongFederation] SET ARITHABORT OFF 
GO
ALTER DATABASE [BeerPongFederation] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BeerPongFederation] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BeerPongFederation] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BeerPongFederation] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BeerPongFederation] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BeerPongFederation] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BeerPongFederation] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BeerPongFederation] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BeerPongFederation] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BeerPongFederation] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BeerPongFederation] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BeerPongFederation] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BeerPongFederation] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BeerPongFederation] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BeerPongFederation] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BeerPongFederation] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BeerPongFederation] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BeerPongFederation] SET RECOVERY FULL 
GO
ALTER DATABASE [BeerPongFederation] SET  MULTI_USER 
GO
ALTER DATABASE [BeerPongFederation] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BeerPongFederation] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BeerPongFederation] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BeerPongFederation] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [BeerPongFederation] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'BeerPongFederation', N'ON'
GO
USE [BeerPongFederation]
GO
/****** Object:  User [BeerPongApi]    Script Date: 11/08/2016 23:16:30 ******/
CREATE USER [BeerPongApi] FOR LOGIN [BeerPongApi] WITH DEFAULT_SCHEMA=[[dbo]]]
GO
ALTER ROLE [db_owner] ADD MEMBER [BeerPongApi]
GO
/****** Object:  Table [dbo].[CupTracker]    Script Date: 11/08/2016 23:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CupTracker](
	[CupTrackerId] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[TeamId] [int] NOT NULL,
	[CupId] [int] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_CupTracker] PRIMARY KEY CLUSTERED 
(
	[CupTrackerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Game]    Script Date: 11/08/2016 23:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Game](
	[GameId] [int] IDENTITY(1,1) NOT NULL,
	[DateStarted] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdateSignature] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED 
(
	[GameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GameSetting]    Script Date: 11/08/2016 23:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameSetting](
	[GameSettingId] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[SettingId] [int] NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_GameSetting] PRIMARY KEY CLUSTERED 
(
	[GameSettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Player]    Script Date: 11/08/2016 23:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Player](
	[PlayerId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[FacebookId] [nvarchar](50) NULL,
 CONSTRAINT [PK_Player] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PlayerGame]    Script Date: 11/08/2016 23:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerGame](
	[PlayerGameId] [int] IDENTITY(1,1) NOT NULL,
	[PlayerId] [int] NOT NULL,
	[GameId] [int] NOT NULL,
	[TeamId] [int] NOT NULL,
 CONSTRAINT [PK_PlayerGame] PRIMARY KEY CLUSTERED 
(
	[PlayerGameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Setting]    Script Date: 11/08/2016 23:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting](
	[SettingId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Type] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[SettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NameSearch]    Script Date: 11/08/2016 23:16:31 ******/
CREATE NONCLUSTERED INDEX [NameSearch] ON [dbo].[Player]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CupTracker]  WITH CHECK ADD  CONSTRAINT [FK_CupTracker_Game] FOREIGN KEY([GameId])
REFERENCES [dbo].[Game] ([GameId])
GO
ALTER TABLE [dbo].[CupTracker] CHECK CONSTRAINT [FK_CupTracker_Game]
GO
ALTER TABLE [dbo].[GameSetting]  WITH CHECK ADD  CONSTRAINT [FK_GameSetting_Game] FOREIGN KEY([GameId])
REFERENCES [dbo].[Game] ([GameId])
GO
ALTER TABLE [dbo].[GameSetting] CHECK CONSTRAINT [FK_GameSetting_Game]
GO
ALTER TABLE [dbo].[GameSetting]  WITH CHECK ADD  CONSTRAINT [FK_GameSetting_Setting] FOREIGN KEY([SettingId])
REFERENCES [dbo].[Setting] ([SettingId])
GO
ALTER TABLE [dbo].[GameSetting] CHECK CONSTRAINT [FK_GameSetting_Setting]
GO
ALTER TABLE [dbo].[PlayerGame]  WITH CHECK ADD  CONSTRAINT [FK_PlayerGame_Game] FOREIGN KEY([GameId])
REFERENCES [dbo].[Game] ([GameId])
GO
ALTER TABLE [dbo].[PlayerGame] CHECK CONSTRAINT [FK_PlayerGame_Game]
GO
ALTER TABLE [dbo].[PlayerGame]  WITH CHECK ADD  CONSTRAINT [FK_PlayerGame_Player] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Player] ([PlayerId])
GO
ALTER TABLE [dbo].[PlayerGame] CHECK CONSTRAINT [FK_PlayerGame_Player]
GO
/****** Object:  StoredProcedure [dbo].[GetAvailableGames]    Script Date: 11/08/2016 23:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jonny Miles
-- Create date: 11/08/2016
-- Description:	Shows what games are available for viewing
-- EXEC GetAvailableGames @StartDate ='2016-08-10 00:00:00.000'
-- =============================================
CREATE PROCEDURE [dbo].[GetAvailableGames] 
	@StartDate Date
AS
BEGIN
	SELECT g.[GameId]
      ,g.[DateStarted]
	  ,pg.TeamId
	  ,p.Name
  FROM [BeerPongFederation].[dbo].[Game] g
  RIGHT JOIN [BeerPongFederation].[dbo].[PlayerGame] pg ON pg.GameId = g.GameId
  LEFT JOIN [BeerPongFederation].[dbo].[Player] p ON p.PlayerId = pg.PlayerId
  WHERE g.[DateStarted] > @StartDate AND g.GameId IN (SELECT TOP 4 [GameId] FROM [BeerPongFederation].[dbo].[Game] ORDER BY DateStarted DESC)
  ORDER BY g.[DateStarted] DESC
END

GO
/****** Object:  StoredProcedure [dbo].[GetLastUpdateSignature]    Script Date: 11/08/2016 23:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jonny Miles
-- Create date: 11/08//2016
-- Description:	Gets the last update signature of a game
-- EXEC GetLastUpdateSignature @GameId = 1
-- =============================================
CREATE PROCEDURE [dbo].[GetLastUpdateSignature] 
	@GameId int
AS
BEGIN
	SELECT [LastUpdateSignature]
  FROM [BeerPongFederation].[dbo].[Game]
  WHERE GameId = @GameId
END

GO
/****** Object:  StoredProcedure [dbo].[PlayerNameSearch]    Script Date: 11/08/2016 23:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jonny Miles
-- Create date: 10/08/2016
-- Description:	Player Name Search
-- EXEC PlayerNameSearch @Query = 'jon%mi%'
-- =============================================
CREATE PROCEDURE [dbo].[PlayerNameSearch] 
	@Query nvarchar(250)
AS
BEGIN
	SELECT [PlayerId]
      ,[Name]
      ,[FacebookId]
  FROM [BeerPongFederation].[dbo].[Player]
  WHERE Name LIKE @Query
END

GO
USE [master]
GO
ALTER DATABASE [BeerPongFederation] SET  READ_WRITE 
GO
