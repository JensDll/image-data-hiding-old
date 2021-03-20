SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[spRefreshTokens_Insert]
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
