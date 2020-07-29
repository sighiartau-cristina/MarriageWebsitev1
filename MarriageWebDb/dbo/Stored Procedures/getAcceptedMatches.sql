CREATE PROCEDURE [dbo].[getAcceptedMatches]( @user_profile int) 
AS

Select m.MatchUserProfileId from Match m  WHERE m.UserProfileId=@user_profile and m.Accepted=1 and EXISTS (SELECT 1 from Match m1 where m1.MatchUserProfileId=@user_profile and m1.UserProfileId=m.MatchUserProfileId and m1.Accepted=1);

