using System;
using System.Collections.Generic;
using System.Linq;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class OrientationHandler : IBusinessAccess<OrientationEntity>
    {

        public ResponseEntity<OrientationEntity> Add(OrientationEntity entity)
        {
            DbModel dbModel = new DbModel();
            ORIENTATION dataEntity;

            if (entity == null)
            {
                return new ResponseEntity<OrientationEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            try
            {
                if (!CheckExisting(entity.OrientationName))
                {
                    dataEntity = ConvertToDataEntity(entity);
                    if (dataEntity == null)
                    {
                        return new ResponseEntity<OrientationEntity>
                        {
                            CompletedRequest = false,
                            ErrorMessage = ErrorConstants.NullConvertedEntityError
                        };
                    }
                }
                else
                {
                    return new ResponseEntity<OrientationEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.OrientationExisting
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<OrientationEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.OrientationGetError
                };
            }

            try
            {
                dbModel.ORIENTATIONs.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<OrientationEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.OrientationInsertError
                };
            }

            return new ResponseEntity<OrientationEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(dataEntity)
            };
        }

        public ResponseEntity<OrientationEntity> Delete(int id)
        {
            DbModel dbModel = new DbModel();
            ORIENTATION dataEntity;
            try
            {
                dataEntity = dbModel.ORIENTATIONs.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<OrientationEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.OrientationGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<OrientationEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.OrientationNotFound
                };
            }

            try
            {
                dbModel.ORIENTATIONs.Remove(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<OrientationEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.OrientationDeleteError
                };
            }

            return new ResponseEntity<OrientationEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<OrientationEntity> Update(OrientationEntity entity)
        {
            if (entity == null)
            {
                return new ResponseEntity<OrientationEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            DbModel dbModel = new DbModel();
            ORIENTATION dataEntity;

            try
            {
                dataEntity = dbModel.ORIENTATIONs.Find(entity.OrientationId);
            }
            catch (Exception)
            {
                return new ResponseEntity<OrientationEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.OrientationGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<OrientationEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.OrientationNotFound
                };
            }

            try
            {
                dataEntity.ORIENT_NAME = entity.OrientationName;
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<OrientationEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.OrientationUpdateError
                };
            }

            return new ResponseEntity<OrientationEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<OrientationEntity> Get(int id)
        {
            DbModel dbModel = new DbModel();
            ORIENTATION entity;
            try
            {
                entity = dbModel.ORIENTATIONs.Find(id);

            }
            catch (Exception)
            {
                return new ResponseEntity<OrientationEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.OrientationGetError
                };
            }
            if (entity == null)
            {
                return new ResponseEntity<OrientationEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.OrientationNotFound
                };
            }

            return new ResponseEntity<OrientationEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<ICollection<OrientationEntity>> GetAll()
        {
            DbModel dbModel = new DbModel();
            ICollection<OrientationEntity> list;

            try
            {
                list = dbModel.ORIENTATIONs.ToList().Select(x => ConvertToEntity(x)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<OrientationEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.OrientationGetError
                };
            }

            return new ResponseEntity<ICollection<OrientationEntity>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }
        private bool CheckExisting(string name)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.ORIENTATIONs.Where(e => e.ORIENT_NAME == name).FirstOrDefault();
            return entity != null;
        }

        private ORIENTATION ConvertToDataEntity(OrientationEntity orientationEntity)
        {
            if (orientationEntity == null)
            {
                return null;
            }

            return new ORIENTATION
            {
                ORIENT_NAME = orientationEntity.OrientationName
            };
        }

        private OrientationEntity ConvertToEntity(ORIENTATION orientation)
        {
            if (orientation == null)
            {
                return null;
            }

            return new OrientationEntity
            {
                OrientationId = orientation.ORIENT_ID,
                OrientationName = orientation.ORIENT_NAME
            };
        }
    }
}
