CREATE TABLE [dbo].[Address] (
    [AddressId]       INT          IDENTITY (1, 1) NOT NULL,
    [AddressStreet]   VARCHAR (25) NOT NULL,
    [AddressStreetNo] VARCHAR (25) NOT NULL,
    [AddressCity]     VARCHAR (25) NOT NULL,
    [AddressCountry]  VARCHAR (25) NOT NULL,
    [UserProfileId]       INT          NOT NULL,
    CONSTRAINT [PK__Address__498C6686D780CD2A] PRIMARY KEY CLUSTERED ([AddressId] ASC),
    CONSTRAINT [FK__ADDRESS__USER_ID__3D5E1FD2] FOREIGN KEY ([UserProfileId]) REFERENCES [dbo].[UserProfile] ([UserProfileId]) ON DELETE CASCADE
);

