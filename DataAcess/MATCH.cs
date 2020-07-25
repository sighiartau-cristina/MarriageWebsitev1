namespace DataAccess
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATCH")]
    public partial class MATCH
    {
        [Key]
        public int MATCH_ID { get; set; }

        public int USER_PROFILE_ID { get; set; }

        public int MATCH_USER_PROFILE_ID { get; set; }

        public DateTime MATCH_DATE { get; set; }

        public virtual USER_PROFILE USER_PROFILE { get; set; }

        public virtual USER_PROFILE USER_PROFILE1 { get; set; }
    }
}
