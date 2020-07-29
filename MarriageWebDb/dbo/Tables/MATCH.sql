CREATE TABLE [dbo].[Match] (
    [MatchId] INT  NOT NULL PRIMARY KEY identity(1, 1),
    [UserProfileId] INT NOT NULL,
    [MatchUserProfileId]    INT NOT NULL,
    [MatchDate] DATETIME NOT NULL, 
    [Accepted] BIT,
    FOREIGN KEY ([UserProfileId]) REFERENCES [dbo].[UserProfile] ([UserProfileId]),
    FOREIGN KEY ([MatchUserProfileId]) REFERENCES [dbo].[UserProfile] ([UserProfileId]) 
);

