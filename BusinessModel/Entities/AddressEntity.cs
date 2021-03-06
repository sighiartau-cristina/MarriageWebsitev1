﻿namespace BusinessModel.Entities
{
    public class AddressEntity
    {
        public int AddressId { get; set; }

        public int UserProfileId { get; set; }

        public string AddressStreet { get; set; }

        public string AddressStreetNo { get; set; }

        public string AddressCity { get; set; }

        public string AddressCountry { get; set; }
    }
}
