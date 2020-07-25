namespace DataAccess
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class MARITAL_STATUS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MARITAL_STATUS()
        {
            USER_PROFILE = new HashSet<USER_PROFILE>();
        }

        [Key]
        public int MRTSTS_ID { get; set; }

        [Required]
        [StringLength(25)]
        public string MRTSTS_NAME { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_PROFILE> USER_PROFILE { get; set; }
    }
}
