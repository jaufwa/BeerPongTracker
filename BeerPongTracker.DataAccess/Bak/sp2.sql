USE [BeerPongFederation]
GO
/****** Object:  StoredProcedure [dbo].[PlayerNameSearch]    Script Date: 11/08/2016 22:25:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jonny Miles
-- Create date: 10/08/2016
-- Description:	Player Name Search
-- EXEC PlayerNameSearch @Query = 'jon%mi%'
-- =============================================
CREATE PROCEDURE [dbo].[PlayerNameSearch] 
	@Query nvarchar(250)
AS
BEGIN
	SELECT [PlayerId]
      ,[Name]
      ,[FacebookId]
  FROM [BeerPongFederation].[dbo].[Player]
  WHERE Name LIKE @Query
END
