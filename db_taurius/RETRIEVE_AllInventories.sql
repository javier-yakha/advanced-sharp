CREATE PROCEDURE [dbo].[RETRIEVE_AllInventories]

AS
    SELECT Id, ProductId, LastUpdated, TotalStock
    FROM Inventories
RETURN 0