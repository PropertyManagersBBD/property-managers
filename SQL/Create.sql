CREATE TABLE [Properties] (
  [Id] bigint IDENTITY(1,1) PRIMARY KEY,
  [OwnerId] bigint NULL,
  [Capacity] int
);

CREATE TABLE [SaleContracts] (
  [Id] bigint IDENTITY(1,1) PRIMARY KEY,
  [PropertyId] bigint,
  [BuyerId] bigint,
  [SellerId] bigint,
  [Price] money,
  CONSTRAINT [FK_SaleContracts_Properties] FOREIGN KEY ([PropertyId]) REFERENCES [Properties]([Id])
);

CREATE TABLE [RentalContracts] (
  [Id] bigint IDENTITY(1,1) PRIMARY KEY,
  [PropertyId] bigint,
  [LandlordId] bigint,
  [TenantId] bigint,
  [Rent] money,
  [IsActive] bit,
  CONSTRAINT [FK_RentalContracts_Properties] FOREIGN KEY ([PropertyId]) REFERENCES [Properties]([Id])
);

CREATE TABLE [Mortgages] (
  [Id] bigint IDENTITY(1,1) PRIMARY KEY,
  [PropertyId] bigint,
  [IsActive] bit,
  CONSTRAINT [FK_Mortgages_Properties] FOREIGN KEY ([PropertyId]) REFERENCES [Properties]([Id])
);
