SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[spUsers_Insert]
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
