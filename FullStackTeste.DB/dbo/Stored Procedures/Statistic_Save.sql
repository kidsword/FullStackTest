CREATE PROC Statistic_Save (
	@IP VARCHAR(20),
	@Page VARCHAR(100) )
AS
BEGIN

	DECLARE @id INT

	INSERT INTO Statistic ([IP], [Page], [DateCreated])
	VALUES (@IP, @PAGE, GETDATE())

	SELECT @id = Scope_Identity()

	SELECT @id
END