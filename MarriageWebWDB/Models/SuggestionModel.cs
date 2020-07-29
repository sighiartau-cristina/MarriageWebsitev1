using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessModel.Entities;

namespace MarriageWebWDB.Models
{
    public class SuggestionModel
    {
        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Description { get; set; }

        public string Gender { get; set; }

        public string Orientation { get; set; }

        public string Religion { get; set; }

        public string Status { get; set; }

        public string Age { get; set; }

        public FileEntity File { get; set; }
    }
}
