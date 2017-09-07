USE [master]
GO

/****** Object:  Database [Results]    Script Date: 9/5/2017 2:30:54 PM ******/
DROP DATABASE IF EXISTS [Results]
GO

/****** Object:  Database [Results]    Script Date: 9/5/2017 2:30:54 PM ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Results')
BEGIN
CREATE DATABASE [Results]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Results', FILENAME = N'%userprofile%\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\Results.mdf' , SIZE = 204800KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Results_log', FILENAME = N'%userprofile%\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\Results.ldf' , SIZE = 139264KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
END

GO

ALTER DATABASE [Results] SET COMPATIBILITY_LEVEL = 130
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Results].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Results] SET ANSI_NULL_DEFAULT ON 
GO

ALTER DATABASE [Results] SET ANSI_NULLS ON 
GO

ALTER DATABASE [Results] SET ANSI_PADDING ON 
GO

ALTER DATABASE [Results] SET ANSI_WARNINGS ON 
GO

ALTER DATABASE [Results] SET ARITHABORT ON 
GO

ALTER DATABASE [Results] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [Results] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [Results] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [Results] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [Results] SET CURSOR_DEFAULT  LOCAL 
GO

ALTER DATABASE [Results] SET CONCAT_NULL_YIELDS_NULL ON 
GO

ALTER DATABASE [Results] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [Results] SET QUOTED_IDENTIFIER ON 
GO

ALTER DATABASE [Results] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [Results] SET  DISABLE_BROKER 
GO

ALTER DATABASE [Results] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [Results] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [Results] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [Results] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [Results] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [Results] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [Results] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [Results] SET RECOVERY FULL 
GO

ALTER DATABASE [Results] SET  MULTI_USER 
GO

ALTER DATABASE [Results] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Results] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Results] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Results] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [Results] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Results] SET QUERY_STORE = OFF
GO

USE [Results]
GO

ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO

ALTER DATABASE [Results] SET  READ_WRITE 
GO




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Run]') AND type in (N'U'))
ALTER TABLE [dbo].[Run] DROP CONSTRAINT IF EXISTS [FK_Run_App]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Spec]') AND type in (N'U'))
ALTER TABLE [dbo].[Spec] DROP CONSTRAINT IF EXISTS [FK_Spec_App]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RunResult]') AND type in (N'U'))
ALTER TABLE [dbo].[RunResult] DROP CONSTRAINT IF EXISTS [FK_RunResult_Run]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RunSpec]') AND type in (N'U'))
ALTER TABLE [dbo].[RunSpec] DROP CONSTRAINT IF EXISTS [FK_RunSpec_Spec]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RunSpec]') AND type in (N'U'))
ALTER TABLE [dbo].[RunSpec] DROP CONSTRAINT IF EXISTS [FK_RunSpec_Run]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Run]') AND type in (N'U'))
ALTER TABLE [dbo].[Run] DROP CONSTRAINT IF EXISTS [DF__Run__RunDateTime__245D67DE]
GO

/****** Object:  Index [UIX_RunResult]    Script Date: 9/5/2017 2:29:54 PM ******/
DROP INDEX IF EXISTS [UIX_RunResult] ON [dbo].[RunResult]
GO

/****** Object:  Table [dbo].[App]    Script Date: 9/5/2017 2:29:54 PM ******/
DROP TABLE IF EXISTS [dbo].[App]
GO

/****** Object:  Table [dbo].[Run]    Script Date: 9/5/2017 2:29:54 PM ******/
DROP TABLE IF EXISTS [dbo].[Run]
GO

/****** Object:  Table [dbo].[Spec]    Script Date: 9/5/2017 2:29:54 PM ******/
DROP TABLE IF EXISTS [dbo].[Spec]
GO

/****** Object:  Table [dbo].[RunResult]    Script Date: 9/5/2017 2:29:54 PM ******/
DROP TABLE IF EXISTS [dbo].[RunResult]
GO

/****** Object:  Table [dbo].[RunSpec]    Script Date: 9/5/2017 2:29:54 PM ******/
DROP TABLE IF EXISTS [dbo].[RunSpec]
GO

