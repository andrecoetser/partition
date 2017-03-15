--This table will hold date attributes for the time dimension - keeping it simple with just three fields.
CREATE TABLE dbo.Dates 
(
	[DateKey] date primary key
)

;WITH DateGenerator AS 
(
	SELECT CAST('2009-01-01' AS DATE) AS CalenderDate
	UNION ALL
	SELECT DATEADD(DAY, 1, CalenderDate)
	FROM
		DateGenerator
	WHERE 
		 DATEADD(DAY, 1, CalenderDate) < CAST('2017-01-01' AS DATE)
)
INSERT INTO dbo.Dates
SELECT
	CalenderDate
FROM
	DateGenerator
OPTION (MAXRECURSION 3000)