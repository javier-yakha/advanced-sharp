CREATE PROCEDURE [dbo].[DELETE_Product]
	@Title nvarchar(50)
	
AS

	DELETE Inventories
	WHERE ProductId = (SELECT Id FROM Products WHERE Title = @Title)

	DELETE Products
	WHERE Title = @Title
RETURN 0
