CREATE TABLE [bank].[Customers] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_Customers_Id] DEFAULT (newsequentialid()) NOT NULL,
    [CustomerNumber] VARCHAR (20)     NOT NULL,
    [FullNameEnc]    VARBINARY (1024) NOT NULL,
    [DobEnc]         VARBINARY (128)  NOT NULL,
    [EmailEnc]       VARBINARY (1024) NOT NULL,
    [MobileEnc]      VARBINARY (256)  NULL,
    [KeyVersion]     INT              CONSTRAINT [DF_Customers_KeyVersion] DEFAULT ((1)) NOT NULL,
    [Status]         NVARCHAR (20)    CONSTRAINT [DF_Customers_Status] DEFAULT (N'Active') NOT NULL,
    [IsActivated]    BIT              CONSTRAINT [DF_Customers_IsActivated] DEFAULT ((0)) NOT NULL,
    [CreatedAt]      DATETIME2 (3)    CONSTRAINT [DF_Customers_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Customers_Status] CHECK ([Status]=N'Closed' OR [Status]=N'Blocked' OR [Status]=N'Active'),
    CONSTRAINT [UQ_Customers_CustomerNumber] UNIQUE NONCLUSTERED ([CustomerNumber] ASC)
);

