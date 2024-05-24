CREATE PROCEDURE [dbo].[DELETE_Product_ByTitle]
	@Title nvarchar(50)
AS
	DECLARE @DeleteId uniqueidentifier
	SET @DeleteId = (SELECT Id FROM Products WHERE Title = @Title)

	DELETE Inventories
	WHERE ProductId = @DeleteId

	DELETE Products
	WHERE Id = @DeleteId
RETURN 0
