using DataAccess;

namespace LinqLessons
{
    class Program
    {
        static void Main(string[] args)
        {
            DbModel dbModel = new DbModel();

            //AddGender("male");
            //AddGender("feMale");
            //UpdateGender(2, "female")
            //DeleteGender(1);

            //AddOrientation("heterosexual");
            //UpdateOrientation(1, "heterosexual");
            //DeleteOrientation(1);

            //AddReligion("jediknight");
            //UpdateReligion(1, "jedi knight");
            //DeleteReligion(2);

            //AddStatus("it complicated");
            //UpdateStatus(1, "it is complicated");
            //AddStatus("definitely not married");
            //DeleteStatus(2);

            //AddUser("username", "email", "pass");
            //AddUser("username2", "email2", "veryefficientpassword");
            //UpdateUser(2, "blahblah", "email2", "veryefficientpassword");
            //DeleteUser(2);

            //AddUserProfile(1, "obi wan", "kenobi", "9989785745", "retired jedi master", "", 3, 2, 1, 1, 50);
            //UpdateUserProfile(1, "obi wan", "kenobi", "9989785745", "retired jedi master", "The perfect man? Well, course I know him. He's me!", 3, 2, 1, 1, 50);

            //AddAddress("The Desert", "32", "Again, the Desert", "Planet Tatooine", 1);
            //UpdateAddress(1, "Somewhere in the Desert", "32", "Again, the Desert", "Planet Tatooine");

            /*GetAddress(1);
            GetGender(2);
            GetOrientation(2);
            GetReligion(1);
            GetStatus(1);
            GetUser(1);
            GetUserProfile(1);*/

            /*DeleteAddress(1);
            DeleteUserProfile(1);
            DeleteUser(1);*/



        }

