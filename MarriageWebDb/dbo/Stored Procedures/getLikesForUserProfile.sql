CREATE PROCEDURE [dbo].[getLikesForUserProfile] ( @user_profile int) 
AS

BEGIN
select l.* from Likes l join UserProfile_Likes u on l.Id=u.LikeId where u.UserProfileId=@user_profile;
END
