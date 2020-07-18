-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE getGendersByName( @name varchar(50)) 
AS
BEGIN
	SELECT * FROM GENDER where GENDER_NAME=@name;
END
