namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Address")]
    public partial class Address
    {
        public int AddressId { get; set; }

        [Required]
        [StringLength(25)]
        public string AddressStreet { get; set; }

        [Required]
        [StringLength(25)]
        public string AddressStreetNo { get; set; }

        [Required]
        [StringLength(25)]
        public string AddressCity { get; set; }

        [Required]
        [StringLength(25)]
        public string AddressCountry { get; set; }

        public int UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
