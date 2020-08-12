using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BusinessModel.Entities;

namespace MarriageWebWDB.Models
{
    public class UserModel
    {
        public IEnumerable<SelectListItem> Religions { get; set; }

        public IEnumerable<SelectListItem> Genders { get; set; }

        public IEnumerable<SelectListItem> Orientations { get; set; }

        public IEnumerable<SelectListItem> Statuses { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Job { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime Birthday { get; set; }

        public int Age { get; set; }

        public int ReligionId { get; set; }

        public int StatusId { get; set; }

        public int GenderId { get; set; }

        public int OrientationId { get; set; }

        public string BirthdayString { get; set; }

        public string Starsign { get; set; }

        public string Motto { get; set; }

        public List<PreferenceEntity> LikesList{ get; set; }

        public List<PreferenceEntity> DislikesList { get; set; }

        public AddressEntity Address { get; set; }

    }
}