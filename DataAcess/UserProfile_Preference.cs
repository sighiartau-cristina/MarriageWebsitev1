namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserProfile_Preference
    {
        public int Id { get; set; }

        public int PrefId { get; set; }

        public int UserProfileId { get; set; }

        public bool Likes { get; set; }

        public virtual Preference Preference { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
