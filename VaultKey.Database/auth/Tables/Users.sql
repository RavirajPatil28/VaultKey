CREATE TABLE [auth].[Users] (
    [Id]                    UNIQUEIDENTIFIER CONSTRAINT [DF_Users_Id] DEFAULT (newsequentialid()) NOT NULL,
    [CustomerId]            UNIQUEIDENTIFIER NOT NULL,
    [EmailBlindIndex]       BINARY (32)      NOT NULL,
    [EmailEnc]              VARBINARY (1024) NOT NULL,
    [PasswordHash]          NVARCHAR (255)   NOT NULL,
    [Status]                NVARCHAR (20)    CONSTRAINT [DF_Users_Status] DEFAULT (N'Active') NOT NULL,
    [TotpEnabled]           BIT              CONSTRAINT [DF_Users_TotpEnabled] DEFAULT ((0)) NOT NULL,
    [FailedAttempts]        INT              CONSTRAINT [DF_Users_FailedAttempts] DEFAULT ((0)) NOT NULL,
    [LockoutEnd]            DATETIME2 (3)    NULL,
    [PasswordStrengthScore] TINYINT          NOT NULL,
    [PasswordExpiresAt]     DATETIME2 (3)    NOT NULL,
    [PasswordChangedAt]     DATETIME2 (3)    NULL,
    [CreatedAt]             DATETIME2 (3)    CONSTRAINT [DF_Users_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]             DATETIME2 (3)    CONSTRAINT [DF_Users_UpdatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [RowVersion]            ROWVERSION       NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Users_PasswordStrengthScore] CHECK ([PasswordStrengthScore]>=(0) AND [PasswordStrengthScore]<=(4)),
    CONSTRAINT [CK_Users_Status] CHECK ([Status]=N'Disabled' OR [Status]=N'Locked' OR [Status]=N'Active'),
    CONSTRAINT [FK_Users_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [bank].[Customers] ([Id]),
    CONSTRAINT [UQ_Users_CustomerId] UNIQUE NONCLUSTERED ([CustomerId] ASC),
    CONSTRAINT [UQ_Users_EmailBlindIndex] UNIQUE NONCLUSTERED ([EmailBlindIndex] ASC)
);

