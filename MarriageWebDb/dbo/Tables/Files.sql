CREATE TABLE [dbo].[Files]
(
	[FileId] INT NOT NULL PRIMARY KEY identity(1,1),
	[FileName] VARCHAR(255) NOT NULL,
	[ContentType] VARCHAR(100) NOT NULL,
	[Content] VARBINARY(max) NOT NULL,
	[FileType] VARCHAR(100) NOT NULL,
	[UserProfileId] INT NOT NULL,
	CONSTRAINT [FK__ADDRESS__USER_ID__3D5E1FD7] FOREIGN KEY ([UserProfileId]) REFERENCES [dbo].[UserProfile] ([UserProfileId]) ON DELETE CASCADE
)
