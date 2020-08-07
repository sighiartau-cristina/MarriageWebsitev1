CREATE PROCEDURE getSuggestions( @user_profile int) 
AS

DECLARE @user_age INT;
DECLARE @addressCity VARCHAR(255);
DECLARE @addressCountry VARCHAR(255);

DECLARE @gender VARCHAR(255);
DECLARE @orientation VARCHAR(255);

DECLARE @religionId INT;
DECLARE @statusId INT;

SET @religionId = (SELECT u.ReligionId from UserProfile u where u.UserProfileId = @user_profile);
SET @statusId = (SELECT u.StatusId from UserProfile u where u.UserProfileId = @user_profile);

SET @gender = (SELECT g.GenderName from Gender g join UserProfile u on g.GenderId=u.GenderId where u.UserProfileId = @user_profile);
SET @orientation = (SELECT o.OrientationName from Orientation o join UserProfile u on o.OrientationId=u.OrientationId where u.UserProfileId = @user_profile);
SET @addressCity = (SELECT a.AddressCity from UserProfile u join ADDRESS a on a.UserProfileId=u.UserProfileId where u.UserProfileId = @user_profile);
SET @addressCountry = (SELECT a.AddressCountry from UserProfile u join ADDRESS a on a.UserProfileId=u.UserProfileId where u.UserProfileId = @user_profile);
SET @user_age = (SELECT u.Age from UserProfile u where u.UserProfileId=@user_profile);

