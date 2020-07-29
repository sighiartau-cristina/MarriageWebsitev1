using System;

namespace BusinessModel.Entities
{
    public class MatchEntity
    {
        public int MatchId { get; set; }

        public int UserProfileId { get; set; }

        public int MatchUserProfileId { get; set; }

        public bool Accepted { get; set; }

        public DateTime MatchDate { get; set; }
    }
}
