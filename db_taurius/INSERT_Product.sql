CREATE PROCEDURE [dbo].[INSERT_Product]
	@Title nvarchar(50),
	@Price money,
	@Description nvarchar(100)
AS

	INSERT INTO Products 
		(Id, Title, Price, DateAdded, Description, DiscountPrice)
	VALUES 
		(NEWID(), @Title, @Price, SYSUTCDATETIME(), @Description, @Price)
RETURN 0
