CREATE PROCEDURE [dbo].[RETRIEVE_AllProducts]
	
AS
	SELECT Products.Id, Title, Price, DateAdded, Description, DiscountPrice, Enabled, Inventories.TotalStock
	FROM Products
	JOIN Inventories ON Products.Id = Inventories.ProductId
RETURN 0
