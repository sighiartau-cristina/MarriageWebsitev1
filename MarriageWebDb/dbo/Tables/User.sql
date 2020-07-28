CREATE TABLE [dbo].[User] (
    [UserId]           INT          IDENTITY (1, 1) NOT NULL,
    [Username]     VARCHAR (25) NOT NULL,
    [Email]        VARCHAR (25) NOT NULL,
    [Password]     VARCHAR (25) NOT NULL,
    [CreatedAt]   DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