If @gender = 'Male'
	If @orientation = 'Straight' 
		--straight male matches straight/bi/other female
		BEGIN
			Select TOP(10) u.UserProfileId from UserProfile u WHERE 
				u.UserProfileId!=@user_profile 
				and (SELECT g.GenderName from Gender g join UserProfile u1 on g.GenderId=u.GenderId where u1.UserProfileId = u.UserProfileId)='Female' 
				and (SELECT o.OrientationName from Orientation o join UserProfile u1 on o.OrientationId=u.OrientationId where u1.UserProfileId = u.UserProfileId)!='Gay' 
				and NOT EXISTS (SELECT * from Match m where (m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile) or (m.MatchUserProfileId=@user_profile and m.UserProfileId=u.UserProfileId and m.Accepted=0))
				ORDER BY  ABS(u.Age - @user_age) ASC, 
						  (SELECT COUNT(p.Id) from UserProfile_Preference p where p.UserProfileId=@user_profile and EXISTS (SELECT * from UserProfile_Preference up where up.Likes=p.Likes and up.UserProfileId=u.UserProfileId and p.PrefId=up.PrefId)) DESC, 
						  (SELECT COUNT (a.AddressId) from Address a where EXISTS(SELECT * from Address a where a.UserProfileId=u.UserProfileId and (@addressCity=a.AddressCity OR a.AddressCountry=@addressCountry))) DESC,
						  (SELECT COUNT (r.ReligionId) from Religion r where EXISTS(SELECT * from Religion r join UserProfile up on r.ReligionId=up.ReligionId where up.UserProfileId=u.UserProfileId and r.ReligionId=@religionId)) DESC,
						  (SELECT COUNT (s.StatusId) from Status s where EXISTS(SELECT * from Status s join UserProfile up on s.StatusId=up.StatusId where up.UserProfileId=u.UserProfileId and s.StatusId=@statusId)) DESC;
		END
	Else If @orientation = 'Gay'
		--gay male matches gay/bi/other male
		BEGIN
			Select TOP(10) u.UserProfileId from UserProfile u WHERE 
				u.UserProfileId!=@user_profile 
				and (SELECT g.GenderName from Gender g join UserProfile u1 on g.GenderId=u.GenderId where u1.UserProfileId = u.UserProfileId)='Male' 
				and (SELECT o.OrientationName from Orientation o join UserProfile u1 on o.OrientationId=u.OrientationId where u1.UserProfileId = u.UserProfileId)!='Straight' 
				and NOT EXISTS (SELECT * from Match m where (m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile) or (m.MatchUserProfileId=@user_profile and m.UserProfileId=u.UserProfileId and m.Accepted=0))
				ORDER BY  ABS(u.Age - @user_age) ASC, 
						  (SELECT COUNT(p.Id) from UserProfile_Preference p where p.UserProfileId=@user_profile and EXISTS (SELECT * from UserProfile_Preference up where up.Likes=p.Likes and up.UserProfileId=u.UserProfileId and p.PrefId=up.PrefId)) DESC, 
						  (SELECT COUNT (a.AddressId) from Address a where EXISTS(SELECT * from Address a where a.UserProfileId=u.UserProfileId and (@addressCity=a.AddressCity OR a.AddressCountry=@addressCountry))) DESC,
						  (SELECT COUNT (r.ReligionId) from Religion r where EXISTS(SELECT * from Religion r join UserProfile up on r.ReligionId=up.ReligionId where up.UserProfileId=u.UserProfileId and r.ReligionId=@religionId)) DESC,
						  (SELECT COUNT (s.StatusId) from Status s where EXISTS(SELECT * from Status s join UserProfile up on s.StatusId=up.StatusId where up.UserProfileId=u.UserProfileId and s.StatusId=@statusId)) DESC;
		END
	Else If @orientation = 'Bisexual'
		--bi male matches gay/bi/other male and straight/bi/other female
		BEGIN
			Select TOP(10) u.UserProfileId from UserProfile u WHERE 
				u.UserProfileId!=@user_profile 
				and ((SELECT g.GenderName as gender from Gender g join UserProfile u1 on g.GenderId=u.GenderId where u1.UserProfileId = u.UserProfileId)='Male' and (SELECT o.OrientationName from Orientation o join UserProfile u1 on o.OrientationId=u.OrientationId where u1.UserProfileId = u.UserProfileId)!='Straight' 
				OR (SELECT g.GenderName as gender from Gender g join UserProfile u1 on g.GenderId=u.GenderId where u1.UserProfileId = u.UserProfileId)='Female' and (SELECT o.OrientationName from Orientation o join UserProfile u1 on o.OrientationId=u.OrientationId where u1.UserProfileId = u.UserProfileId)!='Gay' )
				and NOT EXISTS (SELECT * from Match m where (m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile) or (m.MatchUserProfileId=@user_profile and m.UserProfileId=u.UserProfileId and m.Accepted=0))
				ORDER BY  ABS(u.Age - @user_age) ASC, 
						  (SELECT COUNT(p.Id) from UserProfile_Preference p where p.UserProfileId=@user_profile and EXISTS (SELECT * from UserProfile_Preference up where up.Likes=p.Likes and up.UserProfileId=u.UserProfileId and p.PrefId=up.PrefId)) DESC, 
						  (SELECT COUNT (a.AddressId) from Address a where EXISTS(SELECT * from Address a where a.UserProfileId=u.UserProfileId and (@addressCity=a.AddressCity OR a.AddressCountry=@addressCountry))) DESC,
						  (SELECT COUNT (r.ReligionId) from Religion r where EXISTS(SELECT * from Religion r join UserProfile up on r.ReligionId=up.ReligionId where up.UserProfileId=u.UserProfileId and r.ReligionId=@religionId)) DESC,
						  (SELECT COUNT (s.StatusId) from Status s where EXISTS(SELECT * from Status s join UserProfile up on s.StatusId=up.StatusId where up.UserProfileId=u.UserProfileId and s.StatusId=@statusId)) DESC;
		END
	Else
		--other male matches anything?
		BEGIN
			Select TOP(10) u.UserProfileId from UserProfile u WHERE 
				u.UserProfileId!=@user_profile
				and NOT EXISTS (SELECT * from Match m where (m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile) or (m.MatchUserProfileId=@user_profile and m.UserProfileId=u.UserProfileId and m.Accepted=0))
				ORDER BY  ABS(u.Age - @user_age) ASC, 
						  (SELECT COUNT(p.Id) from UserProfile_Preference p where p.UserProfileId=@user_profile and EXISTS (SELECT * from UserProfile_Preference up where up.Likes=p.Likes and up.UserProfileId=u.UserProfileId and p.PrefId=up.PrefId)) DESC, 
						  (SELECT COUNT (a.AddressId) from Address a where EXISTS(SELECT * from Address a where a.UserProfileId=u.UserProfileId and (@addressCity=a.AddressCity OR a.AddressCountry=@addressCountry))) DESC,
						  (SELECT COUNT (r.ReligionId) from Religion r where EXISTS(SELECT * from Religion r join UserProfile up on r.ReligionId=up.ReligionId where up.UserProfileId=u.UserProfileId and r.ReligionId=@religionId)) DESC,
						  (SELECT COUNT (s.StatusId) from Status s where EXISTS(SELECT * from Status s join UserProfile up on s.StatusId=up.StatusId where up.UserProfileId=u.UserProfileId and s.StatusId=@statusId)) DESC;
		END
