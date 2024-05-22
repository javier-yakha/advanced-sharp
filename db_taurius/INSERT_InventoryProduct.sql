CREATE PROCEDURE [dbo].[INSERT_InventoryProduct]
	@ProductId uniqueidentifier
AS
	INSERT INTO Inventories (Id, ProductId, LastUpdated, TotalStock)
	VALUES (NEWID(), @ProductId, SYSUTCDATETIME(), 0)
RETURN 0
