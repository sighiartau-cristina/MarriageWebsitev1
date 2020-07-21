namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USER_PROFILE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER_PROFILE()
        {
            ADDRESSes = new HashSet<ADDRESS>();
            MATCHes = new HashSet<MATCH>();
            MATCHes1 = new HashSet<MATCH>();
        }

        [Key]
        public int USRPROF_ID { get; set; }

        [Required]
        [StringLength(25)]
        public string USRPROF_NAME { get; set; }

        [Required]
        [StringLength(25)]
        public string USRPROF_SURNAME { get; set; }

        [StringLength(25)]
        public string USRPROF_PHONE { get; set; }

        [StringLength(255)]
        public string USRPROF_DESCRIPTION { get; set; }

        [StringLength(50)]
        public string USRPROF_JOB { get; set; }

        [Column(TypeName = "date")]
        public DateTime USRPROF_BIRTHDAY { get; set; }

        public int RELIGION_ID { get; set; }

        public int STATUS_ID { get; set; }

        public int ORIENTATION_ID { get; set; }

        public int GENDER_ID { get; set; }

        public int USER_AGE { get; set; }

        public int USER_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADDRESS> ADDRESSes { get; set; }

        public virtual GENDER GENDER { get; set; }

        public virtual MARITAL_STATUS MARITAL_STATUS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MATCH> MATCHes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MATCH> MATCHes1 { get; set; }

        public virtual ORIENTATION ORIENTATION { get; set; }

        public virtual RELIGION RELIGION { get; set; }

        public virtual USER USER { get; set; }
    }
}
