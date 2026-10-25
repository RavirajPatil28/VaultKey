CREATE TABLE [auth].[ActivationRequests] (
    [Id]         UNIQUEIDENTIFIER CONSTRAINT [DF_ActReq_Id] DEFAULT (newsequentialid()) NOT NULL,
    [CustomerId] UNIQUEIDENTIFIER NOT NULL,
    [OtpHash]    BINARY (32)      NOT NULL,
    [Attempts]   INT              CONSTRAINT [DF_ActReq_Attempts] DEFAULT ((0)) NOT NULL,
    [ExpiresAt]  DATETIME2 (3)    NOT NULL,
    [Status]     NVARCHAR (20)    CONSTRAINT [DF_ActReq_Status] DEFAULT (N'Pending') NOT NULL,
    [Ip]         VARCHAR (45)     NULL,
    [CreatedAt]  DATETIME2 (3)    CONSTRAINT [DF_ActReq_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_ActivationRequests] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_ActReq_Status] CHECK ([Status]=N'Locked' OR [Status]=N'Expired' OR [Status]=N'Verified' OR [Status]=N'Pending'),
    CONSTRAINT [FK_ActReq_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [bank].[Customers] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_ActReq_OnePendingPerCustomer]
    ON [auth].[ActivationRequests]([CustomerId] ASC) WHERE ([Status]=N'Pending');

