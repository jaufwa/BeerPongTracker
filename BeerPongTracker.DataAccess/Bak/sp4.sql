USE BeerPongFederation
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jonny Miles
-- Create date: 14/08/2016
-- Description:	Get winners
-- EXEC GetWinners @GameId = 3, @TeamId = 1
-- =============================================
CREATE PROCEDURE GetWinners
	@GameId int,
	@TeamId int
AS
BEGIN
SELECT p.Name
	  ,p.FacebookId
  FROM [BeerPongFederation].[dbo].[PlayerGame] pg
  LEFT JOIN [BeerPongFederation].[dbo].[Player] p ON p.PlayerId = pg.PlayerId
  WHERE GameId = @GameId AND TeamId = @TeamId
END
GO
