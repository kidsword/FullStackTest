CREATE PROC Statistic_Parameters_Save (
	@StatisticId INT,
	@Name VARCHAR(255),
	@Value VARCHAR(255) )
AS
BEGIN
	INSERT INTO Statistic_Parameters ([StatisticId], [Name], [Value] )
	VALUES (@StatisticId, @Name, @Value)
END