Else
	If @orientation = 'Straight'
		--straight female matches straight/bi/other male
		BEGIN
			Select TOP(10) u.UserProfileId from UserProfile u WHERE 
				u.UserProfileId!=@user_profile 
				and (SELECT g.GenderName from Gender g join UserProfile u1 on g.GenderId=u.GenderId where u1.UserProfileId = u.UserProfileId)='Male' 
				and (SELECT o.OrientationName from Orientation o join UserProfile u1 on o.OrientationId=u.OrientationId where u1.UserProfileId = u.UserProfileId)!='Gay' 
				and NOT EXISTS (SELECT * from Match m where (m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile) or (m.MatchUserProfileId=@user_profile and m.UserProfileId=u.UserProfileId and m.Accepted=0))
				ORDER BY  ABS(u.Age - @user_age) ASC, 
						  (SELECT COUNT(p.Id) from UserProfile_Preference p where p.UserProfileId=@user_profile and EXISTS (SELECT * from UserProfile_Preference up where up.Likes=p.Likes and up.UserProfileId=u.UserProfileId and p.PrefId=up.PrefId)) DESC, 
						  (SELECT COUNT (a.AddressId) from Address a where EXISTS(SELECT * from Address a where a.UserProfileId=u.UserProfileId and (@addressCity=a.AddressCity OR a.AddressCountry=@addressCountry))) DESC,
						  (SELECT COUNT (r.ReligionId) from Religion r where EXISTS(SELECT * from Religion r join UserProfile up on r.ReligionId=up.ReligionId where up.UserProfileId=u.UserProfileId and r.ReligionId=@religionId)) DESC,
						  (SELECT COUNT (s.StatusId) from Status s where EXISTS(SELECT * from Status s join UserProfile up on s.StatusId=up.StatusId where up.UserProfileId=u.UserProfileId and s.StatusId=@statusId)) DESC;
			
		END
	Else If @orientation = 'Gay'
		--gay female matches gay/bi/other female
		BEGIN
			Select TOP(10) u.UserProfileId from UserProfile u WHERE 
				u.UserProfileId!=@user_profile 
				and (SELECT g.GenderName from Gender g join UserProfile u1 on g.GenderId=u.GenderId where u1.UserProfileId = u.UserProfileId)='Female' 
				and (SELECT o.OrientationName from Orientation o join UserProfile u1 on o.OrientationId=u.OrientationId where u1.UserProfileId = u.UserProfileId)!='Straight' 
				and NOT EXISTS (SELECT * from Match m where (m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile) or (m.MatchUserProfileId=@user_profile and m.UserProfileId=u.UserProfileId and m.Accepted=0))
				ORDER BY  ABS(u.Age - @user_age) ASC, 
						  (SELECT COUNT(p.Id) from UserProfile_Preference p where p.UserProfileId=@user_profile and EXISTS (SELECT * from UserProfile_Preference up where up.Likes=p.Likes and up.UserProfileId=u.UserProfileId and p.PrefId=up.PrefId)) DESC, 
						  (SELECT COUNT (a.AddressId) from Address a where EXISTS(SELECT * from Address a where a.UserProfileId=u.UserProfileId and (@addressCity=a.AddressCity OR a.AddressCountry=@addressCountry))) DESC,
						  (SELECT COUNT (r.ReligionId) from Religion r where EXISTS(SELECT * from Religion r join UserProfile up on r.ReligionId=up.ReligionId where up.UserProfileId=u.UserProfileId and r.ReligionId=@religionId)) DESC,
						  (SELECT COUNT (s.StatusId) from Status s where EXISTS(SELECT * from Status s join UserProfile up on s.StatusId=up.StatusId where up.UserProfileId=u.UserProfileId and s.StatusId=@statusId)) DESC;					  
		END
	Else If @orientation = 'Bisexual'
		--bi female matches gay/bi/other female or straight/bi/other male
		BEGIN
			Select TOP(10) u.UserProfileId from UserProfile u WHERE 
				u.UserProfileId!=@user_profile 
				and ((SELECT g.GenderName as gender from Gender g join UserProfile u1 on g.GenderId=u.GenderId where u1.UserProfileId = u.UserProfileId)='Male' and (SELECT o.OrientationName from Orientation o join UserProfile u1 on o.OrientationId=u.OrientationId where u1.UserProfileId = u.UserProfileId)!='Gay' 
				OR (SELECT g.GenderName as gender from Gender g join UserProfile u1 on g.GenderId=u.GenderId where u1.UserProfileId = u.UserProfileId)='Female' and (SELECT o.OrientationName from Orientation o join UserProfile u1 on o.OrientationId=u.OrientationId where u1.UserProfileId = u.UserProfileId)!='Straight' )
				and NOT EXISTS (SELECT * from Match m where (m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile) or (m.MatchUserProfileId=@user_profile and m.UserProfileId=u.UserProfileId and m.Accepted=0))
				ORDER BY  ABS(u.Age - @user_age) ASC, 
						  (SELECT COUNT(p.Id) from UserProfile_Preference p where p.UserProfileId=@user_profile and EXISTS (SELECT * from UserProfile_Preference up where up.Likes=p.Likes and up.UserProfileId=u.UserProfileId and p.PrefId=up.PrefId)) DESC, 
						  (SELECT COUNT (a.AddressId) from Address a where EXISTS(SELECT * from Address a where a.UserProfileId=u.UserProfileId and (@addressCity=a.AddressCity OR a.AddressCountry=@addressCountry))) DESC,
						  (SELECT COUNT (r.ReligionId) from Religion r where EXISTS(SELECT * from Religion r join UserProfile up on r.ReligionId=up.ReligionId where up.UserProfileId=u.UserProfileId and r.ReligionId=@religionId)) DESC,
						  (SELECT COUNT (s.StatusId) from Status s where EXISTS(SELECT * from Status s join UserProfile up on s.StatusId=up.StatusId where up.UserProfileId=u.UserProfileId and s.StatusId=@statusId)) DESC;
		END
	Else
		--other female matches anything?
		BEGIN
			Select TOP(10) u.UserProfileId from UserProfile u WHERE 
				u.UserProfileId!=@user_profile 
				and NOT EXISTS (SELECT * from Match m where (m.MatchUserProfileId=u.UserProfileId and m.UserProfileId=@user_profile) or (m.MatchUserProfileId=@user_profile and m.UserProfileId=u.UserProfileId and m.Accepted=0))
				ORDER BY  ABS(u.Age - @user_age) ASC, 
						  (SELECT COUNT(p.Id) from UserProfile_Preference p where p.UserProfileId=@user_profile and EXISTS (SELECT * from UserProfile_Preference up where up.Likes=p.Likes and up.UserProfileId=u.UserProfileId and p.PrefId=up.PrefId)) DESC, 
						  (SELECT COUNT (a.AddressId) from Address a where EXISTS(SELECT * from Address a where a.UserProfileId=u.UserProfileId and (@addressCity=a.AddressCity OR a.AddressCountry=@addressCountry))) DESC,
						  (SELECT COUNT (r.ReligionId) from Religion r where EXISTS(SELECT * from Religion r join UserProfile up on r.ReligionId=up.ReligionId where up.UserProfileId=u.UserProfileId and r.ReligionId=@religionId)) DESC,
						  (SELECT COUNT (s.StatusId) from Status s where EXISTS(SELECT * from Status s join UserProfile up on s.StatusId=up.StatusId where up.UserProfileId=u.UserProfileId and s.StatusId=@statusId)) DESC;
		END