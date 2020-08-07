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
    public class MatchHandler : IBusinessAccess<MatchEntity>
    {
        public ResponseEntity<MatchEntity> Add(MatchEntity entity)
        {
            DbModel dbModel = new DbModel();
            Match dataEntity;

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
                dbModel.Matches.Add(dataEntity);
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
            Match dataEntity;
            try
            {
                dataEntity = dbModel.Matches.Find(id);
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
                dbModel.Matches.Remove(dataEntity);
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
            Match dataEntity;

            try
            {
                dataEntity = dbModel.Matches.Find(entity.MatchId);
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
                dataEntity.MatchId = entity.MatchId;
                dataEntity.MatchUserProfileId = entity.MatchUserProfileId;
                dataEntity.Accepted = entity.Accepted;
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
            Match entity;
            try
            {
                entity = dbModel.Matches.Find(id);

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

        public ResponseEntity<MatchEntity> UnmatchForUsers(int userProfileId, int userToMatchId)
        {
            DbModel dbModel = new DbModel();
            Match entity;
            try
            {
                entity = dbModel.Matches.Where(u => u.UserProfileId == userProfileId && u.MatchUserProfileId == userToMatchId).FirstOrDefault();

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

            try
            {
                entity.Accepted = false;
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

        public ResponseEntity<ICollection<MatchEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public ResponseEntity<ICollection<int>> GetAllForUserProfile(int id)
        {
            DbModel dbModel = new DbModel();
            List<int> list;

            try
            {
                list = dbModel.Database.SqlQuery<int>("getAcceptedMatches @user_profile", new SqlParameter("user_profile", id)).ToList();

            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<int>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchGetError
                };
            }

            return new ResponseEntity<ICollection<int>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        public ResponseEntity<ICollection<MatchEntity>> GetAllForUser(int userId)
        {
            DbModel dbModel = new DbModel();
            ICollection<MatchEntity> list;

            try
            {
                list = dbModel.Matches.Where(e => e.UserProfileId == userId).ToList().Select(x => ConvertToEntity(x)).ToList();
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
                list = dbModel.Matches.Where(e => e.MatchUserProfileId == userId).ToList().Select(x => ConvertToEntity(x)).ToList();
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

        public bool Matched(int userProfileId, int userProfileToMatchId)
        {
            DbModel dbModel = new DbModel();
            Match dataEntity;
            Match dataEntityToMatch;

            //TODO transform to stored procedure
            try
            {
                dataEntity = dbModel.Matches.Where(m => m.UserProfileId == userProfileId && m.MatchUserProfileId==userProfileToMatchId).FirstOrDefault();
                dataEntityToMatch = dbModel.Matches.Where(m => m.UserProfileId == userProfileToMatchId && m.MatchUserProfileId == userProfileId).FirstOrDefault();
            }
            catch (Exception)
            {
                return false;
            }

            if(dataEntity==null || dataEntityToMatch == null)
            {
                return false;
            }

            return true;
        }
        private bool CheckExisting(MatchEntity entity)
        {
            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.Matches.Where(e => e.MatchId == entity.MatchId && e.UserProfileId == entity.UserProfileId && e.MatchUserProfileId == entity.MatchUserProfileId).FirstOrDefault();
            return dataEntity != null;
        }

        private Match ConvertToDataEntity(MatchEntity matchEntity)
        {
            if (matchEntity == null)
            {
                return null;
            }

            return new Match
            {
                UserProfileId = matchEntity.UserProfileId,
                MatchUserProfileId = matchEntity.MatchUserProfileId,
                MatchDate = matchEntity.MatchDate,
                Accepted = matchEntity.Accepted
            };
        }

        private MatchEntity ConvertToEntity(Match match)
        {
            if (match == null)
            {
                return null;
            }

            return new MatchEntity
            {
                MatchDate = match.MatchDate,
                MatchId = match.MatchId,
                MatchUserProfileId = match.MatchUserProfileId,
                UserProfileId = match.UserProfileId,
                Accepted = (bool) match.Accepted
            };
        }

    }
}
