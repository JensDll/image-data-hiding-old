SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[spRefreshTokens_DeleteForUser]
  @userId INT
AS
BEGIN

  DELETE from dbo.RefreshTokens
  WHERE UserId = @userId

END
GO
