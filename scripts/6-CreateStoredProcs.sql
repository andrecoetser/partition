
--if for one day as called from dbo.BulkLoad takes more thana few milliseconds, drop and re-create or alter proc to recompile/rebind. 
--when run from the .net load program using 4 threads, the loading of a year into the flat files should take less than a minute.
--the load from the flat files into the destination tables will take a few minutes per year
--18,250,000 rows are loaded per year
CREATE PROCEDURE dbo.InsertTransactionsAssociative @FromDate datetime, @ToDate datetime
WITH NATIVE_COMPILATION, SCHEMABINDING
AS
BEGIN ATOMIC WITH
(
	TRANSACTION ISOLATION LEVEL = SNAPSHOT,
	LANGUAGE = N'English'
)	
	DECLARE @Counter int = 0
	DECLARE @return varchar(max) = ''

	WHILE @Counter < 500
	BEGIN
	
		DECLARE @Seconds INT = DATEDIFF(SECOND, @FromDate, @ToDate)
		DECLARE @Random INT = ROUND(((@Seconds-1) * RAND()), 0)		
		
		DECLARE @aId UNIQUEIDENTIFIER = NEWID()
		DECLARE @tId UNIQUEIDENTIFIER = NEWID()
		DECLARE @pId UNIQUEIDENTIFIER
		DECLARE @sId UNIQUEIDENTIFIER
		
		SELECT
			@pId = [Guid]
		FROM
			dbo.Products_InMemory
		WHERE 
			Build_Key =  1 + ROUND(((500000) * RAND()), 0)
	
		SELECT
			@sId = [Guid]
		FROM
			dbo.Stores_InMemory
		WHERE 
			Build_Key =  1 + ROUND(((200000) * RAND()), 0)		

		SET @Counter += 1;

		SET @return = @return + CAST(@tId as varchar(36)) + ',' + CONVERT(VARCHAR(19), DATEADD(SECOND, @Random, @FromDate), 120) + ',' + CAST(ROUND(((1000) * RAND()), 2) as varchar(10))
		+ '|' + CAST(@aId as varchar(36)) + ',' + CAST(@tId as varchar(36)) + ',' + CAST(@pId as varchar(36)) + ',' + CAST(@sId as varchar(36)) + '#'

	END

	select @return
	
END
GO


CREATE PROCEDURE dbo.BulkLoad @FromDate datetime
AS
BEGIN
		
		DECLARE @result table
		(
			StringOutput varchar(200)
		)

		DECLARE @ToDate datetime = DATEADD(DAY, 1, @FromDate)

		
		exec dbo.InsertTransactionsAssociative @FromDate, @ToDate		

	
END
GO

CREATE PROCEDURE dbo.BulkInsert @FromDate datetime, @ToDate datetime
AS
BEGIN
	WHILE @FromDate < @ToDate
	BEGIN
		declare @sql varchar(max)
		set @sql = 'BULK INSERT dbo.transactions FROM ''c:\data\transaction\' + CONVERT(VARCHAR(8), @FromDate, 112) +'.txt'' WITH (FIELDTERMINATOR ='','',ROWTERMINATOR =''\n'',TABLOCK)' 
		exec (@sql)

		set @sql = 'BULK INSERT dbo.associative FROM ''c:\data\associative\' + CONVERT(VARCHAR(8), @FromDate, 112) +'.txt'' WITH (FIELDTERMINATOR ='','',ROWTERMINATOR =''\n'',TABLOCK)' 
		exec (@sql)	

		SET @FromDate = DATEADD(DAY, 1, @FromDate)
	END
END
GO


CREATE PROCEDURE dbo.[TimeRangeQuery] 
	@DateType VARCHAR(100), 
	@FromDate VARCHAR(100),
	@ToDate VARCHAR(100),
	@DimensionType VARCHAR(100),
	@DimensionTypeValue INT,	
	@DataPointType VARCHAR(100),
	@AggregateType  VARCHAR(100)
AS
BEGIN
	DECLARE @DimensionSelector VARCHAR(100) = ''

	IF @DimensionTypeValue > 0
		SET @DimensionSelector = @DimensionType + 'Name = '''+ @DimensionType +' Number ' + LTRIM(STR(@DimensionTypeValue)) + ''' AND '

	DECLARE @sql varchar(max) = 
	'
	SELECT 
		'+ @DateType + '([Timestamp]) AS [DateType],
		'+ @AggregateType + '(' + @DataPointType + ') AS [DataPoint]
	FROM
		dbo.[Cube]
	WHERE
		'+ @DimensionSelector +
		'[Timestamp] >= '''+ @FromDate + ''' AND [Timestamp] < ''' + @ToDate + '''
	GROUP BY
		'+ @DateType + '([Timestamp])
	'

	EXEC(@sql)
END
GO

CREATE PROCEDURE dbo.[ProductStoreQuery] 	
	@FromDate VARCHAR(100),
	@ToDate VARCHAR(100),
	@DimensionType VARCHAR(100),
	@DimensionTypeValue INT,
	@DataPointType VARCHAR(100),
	@AggregateType  VARCHAR(100),	
	@DataOrder VARCHAR(100)
AS
BEGIN
	DECLARE @DimensionSelector VARCHAR(100) = ''
	DECLARE @RangeDimension VARCHAR(100) = 'Product' 

	IF @DimensionTypeValue > 0
		SET @DimensionSelector = @DimensionType + 'Name = '''+ @DimensionType +' Number ' + LTRIM(STR(@DimensionTypeValue)) + ''' AND '

	IF @DimensionType = 'Product'
		SET @RangeDimension = 'Store'

	DECLARE @sql varchar(max) = 
	'
	SELECT TOP(10) 
		' + @RangeDimension + 'Name AS [Dimension],
		' + @AggregateType + '(' + @DataPointType + ') AS [DataPoint]
	FROM
		dbo.[Cube]
	WHERE
		'+ @DimensionSelector +
		'[Timestamp] >= '''+ @FromDate + ''' AND [Timestamp] < ''' + @ToDate + '''
	GROUP BY
		' + @RangeDimension + 'Name
	ORDER BY
		' + @AggregateType + '(' + @DataPointType + ')' + @DataOrder


	EXEC(@sql)
END
GO


CREATE PROCEDURE dbo.InsertStoresProducts
WITH NATIVE_COMPILATION, SCHEMABINDING
AS
BEGIN ATOMIC WITH
(
	TRANSACTION ISOLATION LEVEL = SNAPSHOT,
	LANGUAGE = N'English'
)

	DECLARE @Counter int = 0;
	WHILE @Counter < 500000
	BEGIN		 
		INSERT [dbo].[Products_InMemory]
		([Description], [Build_Key], [CostPrice])			
		VALUES
			('Product number ' + CAST(@Counter AS VARCHAR(10)), @Counter, ROUND(((1000) * RAND()), 2))

		IF (@Counter < 200000)
			INSERT [dbo].[Stores_InMemory]
			([Description], [Build_Key])			
			VALUES
				('Store number ' + CAST(@Counter AS VARCHAR(10)), @Counter)
		SET @Counter += 1;
	END

	RETURN 0
END
GO
