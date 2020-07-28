﻿using System;
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
                MatchDate = matchEntity.MatchDate
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
                UserProfileId = match.UserProfileId
            };
        }

    }
}
