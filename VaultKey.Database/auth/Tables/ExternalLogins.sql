CREATE TABLE [auth].[ExternalLogins] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_ExtLogins_Id] DEFAULT (newsequentialid()) NOT NULL,
    [UserId]            UNIQUEIDENTIFIER NOT NULL,
    [Provider]          NVARCHAR (30)    NOT NULL,
    [ProviderSubjectId] NVARCHAR (255)   NOT NULL,
    [LinkedAt]          DATETIME2 (3)    CONSTRAINT [DF_ExtLogins_LinkedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_ExternalLogins] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ExtLogins_Users] FOREIGN KEY ([UserId]) REFERENCES [auth].[Users] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_ExtLogins_ProviderSubject] UNIQUE NONCLUSTERED ([Provider] ASC, [ProviderSubjectId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ExtLogins_UserId]
    ON [auth].[ExternalLogins]([UserId] ASC);

