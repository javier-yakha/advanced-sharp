CREATE TABLE [dbo].[ReturnProductForms]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [ProductId] UNIQUEIDENTIFIER NOT NULL, 
    [Used] BIT NULL, 
    [DamagedOnArrival] BIT NULL, 
    [Working] BIT NULL, 
    [CausedDamage] BIT NULL, 
    [Complaint] NVARCHAR(255) NOT NULL, 
    [DateOrdered] DATETIME NOT NULL, 
    [ProductArrived] BIT NOT NULL, 
    [DesiredSolution] INT NOT NULL, 
    [DateReceived] DATETIME NULL, 
    CONSTRAINT [FK_ReturnProductForms_Products_Id] FOREIGN KEY (ProductId) REFERENCES Products(Id)
)
