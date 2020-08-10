using System;
using System.Collections.Generic;
using System.Linq;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class MaritalStatusHandler : IBusinessAccess<StatusEntity>
    {
        public ResponseEntity<StatusEntity> Add(StatusEntity entity)
        {
            DbModel dbModel = new DbModel();
            Status dataEntity;

            if (entity == null)
            {
                return new ResponseEntity<StatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            try
            {
                if (!CheckExisting(entity.StatusName))
                {
                    dataEntity = ConvertToDataEntity(entity);
                    if (dataEntity == null)
                    {
                        return new ResponseEntity<StatusEntity>
                        {
                            CompletedRequest = false,
                            ErrorMessage = ErrorConstants.NullConvertedEntityError
                        };
                    }
                }
                else
                {
                    return new ResponseEntity<StatusEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.MaritalStatusExisting
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<StatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusGetError
                };
            }

            try
            {
                dbModel.Status.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<StatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusInsertError
                };
            }

            return new ResponseEntity<StatusEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(dataEntity)
            };
        }

        public ResponseEntity<StatusEntity> Delete(int id)
        {
            DbModel dbModel = new DbModel();
            Status dataEntity;
            try
            {
                dataEntity = dbModel.Status.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<StatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<StatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusNotFound
                };
            }

            try
            {
                dbModel.Status.Remove(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<StatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusDeleteError
                };
            }

            return new ResponseEntity<StatusEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<StatusEntity> Update(StatusEntity entity)
        {
            if (entity == null)
            {
                return new ResponseEntity<StatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            DbModel dbModel = new DbModel();
            Status dataEntity;

            try
            {
                dataEntity = dbModel.Status.Find(entity.StatusId);
            }
            catch (Exception)
            {
                return new ResponseEntity<StatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<StatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusNotFound
                };
            }

            try
            {
                dataEntity.StatusName = entity.StatusName;
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<StatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusUpdateError
                };
            }

            return new ResponseEntity<StatusEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<StatusEntity> Get(int id)
        {
            DbModel dbModel = new DbModel();
            Status entity;
            try
            {
                entity = dbModel.Status.Find(id);

            }
            catch (Exception)
            {
                return new ResponseEntity<StatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusGetError
                };
            }
            if (entity == null)
            {
                return new ResponseEntity<StatusEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusNotFound
                };
            }

            return new ResponseEntity<StatusEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<ICollection<StatusEntity>> GetAll()
        {
            DbModel dbModel = new DbModel();
            ICollection<StatusEntity> list;

            try
            {
                list = dbModel.Status.ToList().Select(x => ConvertToEntity(x)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<StatusEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MaritalStatusGetError
                };
            }

            return new ResponseEntity<ICollection<StatusEntity>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        private bool CheckExisting(string name)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.Status.Where(e => e.StatusName == name).FirstOrDefault();
            return entity != null;
        }

        private Status ConvertToDataEntity(StatusEntity maritalStatusEntity)
        {
            if (maritalStatusEntity == null)
            {
                return null;
            }

            return new Status
            {
                StatusName = maritalStatusEntity.StatusName
            };
        }

        private StatusEntity ConvertToEntity(Status maritalStatus)
        {
            if (maritalStatus == null)
            {
                return null;
            }

            return new StatusEntity
            {
                StatusId = maritalStatus.StatusId,
                StatusName = maritalStatus.StatusName
            };
        }
    }
}
