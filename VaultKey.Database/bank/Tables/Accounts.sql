CREATE TABLE [bank].[Accounts] (
    [Id]                      UNIQUEIDENTIFIER CONSTRAINT [DF_Accounts_Id] DEFAULT (newsequentialid()) NOT NULL,
    [CustomerId]              UNIQUEIDENTIFIER NOT NULL,
    [AccountNumberEnc]        VARBINARY (256)  NOT NULL,
    [AccountNumberBlindIndex] BINARY (32)      NOT NULL,
    [AccountLast4]            CHAR (4)         NOT NULL,
    [AccountType]             NVARCHAR (20)    NOT NULL,
    [Balance]                 DECIMAL (18, 2)  CONSTRAINT [DF_Accounts_Balance] DEFAULT ((0)) NOT NULL,
    [Currency]                CHAR (3)         CONSTRAINT [DF_Accounts_Currency] DEFAULT ('INR') NOT NULL,
    [KeyVersion]              INT              CONSTRAINT [DF_Accounts_KeyVersion] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Accounts_Type] CHECK ([AccountType]=N'FixedDeposit' OR [AccountType]=N'Current' OR [AccountType]=N'Savings'),
    CONSTRAINT [FK_Accounts_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [bank].[Customers] ([Id]),
    CONSTRAINT [UQ_Accounts_AccountNumberBlindIndex] UNIQUE NONCLUSTERED ([AccountNumberBlindIndex] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Accounts_CustomerId]
    ON [bank].[Accounts]([CustomerId] ASC);

