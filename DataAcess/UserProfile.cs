namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserProfile")]
    public partial class UserProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserProfile()
        {
            Addresses = new HashSet<Address>();
            Files = new HashSet<File>();
            Matches = new HashSet<Match>();
            Matches1 = new HashSet<Match>();
            Messages = new HashSet<Message>();
            Messages1 = new HashSet<Message>();
        }

        public int UserProfileId { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [StringLength(25)]
        public string Surname { get; set; }

        [StringLength(25)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Job { get; set; }

        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; }

        public int ReligionId { get; set; }

        public int StatusId { get; set; }

        public int OrientationId { get; set; }

        public int GenderId { get; set; }

        public int Age { get; set; }

        [StringLength(150)]
        public string Motto { get; set; }

        [StringLength(255)]
        public string Likes { get; set; }

        [StringLength(255)]
        public string Dislikes { get; set; }

        public int StarSignId { get; set; }

        public int UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> Files { get; set; }

        public virtual Gender Gender { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Match> Matches { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Match> Matches1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages1 { get; set; }

        public virtual Orientation Orientation { get; set; }

        public virtual Religion Religion { get; set; }

        public virtual Starsign Starsign { get; set; }

        public virtual Status Status { get; set; }

        public virtual User User { get; set; }
    }
}
