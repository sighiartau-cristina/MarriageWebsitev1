CREATE TABLE [dbo].[UserProfile] (
    [UserProfileId]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (25)  NOT NULL,
    [Surname]     VARCHAR (25)  NOT NULL,
    [Phone]       VARCHAR (25)  NULL,
    [Description] VARCHAR (255) NULL,
    [Job]         VARCHAR (50)  NULL,
    [Birthday]    DATE          NOT NULL,
    [ReligionId]    INT NOT NULL,
    [StatusId]      INT NOT NULL,
    [OrientationId] INT NOT NULL,
    [GenderId]      INT NOT NULL,
    [Age]         INT NOT NULL,
    [UserId] INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([UserProfileId] ASC), 
    CONSTRAINT [FK_USER_PROFILE_ToReligion] FOREIGN KEY ([ReligionId]) REFERENCES [Religion]([ReligionId]),
    CONSTRAINT [FK_USER_PROFILE_ToGender] FOREIGN KEY ([GenderId]) REFERENCES [Gender]([GenderId]),
    CONSTRAINT [FK_USER_PROFILE_ToStatus] FOREIGN KEY ([StatusId]) REFERENCES [Status]([StatusId]),
    CONSTRAINT [FK_USER_PROFILE_ToOrientation] FOREIGN KEY ([OrientationId]) REFERENCES [Orientation]([OrientationId]),
    CONSTRAINT [FK_USER_PROFILE_ToProfile] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]) ON DELETE CASCADE
);

