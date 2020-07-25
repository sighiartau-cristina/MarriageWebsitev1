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
            RELIGION dataEntity;

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
                dbModel.RELIGIONs.Add(dataEntity);
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
            RELIGION dataEntity;
            try
            {
                dataEntity = dbModel.RELIGIONs.Find(id);
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
                dbModel.RELIGIONs.Remove(dataEntity);
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
            RELIGION dataEntity;

            try
            {
                dataEntity = dbModel.RELIGIONs.Find(entity.ReligionId);
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
                dataEntity.RELIGION_NAME = entity.ReligionName;
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
            RELIGION entity;
            try
            {
                entity = dbModel.RELIGIONs.Find(id);

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
                list = dbModel.RELIGIONs.ToList().Select(x => ConvertToEntity(x)).ToList();
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
            var entity = dbModel.RELIGIONs.Where(e => e.RELIGION_NAME == name).FirstOrDefault();
            return entity != null;
        }

        private RELIGION ConvertToDataEntity(ReligionEntity religionEntity)
        {
            if (religionEntity == null)
            {
                return null;
            }

            return new RELIGION
            {
                RELIGION_NAME = religionEntity.ReligionName
            };
        }

        private ReligionEntity ConvertToEntity(RELIGION religion)
        {
            if (religion == null)
            {
                return null;
            }

            return new ReligionEntity
            {
                ReligionId = religion.RELIGION_ID,
                ReligionName = religion.RELIGION_NAME
            };
        }
    }
}
