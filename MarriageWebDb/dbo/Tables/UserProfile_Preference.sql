CREATE TABLE [dbo].[UserProfile_Preference]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[PrefId] INT NOT NULL,
	[UserProfileId] INT NOT NULL,
	[Likes] BIT NOT NULL,
	CONSTRAINT [FK__LIKE__USER_ID__3D5E1FD2] FOREIGN KEY ([PrefId]) REFERENCES [dbo].[Preference] ([Id]),
	CONSTRAINT [FK__USER_ID__USER_ID__3D5E1FD2] FOREIGN KEY ([UserProfileId]) REFERENCES [dbo].[UserProfile] ([UserProfileId])
)
