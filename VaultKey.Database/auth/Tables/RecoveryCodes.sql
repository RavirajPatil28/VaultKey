CREATE TABLE [auth].[RecoveryCodes] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF_Recovery_Id] DEFAULT (newsequentialid()) NOT NULL,
    [UserId]    UNIQUEIDENTIFIER NOT NULL,
    [CodeHash]  BINARY (32)      NOT NULL,
    [UsedAt]    DATETIME2 (3)    NULL,
    [CreatedAt] DATETIME2 (3)    CONSTRAINT [DF_Recovery_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_RecoveryCodes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Recovery_Users] FOREIGN KEY ([UserId]) REFERENCES [auth].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RecoveryCodes_UserId]
    ON [auth].[RecoveryCodes]([UserId] ASC)
    INCLUDE([CodeHash], [UsedAt]);

