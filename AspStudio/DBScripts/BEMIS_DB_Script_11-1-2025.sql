USE [master]
GO

/****** Object:  Database [BEMIS]    Script Date: 1/11/2025 2:20:18 PM ******/
CREATE DATABASE [BEMIS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BEMIS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\BEMIS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BEMIS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\BEMIS_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BEMIS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [BEMIS] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [BEMIS] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [BEMIS] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [BEMIS] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [BEMIS] SET ARITHABORT OFF 
GO

ALTER DATABASE [BEMIS] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [BEMIS] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [BEMIS] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [BEMIS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [BEMIS] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [BEMIS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [BEMIS] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [BEMIS] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [BEMIS] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [BEMIS] SET  ENABLE_BROKER 
GO

ALTER DATABASE [BEMIS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [BEMIS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [BEMIS] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [BEMIS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [BEMIS] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [BEMIS] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [BEMIS] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [BEMIS] SET RECOVERY FULL 
GO

ALTER DATABASE [BEMIS] SET  MULTI_USER 
GO

ALTER DATABASE [BEMIS] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [BEMIS] SET DB_CHAINING OFF 
GO

ALTER DATABASE [BEMIS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [BEMIS] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [BEMIS] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [BEMIS] SET QUERY_STORE = OFF
GO

ALTER DATABASE [BEMIS] SET  READ_WRITE 
GO

