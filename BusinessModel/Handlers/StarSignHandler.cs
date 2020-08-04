using System;
using System.Collections.Generic;
using System.Linq;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class StarSignHandler : IBusinessAccess<StarSignEntity>
    {

        public ResponseEntity<StarSignEntity> Add(StarSignEntity entity)
        {
            DbModel dbModel = new DbModel();
            Starsign dataEntity;

            if (entity == null)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            try
            {
                if (!CheckExisting(entity.SignName))
                {
                    dataEntity = ConvertToDataEntity(entity);
                    if (dataEntity == null)
                    {
                        return new ResponseEntity<StarSignEntity>
                        {
                            CompletedRequest = false,
                            ErrorMessage = ErrorConstants.NullConvertedEntityError
                        };
                    }
                }
                else
                {
                    return new ResponseEntity<StarSignEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.StarsignExisting
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignGetError
                };
            }

            try
            {
                dbModel.Starsigns.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignInsertError
                };
            }

            return new ResponseEntity<StarSignEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(dataEntity)
            };
        }

        public ResponseEntity<StarSignEntity> Delete(int id)
        {
            DbModel dbModel = new DbModel();
            Starsign dataEntity;
            try
            {
                dataEntity = dbModel.Starsigns.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignNotFound
                };
            }

            try
            {
                dbModel.Starsigns.Remove(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignDeleteError
                };
            }

            return new ResponseEntity<StarSignEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<StarSignEntity> Update(StarSignEntity entity)
        {
            if (entity == null)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            DbModel dbModel = new DbModel();
            Starsign dataEntity;

            try
            {
                dataEntity = dbModel.Starsigns.Find(entity.SignId);
            }
            catch (Exception)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignNotFound
                };
            }

            try
            {
                dataEntity.SignName = entity.SignName;
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignUpdateError
                };
            }

            return new ResponseEntity<StarSignEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<StarSignEntity> Get(int id)
        {
            DbModel dbModel = new DbModel();
            Starsign entity;
            try
            {
                entity = dbModel.Starsigns.Find(id);

            }
            catch (Exception)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignGetError
                };
            }
            if (entity == null)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignNotFound
                };
            }

            return new ResponseEntity<StarSignEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<StarSignEntity> GetByName(string name)
        {
            DbModel dbModel = new DbModel();
            Starsign entity;
            try
            {
                entity = dbModel.Starsigns.Where(s => s.SignName == name).FirstOrDefault();

            }
            catch (Exception)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignGetError
                };
            }
            if (entity == null)
            {
                return new ResponseEntity<StarSignEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignNotFound
                };
            }

            return new ResponseEntity<StarSignEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<ICollection<StarSignEntity>> GetAll()
        {
            DbModel dbModel = new DbModel();
            ICollection<StarSignEntity> list;

            try
            {
                list = dbModel.Starsigns.ToList().Select(x => ConvertToEntity(x)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<StarSignEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.StarsignGetError
                };
            }

            return new ResponseEntity<ICollection<StarSignEntity>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        private bool CheckExisting(string name)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.Starsigns.Where(e => e.SignName == name).FirstOrDefault();
            return entity != null;
        }

        private Starsign ConvertToDataEntity(StarSignEntity starSignEntity)
        {
            if (starSignEntity == null)
            {
                return null;
            }

            return new Starsign
            {
                SignName = starSignEntity.SignName
            };
        }

        private StarSignEntity ConvertToEntity(Starsign starSign)
        {
            if (starSign == null)
            {
                return null;
            }

            return new StarSignEntity
            {
                SignId = starSign.SignId,
                SignName = starSign.SignName
            };
        }

    }
}
