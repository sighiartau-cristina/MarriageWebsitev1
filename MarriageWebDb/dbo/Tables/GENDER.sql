CREATE TABLE [dbo].[Gender] (
    [GenderId]   INT          IDENTITY (1, 1) NOT NULL,
    [GenderName] VARCHAR (25) NOT NULL,
    PRIMARY KEY CLUSTERED ([GenderId] ASC)
);

