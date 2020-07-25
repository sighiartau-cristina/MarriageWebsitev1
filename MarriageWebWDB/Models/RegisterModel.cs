using System;
using System.Collections.Generic;

namespace MarriageWebWDB.Models
{
    public class RegisterModel
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> Religions { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Genders { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Orientations { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Statuses { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string UserPassword { get; set; }

        public string UserConfirmPassword { get; set; }

        public string UserProfileName { get; set; }

        public string UserProfileSurname { get; set; }

        public DateTime UserProfileBirthday { get; set; }

        public int GenderId { get; set; }

        public int ReligionId { get; set; }

        public int OrientationId { get; set; }

        public int StatusId { get; set; }
    }
}