        /*static void AddGender(string name)
        {
            GenderHandler genderHandler = new GenderHandler();
            genderHandler.Add(new GenderEntity { GenderName = name });
        }

        static void DeleteGender(int id)
        {
            GenderHandler genderHandler = new GenderHandler();
            genderHandler.Delete(id);
        }

        static void UpdateGender(int id, string name)
        {
            GenderHandler genderHandler = new GenderHandler();
            genderHandler.Update(new GenderEntity { GenderId = id, GenderName = name });
        }

        static void GetGender(int id)
        {
            GenderHandler genderHandler = new GenderHandler();
            Console.WriteLine(genderHandler.Get(id).GenderName);
        }

        static void AddOrientation(string name)
        {
            OrientationHandler orientationHandler = new OrientationHandler();
            orientationHandler.Add(new OrientationEntity { OrientationName = name });
        }

        static void DeleteOrientation(int id)
        {
            OrientationHandler orientationHandler = new OrientationHandler();
            orientationHandler.Delete(id);
        }

        static void UpdateOrientation(int id, string name)
        {
            OrientationHandler orientationHandler = new OrientationHandler();
            orientationHandler.Update(new OrientationEntity { OrientationId = id, OrientationName = name });
        }

        static void GetOrientation(int id)
        {
            OrientationHandler orientationHandler = new OrientationHandler();
            Console.WriteLine(orientationHandler.Get(id).OrientationName);
        }

        static void AddStatus(string name)
        {
            MaritalStatusHandler statusHandler = new MaritalStatusHandler();
            statusHandler.Add(new MaritalStatusEntity { MaritalStatusName = name });
        }

        static void DeleteStatus(int id)
        {
            MaritalStatusHandler statusHandler = new MaritalStatusHandler();
            statusHandler.Delete(id);
        }

        static void UpdateStatus(int id, string name)
        {
            MaritalStatusHandler statusHandler = new MaritalStatusHandler();
            statusHandler.Update(new MaritalStatusEntity { MaritalStatusId = id, MaritalStatusName = name });
        }

        static void GetStatus(int id)
        {
            MaritalStatusHandler maritalStatusHandler = new MaritalStatusHandler();
            Console.WriteLine(maritalStatusHandler.Get(id).MaritalStatusName);
        }

        static void AddReligion(string name)
        {
            ReligionHandler religionHandler = new ReligionHandler();
            religionHandler.Add(new ReligionEntity { ReligionName = name });
        }

        static void DeleteReligion(int id)
        {
            ReligionHandler religionHandler = new ReligionHandler();
            religionHandler.Delete(id);
        }

        static void UpdateReligion(int id, string name)
        {
            ReligionHandler religionHandler = new ReligionHandler();
            religionHandler.Update(new ReligionEntity { ReligionId = id, ReligionName = name });
        }

        static void GetReligion(int id)
        {
            ReligionHandler religionHandler = new ReligionHandler();
            Console.WriteLine(religionHandler.Get(id).ReligionName);
        }

        static void AddAddress(string street, string streetNo, string city, string country, int userId)
        {
            AddressHandler addressHandler = new AddressHandler();
            addressHandler.Add(new AddressEntity { AddressStreet = street, AddressStreetNo = streetNo, AddressCity = city, AddressCountry = country, UserProfileId = userId });
        }

        static void DeleteAddress(int id)
        {
            GenderHandler genderHandler = new GenderHandler();
            genderHandler.Delete(id);
        }

        static void UpdateAddress(int id, string street, string streetNo, string city, string country)
        {
            AddressHandler addressHandler = new AddressHandler();
            addressHandler.Update(new AddressEntity { AddressId = id, AddressStreet = street, AddressStreetNo = streetNo, AddressCity = city, AddressCountry = country });
        }

        static void GetAddress(int id)
        {
            AddressHandler addressHandler = new AddressHandler();
            Console.WriteLine(addressHandler.Get(id).AddressStreet);
        }

        static void AddUserProfile(int userId, string name, string surname, string phone, string job, string description, int gender, int orientation, int status, int religion, int age)
        {
            UserProfileHandler userProfileHandler = new UserProfileHandler();
            var testEntity = new UserProfileEntity { UserProfileName = name, UserProfileSurname = surname, UserProfileDescription = description, UserProfileJob = job, UserProfilePhone = phone, UserProfileBirthday = new DateTime(2000, 10, 11), UserId = userId, UserAge = age, GenderId = gender, OrientationId = orientation, StatusId = status, ReligionId = religion };
            userProfileHandler.Add(testEntity);

        }

        static void DeleteUserProfile(int id)
        {
            UserProfileHandler userProfileHandler = new UserProfileHandler();
            userProfileHandler.Delete(id);
        }

        static void UpdateUserProfile(int id, string name, string surname, string phone, string job, string description, int gender, int orientation, int status, int religion, int age)
        {
            UserProfileHandler userProfileHandler = new UserProfileHandler();
            userProfileHandler.Update(new UserProfileEntity { UserProfileId = id, UserProfileName = name, UserProfileSurname = surname, UserProfileDescription = description, UserProfileJob = job, UserProfilePhone = phone, UserProfileBirthday = new DateTime(2000, 10, 11), UserAge = age, GenderId = gender, OrientationId = orientation, StatusId = status, ReligionId = religion });
        }

        static void GetUserProfile(int id)
        {
            UserProfileHandler userProfileHandler = new UserProfileHandler();
            Console.WriteLine(userProfileHandler.Get(id).UserProfileName);
        }

        static void AddUser(string username, string email, string password)
        {
            UserHandler userHandler = new UserHandler();
            userHandler.Add(new UserEntity { UserUsername = username, UserEmail = email, UserPassword = password, CreatedAt = DateTime.Now });

        }

        static void DeleteUser(int id)
        {
            UserHandler userHandler = new UserHandler();
            userHandler.Delete(id);
        }

        static void UpdateUser(int id, string username, string email, string password)
        {
            UserHandler userHandler = new UserHandler();
            userHandler.Update(new UserEntity { UserId = id, UserUsername = username, UserEmail = email, UserPassword = password });
        }

        static void GetUser(int id)
        {
            UserHandler userHandler = new UserHandler();
            Console.WriteLine(userHandler.Get(id).UserUsername);
        }*/
    }
}
