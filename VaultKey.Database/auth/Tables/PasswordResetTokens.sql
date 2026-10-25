CREATE TABLE [auth].[PasswordResetTokens] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF_PwdReset_Id] DEFAULT (newsequentialid()) NOT NULL,
    [UserId]    UNIQUEIDENTIFIER NOT NULL,
    [TokenHash] BINARY (32)      NOT NULL,
    [ExpiresAt] DATETIME2 (3)    NOT NULL,
    [UsedAt]    DATETIME2 (3)    NULL,
    [Ip]        VARCHAR (45)     NULL,
    [CreatedAt] DATETIME2 (3)    CONSTRAINT [DF_PwdReset_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_PasswordResetTokens] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PwdReset_Users] FOREIGN KEY ([UserId]) REFERENCES [auth].[Users] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_PwdReset_TokenHash] UNIQUE NONCLUSTERED ([TokenHash] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_PwdReset_UserId]
    ON [auth].[PasswordResetTokens]([UserId] ASC, [ExpiresAt] ASC);

