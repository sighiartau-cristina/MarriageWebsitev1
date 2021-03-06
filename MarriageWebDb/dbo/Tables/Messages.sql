﻿CREATE TABLE [dbo].[Messages]
(
	[MessageId] INT NOT NULL PRIMARY KEY identity(1,1),
	[SenderId] INT NOT NULL,
	[ReceiverId] INT NOT NULL,
	[MessageText] VARCHAR(255) NOT NULL,
	[SendDate] DATETIME NOT NULL,
	[ReadDate] DATETIME NULL,
	[Status] VARCHAR(10) NOT NULL,
	CONSTRAINT [SenderId__3D6E1FD8] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE NO ACTION,
	CONSTRAINT [ReceiverId__3D5E1FD1] FOREIGN KEY ([ReceiverId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE  NO ACTION
)
