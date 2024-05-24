CREATE PROCEDURE [dbo].[RETRIEVE_AllProducts]
AS
	SELECT Id, Title, Price, DateAdded, Description, DiscountPrice, Enabled
	FROM Products
RETURN 0
