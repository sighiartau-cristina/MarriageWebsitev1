namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USERS")]
    public partial class USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER()
        {
            USER_PROFILE = new HashSet<USER_PROFILE>();
        }

        [Key]
        public int USER_ID { get; set; }

        [Required]
        [StringLength(25)]
        public string USER_USERNAME { get; set; }

        [Required]
        [StringLength(25)]
        public string USER_EMAIL { get; set; }

        [Required]
        [StringLength(25)]
        public string USER_PASSWORD { get; set; }

        public DateTime USER_CREATED_AT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_PROFILE> USER_PROFILE { get; set; }
    }
}
