CREATE TABLE [auth].[AuditLog] (
    [Id]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [UserId]     UNIQUEIDENTIFIER NULL,
    [Event]      NVARCHAR (50)    NOT NULL,
    [Ip]         VARCHAR (45)     NULL,
    [DeviceInfo] NVARCHAR (300)   NULL,
    [At]         DATETIME2 (3)    CONSTRAINT [DF_Audit_At] DEFAULT (sysutcdatetime()) NOT NULL,
    [Details]    NVARCHAR (1000)  NULL,
    CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Audit_Users] FOREIGN KEY ([UserId]) REFERENCES [auth].[Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_AuditLog_UserId_At]
    ON [auth].[AuditLog]([UserId] ASC, [At] DESC);


GO
CREATE NONCLUSTERED INDEX [IX_AuditLog_Event_At]
    ON [auth].[AuditLog]([Event] ASC, [At] DESC);

