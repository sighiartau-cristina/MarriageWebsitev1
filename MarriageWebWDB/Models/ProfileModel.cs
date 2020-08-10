using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessModel.Contracts;
using BusinessModel.Entities;

namespace MarriageWebWDB.Models
{
    public class ProfileModel
    {
        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Description { get; set; }

        public string Job { get; set; }

        public string Gender { get; set; }

        public string Orientation { get; set; }

        public string Religion { get; set; }

        public string Status { get; set; }

        public string Birthday { get; set; }

        public string Age { get; set; }

        public string Address { get; set; }

        public string Starsign { get; set; }

        public List<string> Likes { get; set; }

        public List<string> Dislikes { get; set; }

        public string Motto { get; set; }
    }
}