namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Message
    {
        public int MessageId { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        [Required]
        [StringLength(255)]
        public string MessageText { get; set; }

        public DateTime SendDate { get; set; }

        public DateTime? ReadDate { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual UserProfile UserProfile1 { get; set; }
    }
}
