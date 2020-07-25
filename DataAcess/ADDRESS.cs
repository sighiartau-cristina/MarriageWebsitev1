namespace DataAccess
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ADDRESS")]
    public partial class ADDRESS
    {
        [Key]
        public int ADDRESS_ID { get; set; }

        [Required]
        [StringLength(25)]
        public string ADDRESS_STREET { get; set; }

        [Required]
        [StringLength(25)]
        public string ADDRESS_STREETNO { get; set; }

        [Required]
        [StringLength(25)]
        public string ADDRESS_CITY { get; set; }

        [Required]
        [StringLength(25)]
        public string ADDRESS_COUNTRY { get; set; }

        public int USER_PROFILE_ID { get; set; }

        public virtual USER_PROFILE USER_PROFILE { get; set; }
    }
}
