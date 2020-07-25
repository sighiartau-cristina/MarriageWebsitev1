using System;
using System.Collections.Generic;
using System.Linq;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class GenderHandler : IBusinessAccess<GenderEntity>
    {

        public ResponseEntity<GenderEntity> Add(GenderEntity entity)
        {
            DbModel dbModel = new DbModel();
            GENDER dataEntity;

            if (entity == null)
            {
                return new ResponseEntity<GenderEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            try
            {
                if (!CheckExisting(entity.GenderName))
                {
                    dataEntity = ConvertToDataEntity(entity);
                    if (dataEntity == null)
                    {
                        return new ResponseEntity<GenderEntity>
                        {
                            CompletedRequest = false,
                            ErrorMessage = ErrorConstants.NullConvertedEntityError
                        };
                    }
                }
                else
                {
                    return new ResponseEntity<GenderEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.GenderExisting
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<GenderEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.GenderGetError
                };
            }

            try
            {
                dbModel.GENDERs.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<GenderEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.GenderInsertError
                };
            }

            return new ResponseEntity<GenderEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(dataEntity)
            };
        }

        public ResponseEntity<GenderEntity> Delete(int id)
        {
            DbModel dbModel = new DbModel();
            GENDER dataEntity;
            try
            {
                dataEntity = dbModel.GENDERs.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<GenderEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.GenderGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<GenderEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.GenderNotFound
                };
            }

            try
            {
                dbModel.GENDERs.Remove(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<GenderEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.GenderDeleteError
                };
            }

            return new ResponseEntity<GenderEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<GenderEntity> Update(GenderEntity entity)
        {
            if (entity == null)
            {
                return new ResponseEntity<GenderEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            DbModel dbModel = new DbModel();
            GENDER dataEntity;

            try
            {
                dataEntity = dbModel.GENDERs.Find(entity.GenderId);
            }
            catch (Exception)
            {
                return new ResponseEntity<GenderEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.GenderGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<GenderEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.GenderNotFound
                };
            }

            try
            {
                dataEntity.GENDER_NAME = entity.GenderName;
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<GenderEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.GenderUpdateError
                };
            }

            return new ResponseEntity<GenderEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<GenderEntity> Get(int id)
        {
            DbModel dbModel = new DbModel();
            GENDER entity;
            try
            {
                entity = dbModel.GENDERs.Find(id);

            }
            catch (Exception)
            {
                return new ResponseEntity<GenderEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.GenderGetError
                };
            }
            if (entity == null)
            {
                return new ResponseEntity<GenderEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.GenderNotFound
                };
            }

            return new ResponseEntity<GenderEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<ICollection<GenderEntity>> GetAll()
        {
            DbModel dbModel = new DbModel();
            ICollection<GenderEntity> list;

            try
            {
                list = dbModel.GENDERs.ToList().Select(x => ConvertToEntity(x)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<GenderEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.GenderGetError
                };
            }

            return new ResponseEntity<ICollection<GenderEntity>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        private bool CheckExisting(string name)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.GENDERs.Where(e => e.GENDER_NAME == name).FirstOrDefault();
            return entity != null;
        }

        private GENDER ConvertToDataEntity(GenderEntity genderEntity)
        {
            if (genderEntity == null)
            {
                return null;
            }

            return new GENDER
            {
                GENDER_NAME = genderEntity.GenderName
            };
        }

        private GenderEntity ConvertToEntity(GENDER gender)
        {
            if (gender == null)
            {
                return null;
            }

            return new GenderEntity
            {
                GenderId = gender.GENDER_ID,
                GenderName = gender.GENDER_NAME
            };
        }

    }
}
