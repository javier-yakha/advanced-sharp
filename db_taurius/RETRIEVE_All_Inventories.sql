CREATE PROCEDURE [dbo].[RETRIEVE_All_Inventories]

AS
    SELECT Id, ProductId, LastUpdated, TotalStock
    FROM Inventories
RETURN 0