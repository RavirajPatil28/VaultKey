CREATE TABLE [auth].[UserDevices] (
    [Id]              UNIQUEIDENTIFIER CONSTRAINT [DF_UserDevices_Id] DEFAULT (newsequentialid()) NOT NULL,
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [DeviceTokenHash] BINARY (32)      NOT NULL,
    [DeviceName]      NVARCHAR (150)   NOT NULL,
    [FirstSeenAt]     DATETIME2 (3)    CONSTRAINT [DF_UserDevices_FirstSeenAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastSeenAt]      DATETIME2 (3)    CONSTRAINT [DF_UserDevices_LastSeenAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastIp]          VARCHAR (45)     NULL,
    [NotifiedAt]      DATETIME2 (3)    NULL,
    [RevokedAt]       DATETIME2 (3)    NULL,
    CONSTRAINT [PK_UserDevices] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserDevices_Users] FOREIGN KEY ([UserId]) REFERENCES [auth].[Users] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_UserDevices_User_Token] UNIQUE NONCLUSTERED ([UserId] ASC, [DeviceTokenHash] ASC)
);

