GO
/****** Object:  StoredProcedure [dbo].[getSuggestions]    Script Date: 24-Jul-20 4:49:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[getSuggestions]( @user_profile int) AS

DECLARE @genderId INT;
DECLARE @orientationId INT;
DECLARE @statusId INT;
DECLARE @religionId INT;
DECLARE @addressCity VARCHAR;
DECLARE @addressCountry VARCHAR;

SET @genderId = (SELECT USER_PROFILE.GENDER_ID from USER_PROFILE where USER_PROFILE.USRPROF_ID = @user_profile)
SET @orientationId = (SELECT USER_PROFILE.ORIENTATION_ID from USER_PROFILE where USER_PROFILE.USRPROF_ID = @user_profile)
SET @statusId = (SELECT USER_PROFILE.STATUS_ID from USER_PROFILE where USER_PROFILE.USRPROF_ID = @user_profile)
SET @religionId = (SELECT USER_PROFILE.RELIGION_ID from USER_PROFILE where USER_PROFILE.USRPROF_ID = @user_profile)
SET @addressCity = (SELECT a.ADDRESS_CITY from USER_PROFILE u join ADDRESS a on a.USER_PROFILE_ID=u.USRPROF_ID where u.USRPROF_ID = @user_profile)
SET @addressCountry = (SELECT a.ADDRESS_COUNTRY from USER_PROFILE u join ADDRESS a on a.USER_PROFILE_ID=u.USRPROF_ID where u.USRPROF_ID = @user_profile)

 If @genderId = 2
	If @orientationId = 13 
		--straight male matches straight/bi/other female
		BEGIN
			Select * from USER_PROFILE u1 inner join USER_PROFILE u2 on u1.USRPROF_ID!=@user_profile and u2.USRPROF_ID!=@user_profile and u2.GENDER_ID=3 and u2.ORIENTATION_ID!=14 and u2.RELIGION_ID=@religionId and u2.STATUS_ID=@statusId
			UNION ALL
			(Select * from USER_PROFILE u1 inner join USER_PROFILE u2 on u1.USRPROF_ID!=@user_profile and u2.USRPROF_ID!=@user_profile and u2.GENDER_ID=3 and u2.ORIENTATION_ID!=14 and u2.RELIGION_ID!=@religionId and u2.STATUS_ID!=@statusId)
		END
	Else If @orientationId = 14
		--gay male matches gay/bi/other male
		BEGIN
			Select * from USER_PROFILE u1 inner join USER_PROFILE u2 on u1.USRPROF_ID!=@user_profile and u2.USRPROF_ID!=@user_profile and u2.GENDER_ID=2 and u2.ORIENTATION_ID!=13
		END
	Else If @orientationId = 4
		--bi male matches gay/bi/other male and straight/bi/other female
		BEGIN
			Select * from USER_PROFILE u1 inner join USER_PROFILE u2 on u1.USRPROF_ID!=@user_profile and u2.USRPROF_ID!=@user_profile and ((u2.GENDER_ID=2 and u2.ORIENTATION_ID!=13) or (u2.GENDER_ID=3 and u2.ORIENTATION_ID!=14))
		END
	Else
		--other male matches anything?
		BEGIN
			Select * from USER_PROFILE u1 inner join USER_PROFILE u2 on u1.USRPROF_ID!=@user_profile and u2.USRPROF_ID!=@user_profile
		END
Else
	If @orientationId = 13 
		--straight female matches straight/bi/other male
		BEGIN
			Select * from USER_PROFILE u1 inner join USER_PROFILE u2 on u1.USRPROF_ID!=@user_profile and u2.USRPROF_ID!=@user_profile and u2.GENDER_ID=2 and u2.ORIENTATION_ID!=14
		END
	Else If @orientationId = 14
		--gay female matches gay/bi/other female
		BEGIN
			Select * from USER_PROFILE u1 inner join USER_PROFILE u2 on u1.USRPROF_ID!=@user_profile and u2.USRPROF_ID!=@user_profile and u2.GENDER_ID=3 and u2.ORIENTATION_ID!=13
		END
	Else
		--other female matches anything?
		BEGIN
			Select * from USER_PROFILE u1 inner join USER_PROFILE u2 on u1.USRPROF_ID!=@user_profile and u2.USRPROF_ID!=@user_profile
		END