CREATE PROCEDURE [dbo].[RETRIEVE_All_Products]
AS
	SELECT Id, Title, Price, DateAdded, Description, DiscountPrice, Enabled
	FROM Products
RETURN 0
