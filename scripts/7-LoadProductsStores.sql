

exec dbo.InsertStoresProducts

INSERT [dbo].[Products]
SELECT
	[Guid], [Description], [CostPrice]
FROM
	[dbo].[Products_InMemory]

INSERT [dbo].[Stores]
SELECT
	[Guid], [Description]
FROM
	[dbo].[Stores_InMemory]
