USE [master]
GO
/****** Object:  Database [image_db]    Script Date: 02/04/2021 00:55:34 ******/
CREATE DATABASE [image_db]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'image_db', FILENAME = N'/var/opt/mssql/data/image_db.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'image_db_log', FILENAME = N'/var/opt/mssql/data/image_db_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [image_db] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [image_db].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [image_db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [image_db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [image_db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [image_db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [image_db] SET ARITHABORT OFF 
GO
ALTER DATABASE [image_db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [image_db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [image_db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [image_db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [image_db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [image_db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [image_db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [image_db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [image_db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [image_db] SET  DISABLE_BROKER 
GO
ALTER DATABASE [image_db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [image_db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [image_db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [image_db] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [image_db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [image_db] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [image_db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [image_db] SET RECOVERY FULL 
GO
ALTER DATABASE [image_db] SET  MULTI_USER 
GO
ALTER DATABASE [image_db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [image_db] SET DB_CHAINING OFF 
GO
ALTER DATABASE [image_db] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [image_db] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [image_db] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [image_db] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'image_db', N'ON'
GO
ALTER DATABASE [image_db] SET QUERY_STORE = OFF
GO
USE [image_db]
GO
/****** Object:  Table [dbo].[RefreshTokens]    Script Date: 02/04/2021 00:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshTokens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Token] [nvarchar](max) NOT NULL,
	[JwtId] [nvarchar](max) NOT NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[ExpiryDate] [datetime2](7) NOT NULL,
	[Used] [bit] NOT NULL,
	[Invalidated] [bit] NOT NULL,
 CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 02/04/2021 00:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[RegistrationDate] [datetime2](7) NOT NULL,
	[DeletionDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[RefreshTokens]  WITH CHECK ADD  CONSTRAINT [FK_RefreshTokens-UserId_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RefreshTokens] CHECK CONSTRAINT [FK_RefreshTokens-UserId_Users]
GO
/****** Object:  StoredProcedure [dbo].[spRefreshTokens_DeleteForUser]    Script Date: 02/04/2021 00:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spRefreshTokens_DeleteForUser]
  @userId INT
AS
BEGIN

  DELETE from dbo.RefreshTokens
  WHERE UserId = @userId

END
GO
/****** Object:  StoredProcedure [dbo].[spRefreshTokens_GetByToken]    Script Date: 02/04/2021 00:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spRefreshTokens_GetByToken]
  @token NVARCHAR(MAX)
AS
BEGIN

  SELECT *
  FROM dbo.RefreshTokens
  WHERE Token = @token
  FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;

END
GO
/****** Object:  StoredProcedure [dbo].[spRefreshTokens_Insert]    Script Date: 02/04/2021 00:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spRefreshTokens_Insert]
  @userId INT,
  @jwtId NVARCHAR(MAX),
  @creationDate DATETIME2(7),
  @expiryDate DATETIME2(7)
AS
BEGIN

  INSERT INTO dbo.RefreshTokens (
    UserId,
    Token,
    JwtId,
    CreationDate,
    ExpiryDate,
    Used,
    Invalidated
  )
  OUTPUT
    inserted.Token
  VALUES (
    @userId,
    NEWID(),
    @jwtId,
    @creationDate,
    @expiryDate,
    0,
    0
  );

END
GO
/****** Object:  StoredProcedure [dbo].[spRefreshTokens_SetUsed]    Script Date: 02/04/2021 00:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spRefreshTokens_SetUsed]
  @id INT
AS
BEGIN

  UPDATE dbo.RefreshTokens
  SET Used = 1
  WHERE Id = @id;

END
GO
/****** Object:  StoredProcedure [dbo].[spUsers_Delete]    Script Date: 02/04/2021 00:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      PROCEDURE [dbo].[spUsers_Delete]
  @id INT
AS
BEGIN
  DELETE FROM dbo.Users
  WHERE Id = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[spUsers_GetAll]    Script Date: 02/04/2021 00:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spUsers_GetAll]
  @skip INT,
  @take INT,
  @total INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT
    Id,
    Username,
    RegistrationDate,
    DeletionDate
  FROM dbo.Users
  ORDER BY Id
    OFFSET @skip ROWS
    FETCH NEXT @take ROWS ONLY
  FOR JSON PATH;

  SELECT @total = COUNT(*)
  FROM dbo.Users;
END
GO
/****** Object:  StoredProcedure [dbo].[spUsers_GetById]    Script Date: 02/04/2021 00:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spUsers_GetById]
  @id INT
AS
BEGIN

  SELECT *
  FROM dbo.Users
  WHERE Id = @id
  FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;

END
GO
/****** Object:  StoredProcedure [dbo].[spUsers_GetByName]    Script Date: 02/04/2021 00:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROCEDURE [dbo].[spUsers_GetByName]
  @username NVARCHAR(100)
AS
BEGIN
  SELECT *
  FROM dbo.Users
  WHERE Username = @username
  FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;
  END
GO
/****** Object:  StoredProcedure [dbo].[spUsers_Insert]    Script Date: 02/04/2021 00:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spUsers_Insert]
  @username NVARCHAR(100),
  @password NVARCHAR(MAX),
  @registrationDate DATETIME2(7),
  @deletionDate DATETIME2(7)
AS
BEGIN
  SET NOCOUNT ON;

  INSERT INTO dbo.Users
  OUTPUT
  inserted.Id
  VALUES
    (@username, @password, @registrationDate, @deletionDate);

END
GO
USE [master]
GO
ALTER DATABASE [image_db] SET  READ_WRITE 
GO
