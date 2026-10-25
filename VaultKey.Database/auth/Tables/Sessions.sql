CREATE TABLE [auth].[Sessions] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_Sessions_Id] DEFAULT (newsequentialid()) NOT NULL,
    [UserId]         UNIQUEIDENTIFIER NOT NULL,
    [DeviceInfo]     NVARCHAR (300)   NULL,
    [Ip]             VARCHAR (45)     NULL,
    [CreatedAt]      DATETIME2 (3)    CONSTRAINT [DF_Sessions_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastSeenAt]     DATETIME2 (3)    CONSTRAINT [DF_Sessions_LastSeenAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [AbsoluteExpiry] DATETIME2 (3)    NOT NULL,
    [RevokedAt]      DATETIME2 (3)    NULL,
    [RevokeReason]   NVARCHAR (30)    NULL,
    [DeviceId]       UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Sessions_RevokeReason] CHECK ([RevokeReason] IS NULL OR ([RevokeReason]=N'Expired' OR [RevokeReason]=N'TotpChange' OR [RevokeReason]=N'TokenReuse' OR [RevokeReason]=N'PasswordChange' OR [RevokeReason]=N'PasswordReset' OR [RevokeReason]=N'LogoutAll' OR [RevokeReason]=N'Logout')),
    CONSTRAINT [FK_Sessions_UserDevices] FOREIGN KEY ([DeviceId]) REFERENCES [auth].[UserDevices] ([Id]),
    CONSTRAINT [FK_Sessions_Users] FOREIGN KEY ([UserId]) REFERENCES [auth].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Sessions_DeviceId]
    ON [auth].[Sessions]([DeviceId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Sessions_UserId_RevokedAt]
    ON [auth].[Sessions]([UserId] ASC, [RevokedAt] ASC)
    INCLUDE([LastSeenAt], [AbsoluteExpiry]);

