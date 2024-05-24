CREATE PROCEDURE [dbo].[RETRIEVE_ProductId_ByProductTitle]
	@Title nvarchar(50)
AS
	SELECT TOP 1 Id 
	FROM Products
	WHERE Title = @Title
RETURN 0
