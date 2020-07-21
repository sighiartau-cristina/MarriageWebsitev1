using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
