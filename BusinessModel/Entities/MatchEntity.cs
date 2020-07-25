using System;

namespace BusinessModel.Entities
{
    class MatchEntity
    {
        public int MatchId { get; set; }

        public int UserProfileId { get; set; }

        public int MatchUserProfileId { get; set; }

        public DateTime MatchDate { get; set; }
    }
}
