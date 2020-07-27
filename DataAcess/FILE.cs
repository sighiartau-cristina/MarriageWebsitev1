namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FILE")]
    public partial class FILE
    {
        public int FileId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        [StringLength(100)]
        public string ContentType { get; set; }

        [Required]
        public byte[] Content { get; set; }

        [Required]
        [StringLength(100)]
        public string FileType { get; set; }

        public int USER_PROFILE_ID { get; set; }

        public virtual USER_PROFILE USER_PROFILE { get; set; }
    }
}