/****** Object:  Table [dbo].[RunSpec]    Script Date: 9/5/2017 2:29:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RunSpec]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RunSpec](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RunId] [int] NOT NULL,
	[SpecId] [int] NOT NULL,
	[Passed] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[RunResult]    Script Date: 9/5/2017 2:29:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RunResult]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RunResult](
	[RunId] [int] NOT NULL,
	[HtmlResults] [varchar](max) NOT NULL,
	[Passed] [bit] NOT NULL,
 CONSTRAINT [PK_RunResult] PRIMARY KEY CLUSTERED 
(
	[RunId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[Spec]    Script Date: 9/5/2017 2:29:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Spec]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Spec](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](256) NOT NULL,
	[StoryTellerId] [uniqueidentifier] NOT NULL,
	[AppId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_AppId_StoryTellerId] UNIQUE NONCLUSTERED 
(
	[AppId] ASC,
	[StoryTellerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[Run]    Script Date: 9/5/2017 2:29:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Run]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Run](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RunDateTime] [datetime2](7) NOT NULL,
	[Name] [varchar](50) NULL,
	[AppId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[App]    Script Date: 9/5/2017 2:29:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[App]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[App](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ApiKey] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Index [UIX_RunResult]    Script Date: 9/5/2017 2:29:54 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RunResult]') AND name = N'UIX_RunResult')
CREATE UNIQUE NONCLUSTERED INDEX [UIX_RunResult] ON [dbo].[RunResult]
(
	[RunId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Run__RunDateTime__245D67DE]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Run] ADD  DEFAULT (getutcdate()) FOR [RunDateTime]
END

GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RunSpec_Run]') AND parent_object_id = OBJECT_ID(N'[dbo].[RunSpec]'))
ALTER TABLE [dbo].[RunSpec]  WITH CHECK ADD  CONSTRAINT [FK_RunSpec_Run] FOREIGN KEY([RunId])
REFERENCES [dbo].[Run] ([Id])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RunSpec_Run]') AND parent_object_id = OBJECT_ID(N'[dbo].[RunSpec]'))
ALTER TABLE [dbo].[RunSpec] CHECK CONSTRAINT [FK_RunSpec_Run]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RunSpec_Spec]') AND parent_object_id = OBJECT_ID(N'[dbo].[RunSpec]'))
ALTER TABLE [dbo].[RunSpec]  WITH CHECK ADD  CONSTRAINT [FK_RunSpec_Spec] FOREIGN KEY([SpecId])
REFERENCES [dbo].[Spec] ([Id])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RunSpec_Spec]') AND parent_object_id = OBJECT_ID(N'[dbo].[RunSpec]'))
ALTER TABLE [dbo].[RunSpec] CHECK CONSTRAINT [FK_RunSpec_Spec]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RunResult_Run]') AND parent_object_id = OBJECT_ID(N'[dbo].[RunResult]'))
ALTER TABLE [dbo].[RunResult]  WITH CHECK ADD  CONSTRAINT [FK_RunResult_Run] FOREIGN KEY([RunId])
REFERENCES [dbo].[Run] ([Id])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RunResult_Run]') AND parent_object_id = OBJECT_ID(N'[dbo].[RunResult]'))
ALTER TABLE [dbo].[RunResult] CHECK CONSTRAINT [FK_RunResult_Run]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Spec_App]') AND parent_object_id = OBJECT_ID(N'[dbo].[Spec]'))
ALTER TABLE [dbo].[Spec]  WITH CHECK ADD  CONSTRAINT [FK_Spec_App] FOREIGN KEY([AppId])
REFERENCES [dbo].[App] ([Id])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Spec_App]') AND parent_object_id = OBJECT_ID(N'[dbo].[Spec]'))
ALTER TABLE [dbo].[Spec] CHECK CONSTRAINT [FK_Spec_App]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Run_App]') AND parent_object_id = OBJECT_ID(N'[dbo].[Run]'))
ALTER TABLE [dbo].[Run]  WITH CHECK ADD  CONSTRAINT [FK_Run_App] FOREIGN KEY([AppId])
REFERENCES [dbo].[App] ([Id])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Run_App]') AND parent_object_id = OBJECT_ID(N'[dbo].[Run]'))
ALTER TABLE [dbo].[Run] CHECK CONSTRAINT [FK_Run_App]
GO


