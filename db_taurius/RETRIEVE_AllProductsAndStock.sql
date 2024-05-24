CREATE PROCEDURE [dbo].[RETRIEVE_AllProductsAndStock]
	
AS
	SELECT Products.Id, Title, Price, DateAdded, Description, DiscountPrice, Enabled, Inventories.TotalStock
	FROM Products
	JOIN Inventories ON Products.Id = Inventories.ProductId
RETURN 0
