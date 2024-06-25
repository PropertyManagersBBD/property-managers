CREATE TABLE [Properties] (
  [Id] bigint IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [OwnerId] bigint NULL,
  [Capacity] int NOT NULL
);

CREATE TABLE [SaleContracts] (
  [Id] bigint IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [PropertyId] bigint NOT NULL,
  [BuyerId] bigint NOT NULL,
  [SellerId] bigint NOT NULL,
  [Price] money NOT NULL,
  CONSTRAINT [FK_SaleContracts_Properties] FOREIGN KEY ([PropertyId]) REFERENCES [Properties]([Id])
);

CREATE TABLE [RentalContracts] (
  [Id] bigint IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [PropertyId] bigint NOT NULL,
  [LandlordId] bigint NOT NULL,
  [TenantId] bigint NOT NULL,
  [Rent] money NOT NULL,
  [IsActive] bit NOT NULL,
  CONSTRAINT [FK_RentalContracts_Properties] FOREIGN KEY ([PropertyId]) REFERENCES [Properties]([Id])
);

CREATE TABLE [Mortgages] (
  [Id] bigint IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [PropertyId] bigint NOT NULL,
  [IsActive] bit NOT NULL,
  CONSTRAINT [FK_Mortgages_Properties] FOREIGN KEY ([PropertyId]) REFERENCES [Properties]([Id])
);
