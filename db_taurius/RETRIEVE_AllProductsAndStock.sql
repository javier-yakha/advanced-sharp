CREATE PROCEDURE [dbo].[RETRIEVE_AllProductsAndStock]
	
AS
	SELECT Title, Price, DateAdded, Description, DiscountPrice, Enabled, Inventories.TotalStock
	FROM Products
	JOIN Inventories ON Products.Id = Inventories.ProductId
RETURN 0
