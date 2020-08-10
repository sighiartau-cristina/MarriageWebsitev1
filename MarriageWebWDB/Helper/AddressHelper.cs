using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;
using MarriageWebWDB.Validators;

namespace MarriageWebWDB.Helper
{
    public class AddressHelper
    {
        public string AddressMessage { get; private set; }

        public bool CheckAddress(AddressModel addressModel)
        {
            AddressValidator validator = new AddressValidator();
            var result = validator.Validate(addressModel);

            if (!result.IsValid)
            {
                AddressMessage = ErrorMessageGenerator.ComposeErrorMessage(result);
                return false;
            }

            return true;
        }

        public AddressModel GetAddressModel(int id, AddressModel addressModel = null)
        {
            if (addressModel == null)
            {
                addressModel = new AddressModel();
            }

            var address = new AddressHandler().Get(id);

            if (address.CompletedRequest)
            {
                addressModel.Street = address.Entity.AddressStreet;
                addressModel.StreetNo = address.Entity.AddressStreetNo;
                addressModel.City = address.Entity.AddressCity;
                addressModel.Country = address.Entity.AddressCountry;
            }

            return addressModel;
        }

        public AddressEntity ToDataEntity(int userProfileId, AddressModel addressModel)
        {
            return new AddressEntity
            {
                AddressStreet = addressModel.Street,
                AddressStreetNo = addressModel.StreetNo,
                AddressCity = addressModel.City,
                AddressCountry = addressModel.Country,
                UserProfileId = userProfileId
            };
        }

        public AddressEntity ToDataEntity(int addressId, int userProfileId, AddressModel addressModel)
        {
            return new AddressEntity
            {
                AddressStreet = addressModel.Street,
                AddressStreetNo = addressModel.StreetNo,
                AddressCity = addressModel.City,
                AddressCountry = addressModel.Country,
                UserProfileId = userProfileId,
                AddressId = addressId
            };
        }

        public bool NoChanges(AddressEntity address, AddressModel addressModel)
        {
            if(!string.Equals(address.AddressStreet, addressModel.Street))
            {
                return false;
            }

            if (!string.Equals(address.AddressStreetNo, addressModel.StreetNo))
            {
                return false;
            }

            if (!string.Equals(address.AddressCity, addressModel.City))
            {
                return false;
            }

            if (!string.Equals(address.AddressCountry, addressModel.Country))
            {
                return false;
            }

            return true;
        }
    }
}