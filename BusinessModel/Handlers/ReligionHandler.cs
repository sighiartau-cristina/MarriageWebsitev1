using System;
using System.Collections.Generic;
using System.Linq;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class ReligionHandler : IBusinessAccess<ReligionEntity>
    {

        public ResponseEntity<ReligionEntity> Add(ReligionEntity entity)
        {
            DbModel dbModel = new DbModel();
            Religion dataEntity;

            if (entity == null)
            {
                return new ResponseEntity<ReligionEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            try
            {
                if (!CheckExisting(entity.ReligionName))
                {
                    dataEntity = ConvertToDataEntity(entity);
                    if (dataEntity == null)
                    {
                        return new ResponseEntity<ReligionEntity>
                        {
                            CompletedRequest = false,
                            ErrorMessage = ErrorConstants.NullConvertedEntityError
                        };
                    }
                }
                else
                {
                    return new ResponseEntity<ReligionEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.ReligionExisting
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<ReligionEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.ReligionGetError
                };
            }

            try
            {
                dbModel.Religions.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<ReligionEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.ReligionInsertError
                };
            }

            return new ResponseEntity<ReligionEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(dataEntity)
            };
        }

        public ResponseEntity<ReligionEntity> Delete(int id)
        {
            DbModel dbModel = new DbModel();
            Religion dataEntity;
            try
            {
                dataEntity = dbModel.Religions.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<ReligionEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.ReligionGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<ReligionEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.ReligionNotFound
                };
            }

            try
            {
                dbModel.Religions.Remove(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<ReligionEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.ReligionDeleteError
                };
            }

            return new ResponseEntity<ReligionEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<ReligionEntity> Update(ReligionEntity entity)
        {
            if (entity == null)
            {
                return new ResponseEntity<ReligionEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            DbModel dbModel = new DbModel();
            Religion dataEntity;

            try
            {
                dataEntity = dbModel.Religions.Find(entity.ReligionId);
            }
            catch (Exception)
            {
                return new ResponseEntity<ReligionEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.ReligionGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<ReligionEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.ReligionNotFound
                };
            }

            try
            {
                dataEntity.ReligionName = entity.ReligionName;
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<ReligionEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.ReligionUpdateError
                };
            }

            return new ResponseEntity<ReligionEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<ReligionEntity> Get(int id)
        {
            DbModel dbModel = new DbModel();
            Religion entity;
            try
            {
                entity = dbModel.Religions.Find(id);

            }
            catch (Exception)
            {
                return new ResponseEntity<ReligionEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.ReligionGetError
                };
            }
            if (entity == null)
            {
                return new ResponseEntity<ReligionEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.ReligionNotFound
                };
            }

            return new ResponseEntity<ReligionEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<ICollection<ReligionEntity>> GetAll()
        {
            DbModel dbModel = new DbModel();
            ICollection<ReligionEntity> list;

            try
            {
                list = dbModel.Religions.ToList().Select(x => ConvertToEntity(x)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<ReligionEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.ReligionGetError
                };
            }

            return new ResponseEntity<ICollection<ReligionEntity>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        private bool CheckExisting(string name)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.Religions.Where(e => e.ReligionName == name).FirstOrDefault();
            return entity != null;
        }

        private Religion ConvertToDataEntity(ReligionEntity religionEntity)
        {
            if (religionEntity == null)
            {
                return null;
            }

            return new Religion
            {
                ReligionName = religionEntity.ReligionName
            };
        }

        private ReligionEntity ConvertToEntity(Religion religion)
        {
            if (religion == null)
            {
                return null;
            }

            return new ReligionEntity
            {
                ReligionId = religion.ReligionId,
                ReligionName = religion.ReligionName
            };
        }
    }
}
