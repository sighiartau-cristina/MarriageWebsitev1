namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Match")]
    public partial class Match
    {
        public int MatchId { get; set; }

        public int UserProfileId { get; set; }

        public int MatchUserProfileId { get; set; }

        public DateTime MatchDate { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual UserProfile UserProfile1 { get; set; }
    }
}
