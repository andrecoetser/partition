

CREATE TABLE dbo.Transactions (  
	[Guid] UNIQUEIDENTIFIER PRIMARY KEY NONCLUSTERED,
	[Timestamp] [datetime] NOT NULL,
	[SalePrice] [money] NOT NULL
) ON [Primary]
GO 

CREATE TABLE [dbo].[Stores](
	[Guid] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY NONCLUSTERED,
	[Description] varchar(50) NOT NULL
) ON [Primary]
GO 

CREATE TABLE [dbo].[Stores_InMemory](
	[Guid] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY NONCLUSTERED,
	[Build_Key] int,
	[Description] varchar(50) NOT NULL,
	INDEX I_BK NONCLUSTERED ([Build_Key])
) 
WITH (MEMORY_OPTIMIZED = ON, DURABILITY = SCHEMA_AND_DATA)
GO

CREATE TABLE [dbo].[Products](
	[Guid] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY NONCLUSTERED,
	[Description] varchar(50) NOT NULL,
	[CostPrice] [money] NOT NULL
) ON [Primary]
GO 

CREATE TABLE [dbo].[Products_InMemory](
	[Guid] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY NONCLUSTERED,
	[Build_Key] int,
	[Description] varchar(50) NOT NULL,
	[CostPrice] [money] NOT NULL,
	INDEX I_BK NONCLUSTERED ([Build_Key])
) 
WITH (MEMORY_OPTIMIZED = ON, DURABILITY = SCHEMA_AND_DATA)
GO

CREATE TABLE [dbo].[Associative](
	[Guid] UNIQUEIDENTIFIER,
	[TransactionId] [uniqueidentifier] REFERENCES Transactions ([Guid])  NOT NULL,
	[ProductId] [uniqueidentifier] REFERENCES Products ([Guid]) NOT NULL,
	[StoreId] [uniqueidentifier] REFERENCES Stores ([Guid]) NOT NULL
) ON [Primary]
GO
