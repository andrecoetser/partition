
CREATE TABLE dbo.[Cube] (  
	[Guid] UNIQUEIDENTIFIER NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[SalePrice] [money] NOT NULL,
	[CostPrice] [money] NOT NULL,
	[StoreName] varchar(50) NOT NULL,
	[ProductName] varchar(50) NOT NULL
) ON [Primary]
GO 


INSERT dbo.[Cube] WITH (TABLOCK)
SELECT
	a.[Guid],
	t.[TimeStamp],
	t.[SalePrice],
	p.[CostPrice],
	s.[Description],
	p.[Description]
FROM
	dbo.Associative a INNER JOIN
	dbo.Transactions t ON a.[TransactionId] = t.[Guid] INNER JOIN	
	dbo.Products p ON a.[ProductId] = p.[Guid] INNER JOIN
	dbo.Stores s ON a.[StoreId] = s.[Guid]

GO

ALTER TABLE dbo.[Cube]
ADD CONSTRAINT PK_Date PRIMARY KEY NONCLUSTERED ([Timestamp],[Guid])
GO

CREATE CLUSTERED INDEX [ClusteredIndex_on_ByDateRange] ON [dbo].[Cube] ([TimeStamp]) 
WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [ByDateRange]([TimeStamp])
GO
