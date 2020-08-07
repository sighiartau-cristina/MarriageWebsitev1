CREATE PROCEDURE [dbo].[updateMessageStatus](@senderId int, @receiverId int)
AS

BEGIN
Update Messages 
SET ReadDate=GETDATE(), Status='Read' WHERE
SenderId = @receiverId AND ReceiverId=@senderId AND ReadDate is NULL

END
