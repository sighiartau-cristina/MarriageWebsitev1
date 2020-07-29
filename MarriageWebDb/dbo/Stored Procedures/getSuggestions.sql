CREATE PROCEDURE getSuggestions( @user_profile int) 
AS

DECLARE @genderId INT;
DECLARE @orientationId INT;
DECLARE @statusId INT;
DECLARE @religionId INT;
DECLARE @addressCity VARCHAR;
DECLARE @addressCountry VARCHAR;

SET @genderId = (SELECT UserProfile.GenderId from UserProfile where UserProfile.UserProfileId = @user_profile)
SET @orientationId = (SELECT UserProfile.OrientationId from UserProfile where UserProfile.UserProfileId = @user_profile)
SET @statusId = (SELECT UserProfile.StatusId from UserProfile where UserProfile.UserProfileId = @user_profile)
SET @religionId = (SELECT UserProfile.ReligionId from UserProfile where UserProfile.UserProfileId = @user_profile)
SET @addressCity = (SELECT a.AddressCity from UserProfile u join ADDRESS a on a.UserProfileId=u.UserProfileId where u.UserProfileId = @user_profile)
SET @addressCountry = (SELECT a.AddressCountry from UserProfile u join ADDRESS a on a.UserProfileId=u.UserProfileId where u.UserProfileId = @user_profile)

If @genderId = 1
	If @orientationId = 1 
		--straight male matches straight/bi/other female
		BEGIN
			Select u.UserProfileId from UserProfile u full join Match m on u.UserProfileId=m.MatchUserProfileId WHERE u.UserProfileId!=@user_profile and u.GenderId=2 and NOT EXISTS (SELECT * from Match m where m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile)
		END
	Else If @orientationId = 2
		--gay male matches gay/bi/other male
		BEGIN
			Select UserProfileId from UserProfile u WHERE u.UserProfileId!=@user_profile and u.GenderId=1 and u.OrientationId!=1 and NOT EXISTS (SELECT * from Match m where m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile)
		END
	Else If @orientationId = 3
		--bi male matches gay/bi/other male and straight/bi/other female
		BEGIN
			Select UserProfileId from UserProfile u WHERE u.UserProfileId!=@user_profile and ((u.GenderId=1 and u.OrientationId!=1) or (u.GenderId=2 and u.OrientationId!=2)) and NOT EXISTS (SELECT * from Match m where m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile)
		END
	Else
		--other male matches anything?
		BEGIN
			Select UserProfileId from UserProfile u WHERE u.UserProfileId!=@user_profile and NOT EXISTS (SELECT * from Match m where m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile)
		END
Else
	If @orientationId = 1 
		--straight female matches straight/bi/other male
		BEGIN
			Select UserProfileId from UserProfile u WHERE u.UserProfileId!=@user_profile and u.GenderId=1 and u.OrientationId!=2 and NOT EXISTS (SELECT * from Match m where m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile)
		END
	Else If @orientationId = 2
		--gay female matches gay/bi/other female
		BEGIN
			Select UserProfileId from UserProfile u WHERE u.UserProfileId!=@user_profile and u.GenderId=2 and u.OrientationId!=1 and NOT EXISTS (SELECT * from Match m where m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile)
		END
	Else If @orientationId = 3
		--bi female matches gay/bi/other female or straight/bi/other male
		BEGIN
			Select UserProfileId from UserProfile u WHERE u.UserProfileId!=@user_profile and ((u.GenderId=1 and u.OrientationId!=2) or (u.GenderId=2 and u.OrientationId!=1)) and NOT EXISTS (SELECT * from Match m where m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile)
		END
	Else
		--other female matches anything?
		BEGIN
			Select UserProfileId from UserProfile u WHERE u.UserProfileId!=@user_profile and NOT EXISTS (SELECT * from Match m where m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile)
		END