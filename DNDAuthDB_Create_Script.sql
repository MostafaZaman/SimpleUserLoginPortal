USE [master]
GO
/****** Object:  Database [DNDAuthDB]    Script Date: 11/1/2019 1:45:25 PM ******/
CREATE DATABASE [DNDAuthDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DNDAuthDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\DNDAuthDB.mdf' , SIZE = 3136KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DNDAuthDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\DNDAuthDB_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [DNDAuthDB] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DNDAuthDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DNDAuthDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DNDAuthDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DNDAuthDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DNDAuthDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DNDAuthDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [DNDAuthDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DNDAuthDB] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [DNDAuthDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DNDAuthDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DNDAuthDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DNDAuthDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DNDAuthDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DNDAuthDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DNDAuthDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DNDAuthDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DNDAuthDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DNDAuthDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DNDAuthDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DNDAuthDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DNDAuthDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DNDAuthDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DNDAuthDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [DNDAuthDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DNDAuthDB] SET RECOVERY FULL 
GO
ALTER DATABASE [DNDAuthDB] SET  MULTI_USER 
GO
ALTER DATABASE [DNDAuthDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DNDAuthDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DNDAuthDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DNDAuthDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'DNDAuthDB', N'ON'
GO
USE [DNDAuthDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/1/2019 1:45:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SecurityQuestions]    Script Date: 11/1/2019 1:45:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityQuestions](
	[SecurityQuesID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityQues] [nvarchar](max) NULL,
 CONSTRAINT [PK_SecurityQuestions] PRIMARY KEY CLUSTERED 
(
	[SecurityQuesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/1/2019 1:45:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[EmailID] [nvarchar](250) NULL,
	[Password] [nvarchar](50) NULL,
	[ConfirmPassword] [nvarchar](max) NULL,
	[DateOfBirth] [datetime] NULL,
	[ActivationCode] [uniqueidentifier] NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[SecurityQuesID] [int] NULL,
	[SecurityAnswer] [nvarchar](50) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20191029114810_CreateDNDAuthDB', N'2.0.3-rtm-10026')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20191031115540_a', N'2.0.3-rtm-10026')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20191031135405_b', N'2.0.3-rtm-10026')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20191101010803_c', N'2.0.3-rtm-10026')
GO
SET IDENTITY_INSERT [dbo].[SecurityQuestions] ON 

GO
INSERT [dbo].[SecurityQuestions] ([SecurityQuesID], [SecurityQues]) VALUES (1, N'What was your childhood nickname?')
GO
INSERT [dbo].[SecurityQuestions] ([SecurityQuesID], [SecurityQues]) VALUES (2, N'What school did you attend for sixth grade?')
GO
INSERT [dbo].[SecurityQuestions] ([SecurityQuesID], [SecurityQues]) VALUES (3, N'What is the name of your favorite childhood friend?')
GO
INSERT [dbo].[SecurityQuestions] ([SecurityQuesID], [SecurityQues]) VALUES (4, N'In what city or town did your mother and father meet?')
GO
INSERT [dbo].[SecurityQuestions] ([SecurityQuesID], [SecurityQues]) VALUES (5, N'What was the make and model of your first car?')
GO
INSERT [dbo].[SecurityQuestions] ([SecurityQuesID], [SecurityQues]) VALUES (6, N'In what city and country do you want to retire?')
GO
SET IDENTITY_INSERT [dbo].[SecurityQuestions] OFF
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_SecurityQuestions] FOREIGN KEY([SecurityQuesID])
REFERENCES [dbo].[SecurityQuestions] ([SecurityQuesID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_SecurityQuestions]
GO
USE [master]
GO
ALTER DATABASE [DNDAuthDB] SET  READ_WRITE 
GO
