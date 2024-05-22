CREATE PROCEDURE [dbo].[UPDATE_ProductStockById]
	@ProductId uniqueidentifier,
	@TotalStock int
AS
	UPDATE Inventories
	SET TotalStock = @TotalStock,
		LastUpdated = SYSUTCDATETIME()
	WHERE ProductId = @ProductId
RETURN 0
