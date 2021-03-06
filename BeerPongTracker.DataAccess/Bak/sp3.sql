USE [BeerPongFederation]
GO
/****** Object:  StoredProcedure [dbo].[GetLastUpdateSignature]    Script Date: 11/08/2016 22:26:03 ******/
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
