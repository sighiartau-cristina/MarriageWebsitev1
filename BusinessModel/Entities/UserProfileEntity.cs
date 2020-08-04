using System;

namespace BusinessModel.Entities
{
    public class UserProfileEntity
    {
        public int UserProfileId { get; set; }

        public string UserProfileName { get; set; }

        public string UserProfileSurname { get; set; }

        public string UserProfilePhone { get; set; }

        public string UserProfileDescription { get; set; }

        public string UserProfileJob { get; set; }

        public DateTime UserProfileBirthday { get; set; }

        public int StarsignId { get; set; }

        public string Motto { get; set; }

        public string Likes { get; set; }

        public string Dislikes { get; set; }

        public int ReligionId { get; set; }

        public int StatusId { get; set; }

        public int OrientationId { get; set; }

        public int GenderId { get; set; }

        public int UserAge { get; set; }

        public int UserId { get; set; }
    }
}
