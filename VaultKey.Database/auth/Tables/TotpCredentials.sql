CREATE TABLE [auth].[TotpCredentials] (
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [SecretEnc]    VARBINARY (256)  NOT NULL,
    [LastUsedStep] BIGINT           CONSTRAINT [DF_Totp_LastUsedStep] DEFAULT ((0)) NOT NULL,
    [ConfirmedAt]  DATETIME2 (3)    NULL,
    [CreatedAt]    DATETIME2 (3)    CONSTRAINT [DF_Totp_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_TotpCredentials] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_Totp_Users] FOREIGN KEY ([UserId]) REFERENCES [auth].[Users] ([Id]) ON DELETE CASCADE
);

