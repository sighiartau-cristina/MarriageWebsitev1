namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RELIGION")]
    public partial class RELIGION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RELIGION()
        {
            USER_PROFILE = new HashSet<USER_PROFILE>();
        }

        [Key]
        public int RELIGION_ID { get; set; }

        [Required]
        [StringLength(25)]
        public string RELIGION_NAME { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_PROFILE> USER_PROFILE { get; set; }
    }
}
