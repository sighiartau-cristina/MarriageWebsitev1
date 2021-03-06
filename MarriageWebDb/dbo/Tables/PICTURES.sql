﻿CREATE TABLE [dbo].[PICTURES]
(
    [PICTURE_ID]       INT          IDENTITY (1, 1) NOT NULL,
    [PICTURE]   VARBINARY(max) NOT NULL,
    [USER_PROFILE_ID]       INT          NOT NULL,
    CONSTRAINT [PK__Address__498C6686D780CD7A] PRIMARY KEY CLUSTERED ([PICTURE_ID] ASC),
    CONSTRAINT [FK__ADDRESS__USER_ID__3D6E1FD8] FOREIGN KEY ([USER_PROFILE_ID]) REFERENCES [dbo].[USER_PROFILE] ([USRPROF_ID]) ON DELETE CASCADE
)


