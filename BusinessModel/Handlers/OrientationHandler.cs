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
            Orientation dataEntity;

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
                dbModel.Orientations.Add(dataEntity);
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
            Orientation dataEntity;
            try
            {
                dataEntity = dbModel.Orientations.Find(id);
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
                dbModel.Orientations.Remove(dataEntity);
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
            Orientation dataEntity;

            try
            {
                dataEntity = dbModel.Orientations.Find(entity.OrientationId);
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
                dataEntity.OrientationName = entity.OrientationName;
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
            Orientation entity;
            try
            {
                entity = dbModel.Orientations.Find(id);

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
                list = dbModel.Orientations.ToList().Select(x => ConvertToEntity(x)).ToList();
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
            var entity = dbModel.Orientations.Where(e => e.OrientationName == name).FirstOrDefault();
            return entity != null;
        }

        private Orientation ConvertToDataEntity(OrientationEntity orientationEntity)
        {
            if (orientationEntity == null)
            {
                return null;
            }

            return new Orientation
            {
                OrientationName = orientationEntity.OrientationName
            };
        }

        private OrientationEntity ConvertToEntity(Orientation orientation)
        {
            if (orientation == null)
            {
                return null;
            }

            return new OrientationEntity
            {
                OrientationId = orientation.OrientationId,
                OrientationName = orientation.OrientationName
            };
        }
    }
}
