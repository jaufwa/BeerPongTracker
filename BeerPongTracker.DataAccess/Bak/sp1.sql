USE BeerPongFederation
GO
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
CREATE PROCEDURE GetAvailableGames 
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
  WHERE g.[DateStarted] > @StartDate
END
GO
