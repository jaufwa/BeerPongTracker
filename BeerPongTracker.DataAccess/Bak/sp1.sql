USE [BeerPongFederation]
GO
/****** Object:  StoredProcedure [dbo].[GetAvailableGames]    Script Date: 11/08/2016 22:30:09 ******/
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
