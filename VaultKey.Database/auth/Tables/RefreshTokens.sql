CREATE TABLE [auth].[RefreshTokens] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_RefreshTokens_Id] DEFAULT (newsequentialid()) NOT NULL,
    [SessionId]    UNIQUEIDENTIFIER NOT NULL,
    [FamilyId]     UNIQUEIDENTIFIER NOT NULL,
    [TokenHash]    BINARY (32)      NOT NULL,
    [ReplacedById] UNIQUEIDENTIFIER NULL,
    [ExpiresAt]    DATETIME2 (3)    NOT NULL,
    [UsedAt]       DATETIME2 (3)    NULL,
    [RevokedAt]    DATETIME2 (3)    NULL,
    [CreatedAt]    DATETIME2 (3)    CONSTRAINT [DF_RefreshTokens_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RefreshTokens_ReplacedBy] FOREIGN KEY ([ReplacedById]) REFERENCES [auth].[RefreshTokens] ([Id]),
    CONSTRAINT [FK_RefreshTokens_Sessions] FOREIGN KEY ([SessionId]) REFERENCES [auth].[Sessions] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_RefreshTokens_TokenHash] UNIQUE NONCLUSTERED ([TokenHash] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_FamilyId]
    ON [auth].[RefreshTokens]([FamilyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_SessionId]
    ON [auth].[RefreshTokens]([SessionId] ASC);

