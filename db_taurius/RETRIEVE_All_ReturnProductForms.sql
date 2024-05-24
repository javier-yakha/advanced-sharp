CREATE PROCEDURE [dbo].[RETRIEVE_All_ReturnProductForms]
AS
	SELECT Id, ProductId, Used, DamagedOnArrival,
		Working, CausedDamage, Complaint,
		DateOrdered, ProductArrived,
		DesiredSolution, DateReceived
	FROM ReturnProductForms
RETURN 0
