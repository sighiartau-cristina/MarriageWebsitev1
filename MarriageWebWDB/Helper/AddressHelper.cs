using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public bool AddAddress(int userProfile, AddressModel addressModel)
        {

            //UserEntity user = new UserHandler().GetByUsername(username);

           /* if(user == null)
            {
                return false;
            }*/

            AddressEntity address = ToDataEntity(userProfile, addressModel);

            if (address == null)
            {
                return false;
            }

            AddressHandler addressHandler = new AddressHandler();
            if (addressHandler.Add(address) == -1)
            {
                return false;
            }

            return true;
        }

        // public bool DeleteAddress()

        public AddressModel GetAddressModel(int id, AddressModel addressModel = null)
        {
            if (addressModel == null)
            {
                addressModel = new AddressModel();
            }
            
            AddressHandler addressHandler = new AddressHandler();
            AddressEntity address = addressHandler.Get(id);

            if (address != null)
            {
                addressModel.Street = address.AddressStreet;
                addressModel.StreetNo = address.AddressStreetNo;
                addressModel.City = address.AddressCity;
                addressModel.Country = address.AddressCountry;
            }
            //TODO check user and userProfile

            //PopulateModel(addressModel, user, userProfile);

            return addressModel;
        }

        private AddressEntity ToDataEntity(int userProfileId, AddressModel addressModel)
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
        public AddressEntity ToDataEntity (int addressId, int userProfileId, AddressModel addressModel)
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
    }
}