CREATE TABLE [dbo].[Products]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[Title] NVARCHAR(50) NOT NULL,
	[Price] MONEY NOT NULL,
	[DateAdded] DATETIME NOT NULL, 
    [Description] NVARCHAR(100) NOT NULL default 'Product', 
    [DiscountPrice] MONEY NOT NULL DEFAULT 0, 
    [Enabled] BIT NOT NULL DEFAULT 0
)
