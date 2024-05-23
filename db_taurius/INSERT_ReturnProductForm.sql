CREATE PROCEDURE [dbo].[INSERT_ReturnProductForm]
	@ProductId uniqueidentifier,
	@Used bit NULL,
	@DamagedOnArrival bit NULL,
	@Working bit NULL,
	@CausedDamage bit NULL,
	@Complaint nvarchar(50),
	@DateOrdered datetime,
	@ProductArrived bit,
	@DesiredSolution nvarchar(50),
	@DateReceived datetime NULL
AS
	INSERT INTO ReturnProductForms
		(Id, ProductId, Used, DamagedOnArrival,
		Working, CausedDamage, Complaint,
		DateOrdered, ProductArrived,
		DesiredSolution, DateReceived)
	VALUES (NEWID(), @ProductId, @Used, @DamagedOnArrival,
		@Working, @CausedDamage, @Complaint,
		@DateOrdered, @ProductArrived,
		@DesiredSolution, @DateReceived)
RETURN 0
