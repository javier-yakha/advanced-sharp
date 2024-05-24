CREATE PROCEDURE [dbo].[RETRIEVE_All_ProductStock]
AS
	SELECT Products.Title, Inventories.TotalStock, Inventories.LastUpdated 
	FROM Inventories
	JOIN Products
	ON Inventories.ProductId = Products.Id
RETURN 0
