﻿CREATE PROCEDURE [dbo].[RETRIEVE_LatestProductId]
AS
	SELECT TOP 1 Id 
	FROM Products
	ORDER BY DateAdded DESC
RETURN 0