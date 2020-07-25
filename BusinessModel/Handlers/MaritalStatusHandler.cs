using System;
using System.Collections.Generic;
using System.Linq;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class MaritalStatusHandler : IBusinessAccess<MaritalStatusEntity>
    {


        public ResponseEntity<MaritalStatusEntity> Add(MaritalStatusEntity entity)
        {
            DbModel dbModel = new DbModel();
            MARITAL_STATUS dataEntity;

            if (entity == null)
            {
                return new ResponseEntity<MaritalStatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            try
            {
                if (!CheckExisting(entity.MaritalStatusName))
                {
                    dataEntity = ConvertToDataEntity(entity);
                    if (dataEntity == null)
                    {
                        return new ResponseEntity<MaritalStatusEntity>
                        {
                            CompletedRequest = false,
                            ErrorMessage = ErrorConstants.NullConvertedEntityError
                        };
                    }
                }
                else
                {
                    return new ResponseEntity<MaritalStatusEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.MaritalStatusExisting
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<MaritalStatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusGetError
                };
            }

            try
            {
                dbModel.MARITAL_STATUS.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<MaritalStatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusInsertError
                };
            }

            return new ResponseEntity<MaritalStatusEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(dataEntity)
            };
        }

        public ResponseEntity<MaritalStatusEntity> Delete(int id)
        {
            DbModel dbModel = new DbModel();
            MARITAL_STATUS dataEntity;
            try
            {
                dataEntity = dbModel.MARITAL_STATUS.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<MaritalStatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<MaritalStatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusNotFound
                };
            }

            try
            {
                dbModel.MARITAL_STATUS.Remove(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<MaritalStatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusDeleteError
                };
            }

            return new ResponseEntity<MaritalStatusEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<MaritalStatusEntity> Update(MaritalStatusEntity entity)
        {
            if (entity == null)
            {
                return new ResponseEntity<MaritalStatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            DbModel dbModel = new DbModel();
            MARITAL_STATUS dataEntity;

            try
            {
                dataEntity = dbModel.MARITAL_STATUS.Find(entity.MaritalStatusId);
            }
            catch (Exception)
            {
                return new ResponseEntity<MaritalStatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<MaritalStatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusNotFound
                };
            }

            try
            {
                dataEntity.MRTSTS_NAME = entity.MaritalStatusName;
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<MaritalStatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusUpdateError
                };
            }

            return new ResponseEntity<MaritalStatusEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<MaritalStatusEntity> Get(int id)
        {
            DbModel dbModel = new DbModel();
            MARITAL_STATUS entity;
            try
            {
                entity = dbModel.MARITAL_STATUS.Find(id);

            }
            catch (Exception)
            {
                return new ResponseEntity<MaritalStatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusGetError
                };
            }
            if (entity == null)
            {
                return new ResponseEntity<MaritalStatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusNotFound
                };
            }

            return new ResponseEntity<MaritalStatusEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<ICollection<MaritalStatusEntity>> GetAll()
        {
            DbModel dbModel = new DbModel();
            ICollection<MaritalStatusEntity> list;

            try
            {
                list = dbModel.MARITAL_STATUS.ToList().Select(x => ConvertToEntity(x)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<MaritalStatusEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusGetError
                };
            }

            return new ResponseEntity<ICollection<MaritalStatusEntity>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }
        private bool CheckExisting(string name)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.MARITAL_STATUS.Where(e => e.MRTSTS_NAME == name).FirstOrDefault();
            return entity != null;
        }

        private MARITAL_STATUS ConvertToDataEntity(MaritalStatusEntity maritalStatusEntity)
        {
            if (maritalStatusEntity == null)
            {
                return null;
            }

            return new MARITAL_STATUS
            {
                MRTSTS_NAME = maritalStatusEntity.MaritalStatusName
            };
        }

        private MaritalStatusEntity ConvertToEntity(MARITAL_STATUS maritalStatus)
        {
            if (maritalStatus == null)
            {
                return null;
            }

            return new MaritalStatusEntity
            {
                MaritalStatusId = maritalStatus.MRTSTS_ID,
                MaritalStatusName = maritalStatus.MRTSTS_NAME
            };
        }
    }
}
