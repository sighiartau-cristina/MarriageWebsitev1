using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            Address dataEntity;

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
                dbModel.Addresses.Add(dataEntity);
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
            Address entity;

            try
            {
                entity = dbModel.Addresses.Find(id);

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
                dbModel.Addresses.Remove(entity);
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
            Address entity;

            try
            {
                entity = dbModel.Addresses.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<AddressEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageNotFound
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
            Address dataEntity;

            try
            {
                dataEntity = dbModel.Addresses.Find(entity.AddressId);

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
                dataEntity.AddressStreet = entity.AddressStreet;
                dataEntity.AddressStreetNo = entity.AddressStreetNo;
                dataEntity.AddressCity = entity.AddressCity;
                dataEntity.AddressCountry = entity.AddressCountry;

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
            Address entity;

            try
            {
                entity = dbModel.Addresses.Where(e => e.UserProfileId == id).FirstOrDefault();
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

        public ResponseEntity<AddressEntity> GetForUser(int id)
        {
            DbModel dbModel = new DbModel();
            Address entity;

            try
            {
                entity = dbModel.Database.SqlQuery<Address>("select a.* from UserProfile up join [User] u on up.UserId = u.UserID join Address a on a.UserProfileId = up.UserProfileId where @user=u.UserID;", new SqlParameter("user", id)).FirstOrDefault();

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

        public ResponseEntity<ICollection<AddressEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        private bool CheckExisting(int userProfileId)
        {
            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.Addresses.Where(e => e.UserProfileId == userProfileId).FirstOrDefault();
            return dataEntity != null;
        }

        private Address ConvertToDataEntity(AddressEntity addressEntity)
        {
            if (addressEntity == null)
            {
                return null;
            }

            return new Address
            {
                UserProfileId = addressEntity.UserProfileId,
                AddressStreet = addressEntity.AddressStreet,
                AddressStreetNo = addressEntity.AddressStreetNo,
                AddressCity = addressEntity.AddressCity,
                AddressCountry = addressEntity.AddressCountry
            };
        }

        private AddressEntity ConvertToEntity(Address address)
        {
            if (address == null)
            {
                return null;
            }

            return new AddressEntity
            {
                AddressId = address.AddressId,
                AddressStreet = address.AddressStreet,
                AddressStreetNo = address.AddressStreetNo,
                AddressCity = address.AddressCity,
                AddressCountry = address.AddressCountry,
                UserProfileId = address.UserProfileId
            };
        }
    }
}
