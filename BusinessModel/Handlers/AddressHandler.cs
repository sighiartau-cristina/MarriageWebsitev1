using System;
using System.Collections.Generic;
using System.Linq;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class AddressHandler : IBusinessAccess<AddressEntity>
    {
        public ResponseEntity<AddressEntity> Add(AddressEntity entity)
        {
            DbModel dbModel = new DbModel();
            ADDRESS dataEntity;

            if (entity == null)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            try
            {
                if (!CheckExisting(entity.UserProfileId))
                {
                    dataEntity = ConvertToDataEntity(entity);
                    if (dataEntity == null)
                    {
                        return new ResponseEntity<AddressEntity>
                        {
                            CompletedRequest = false,
                            ErrorMessage = ErrorConstants.NullConvertedEntityError
                        };
                    }
                }
                else
                {
                    return new ResponseEntity<AddressEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.AddressExisting
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.AddressGetError
                };
            }

            try
            {
                dbModel.ADDRESSes.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.AddressInsertError
                };
            }

            return new ResponseEntity<AddressEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(dataEntity)
            };
        }

        public ResponseEntity<AddressEntity> Delete(int id)
        {
            DbModel dbModel = new DbModel();
            ADDRESS entity;

            try
            {
                entity = dbModel.ADDRESSes.Find(id);

                if (entity == null)
                {
                    return new ResponseEntity<AddressEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.AddressNotFound
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.AddressGetError
                };
            }

            try
            {
                dbModel.ADDRESSes.Remove(entity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.AddressDeleteError
                };
            }

            return new ResponseEntity<AddressEntity>
            {
                CompletedRequest = true
            };

        }

        public ResponseEntity<AddressEntity> Get(int id)
        {
            DbModel dbModel = new DbModel();
            ADDRESS entity;

            try
            {
                entity = dbModel.ADDRESSes.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = true
                };
            }


            if (entity == null)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.AddressNotFound
                };
            }

            return new ResponseEntity<AddressEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<AddressEntity> Update(AddressEntity entity)
        {
            if (entity == null)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            DbModel dbModel = new DbModel();
            ADDRESS dataEntity;

            try
            {
                dataEntity = dbModel.ADDRESSes.Find(entity.AddressId);

                if (dataEntity == null)
                {
                    return new ResponseEntity<AddressEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.AddressNotFound
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.AddressGetError
                };
            }

            try
            {
                dataEntity.ADDRESS_STREET = entity.AddressStreet;
                dataEntity.ADDRESS_STREETNO = entity.AddressStreetNo;
                dataEntity.ADDRESS_CITY = entity.AddressCity;
                dataEntity.ADDRESS_COUNTRY = entity.AddressCountry;

                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.AddressUpdateError
                };
            }

            return new ResponseEntity<AddressEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<AddressEntity> GetForUserProfile(int id)
        {
            DbModel dbModel = new DbModel();
            ADDRESS entity;

            try
            {
                entity = dbModel.ADDRESSes.Where(e => e.USER_PROFILE_ID == id).FirstOrDefault();
            }
            catch (Exception)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.AddressGetError
                };
            }
            if (entity == null)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = true
                };
            }

            return new ResponseEntity<AddressEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<ICollection<AddressEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        private bool CheckExisting(int userProfileId)
        {
            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.ADDRESSes.Where(e => e.USER_PROFILE_ID == userProfileId).FirstOrDefault();
            return dataEntity != null;
        }

        private ADDRESS ConvertToDataEntity(AddressEntity addressEntity)
        {
            if (addressEntity == null)
            {
                return null;
            }

            return new ADDRESS
            {
                USER_PROFILE_ID = addressEntity.UserProfileId,
                ADDRESS_STREET = addressEntity.AddressStreet,
                ADDRESS_STREETNO = addressEntity.AddressStreetNo,
                ADDRESS_CITY = addressEntity.AddressCity,
                ADDRESS_COUNTRY = addressEntity.AddressCountry
            };
        }

        private AddressEntity ConvertToEntity(ADDRESS address)
        {
            if (address == null)
            {
                return null;
            }

            return new AddressEntity
            {
                AddressId = address.ADDRESS_ID,
                AddressStreet = address.ADDRESS_STREET,
                AddressStreetNo = address.ADDRESS_STREETNO,
                AddressCity = address.ADDRESS_CITY,
                AddressCountry = address.ADDRESS_COUNTRY,
                UserProfileId = address.USER_PROFILE_ID
            };
        }

    }
}
