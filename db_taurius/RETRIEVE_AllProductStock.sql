CREATE PROCEDURE [dbo].[RETRIEVE_AllProductStock]
AS
	SELECT Products.Title, Inventories.TotalStock, Inventories.LastUpdated 
	FROM Inventories
	JOIN Products
	ON Inventories.ProductId = Products.Id
RETURN 0
