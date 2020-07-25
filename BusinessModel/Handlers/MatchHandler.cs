using System;
using System.Collections.Generic;
using System.Linq;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    class MatchHandler : IBusinessAccess<MatchEntity>
    {
        public ResponseEntity<MatchEntity> Add(MatchEntity entity)
        {
            DbModel dbModel = new DbModel();
            MATCH dataEntity;

            if (entity == null)
            {
                return new ResponseEntity<MatchEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            try
            {
                if (!CheckExisting(entity))
                {
                    dataEntity = ConvertToDataEntity(entity);
                    if (dataEntity == null)
                    {
                        return new ResponseEntity<MatchEntity>
                        {
                            CompletedRequest = false,
                            ErrorMessage = ErrorConstants.NullConvertedEntityError
                        };
                    }
                }
                else
                {
                    return new ResponseEntity<MatchEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.MatchExisting
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<MatchEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchGetError
                };
            }

            try
            {
                dbModel.MATCHes.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<MatchEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchInsertError
                };
            }

            return new ResponseEntity<MatchEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(dataEntity)
            };
        }

        public ResponseEntity<MatchEntity> Delete(int id)
        {
            DbModel dbModel = new DbModel();
            MATCH dataEntity;
            try
            {
                dataEntity = dbModel.MATCHes.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<MatchEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<MatchEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchNotFound
                };
            }

            try
            {
                dbModel.MATCHes.Remove(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<MatchEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchDeleteError
                };
            }

            return new ResponseEntity<MatchEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<MatchEntity> Update(MatchEntity entity)
        {
            if (entity == null)
            {
                return new ResponseEntity<MatchEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            DbModel dbModel = new DbModel();
            MATCH dataEntity;

            try
            {
                dataEntity = dbModel.MATCHes.Find(entity.MatchId);
            }
            catch (Exception)
            {
                return new ResponseEntity<MatchEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<MatchEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchNotFound
                };
            }

            try
            {
                dataEntity.MATCH_ID = entity.MatchId;
                dataEntity.MATCH_USER_PROFILE_ID = entity.MatchUserProfileId;
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<MatchEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchUpdateError
                };
            }

            return new ResponseEntity<MatchEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<MatchEntity> Get(int id)
        {
            DbModel dbModel = new DbModel();
            MATCH entity;
            try
            {
                entity = dbModel.MATCHes.Find(id);

            }
            catch (Exception)
            {
                return new ResponseEntity<MatchEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchGetError
                };
            }
            if (entity == null)
            {
                return new ResponseEntity<MatchEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchNotFound
                };
            }

            return new ResponseEntity<MatchEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }


        public ResponseEntity<ICollection<MatchEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public ResponseEntity<ICollection<MatchEntity>> GetAllForUser(int userId)
        {
            DbModel dbModel = new DbModel();
            ICollection<MatchEntity> list;

            try
            {
                list = dbModel.MATCHes.Where(e => e.USER_PROFILE_ID == userId).ToList().Select(x => ConvertToEntity(x)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<MatchEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchGetError
                };
            }

            return new ResponseEntity<ICollection<MatchEntity>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        public ResponseEntity<ICollection<MatchEntity>> GetAllAgainstUser(int userId)
        {
            DbModel dbModel = new DbModel();
            ICollection<MatchEntity> list;

            try
            {
                list = dbModel.MATCHes.Where(e => e.MATCH_USER_PROFILE_ID == userId).ToList().Select(x => ConvertToEntity(x)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<MatchEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchGetError
                };
            }

            return new ResponseEntity<ICollection<MatchEntity>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        private bool CheckExisting(MatchEntity entity)
        {
            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.MATCHes.Where(e => e.MATCH_ID == entity.MatchId && e.USER_PROFILE_ID == entity.UserProfileId && e.MATCH_USER_PROFILE_ID == entity.MatchUserProfileId).FirstOrDefault();
            return dataEntity != null;
        }

        private MATCH ConvertToDataEntity(MatchEntity matchEntity)
        {
            if (matchEntity == null)
            {
                return null;
            }

            return new MATCH
            {
                USER_PROFILE_ID = matchEntity.UserProfileId,
                MATCH_USER_PROFILE_ID = matchEntity.MatchUserProfileId,
                MATCH_DATE = matchEntity.MatchDate
            };
        }

        private MatchEntity ConvertToEntity(MATCH match)
        {
            if (match == null)
            {
                return null;
            }

            return new MatchEntity
            {
                MatchDate = match.MATCH_DATE,
                MatchId = match.MATCH_ID,
                MatchUserProfileId = match.MATCH_USER_PROFILE_ID,
                UserProfileId = match.USER_PROFILE_ID
            };
        }

    }
}
