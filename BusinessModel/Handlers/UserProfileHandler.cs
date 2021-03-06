﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class UserProfileHandler : IBusinessAccess<UserProfileEntity>
    {
        public ResponseEntity<UserProfileEntity> Add(UserProfileEntity entity)
        {
            DbModel dbModel = new DbModel();
            UserProfile dataEntity;

            if (entity == null)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            try
            {
                if (!CheckExisting(entity.UserId))
                {
                    dataEntity = ConvertToDataEntity(entity);

                    if (dataEntity == null)
                    {
                        return new ResponseEntity<UserProfileEntity>
                        {
                            CompletedRequest = false,
                            ErrorMessage = ErrorConstants.NullConvertedEntityError
                        };
                    }
                }
                else
                {
                    return new ResponseEntity<UserProfileEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.UserProfileExisting
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileGetError
                };
            }

            try
            {
                dbModel.UserProfiles.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileInsertError
                };
            }

            return new ResponseEntity<UserProfileEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(dataEntity)
            };
        }

        public ResponseEntity<UserProfileEntity> Delete(int id)
        {
            DbModel dbModel = new DbModel();
            UserProfile entity;

            try
            {
                entity = dbModel.UserProfiles.Find(id);

                if (entity == null)
                {
                    return new ResponseEntity<UserProfileEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.NullEntityError
                    };
                }

            }
            catch (Exception)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileGetError
                };
            }

            try
            {
                dbModel.UserProfiles.Remove(entity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileDeleteError
                };
            }

            return new ResponseEntity<UserProfileEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<UserProfileEntity> Update(UserProfileEntity entity)
        {
            if (entity == null)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            DbModel dbModel = new DbModel();
            UserProfile dataEntity;

            try
            {
                dataEntity = dbModel.UserProfiles.Find(entity.UserProfileId);

                if (dataEntity == null)
                {
                    return new ResponseEntity<UserProfileEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.UserProfileNotFound
                    };
                }

            }
            catch (Exception)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileGetError
                };
            }

            dataEntity.Job = entity.UserProfileJob;
            dataEntity.Name = entity.UserProfileName;
            dataEntity.Surname = entity.UserProfileSurname;
            dataEntity.Description = entity.UserProfileDescription;
            dataEntity.Phone = entity.UserProfilePhone;
            dataEntity.Birthday = entity.UserProfileBirthday;
            dataEntity.StatusId = entity.StatusId;
            dataEntity.GenderId = entity.GenderId;
            dataEntity.OrientationId = entity.OrientationId;
            dataEntity.ReligionId = entity.ReligionId;
            dataEntity.Age = entity.UserAge;
            dataEntity.StarSignId = entity.StarsignId;
            dataEntity.Motto = entity.Motto;

            try
            {
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileUpdateError
                };
            }

            return new ResponseEntity<UserProfileEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<ICollection<UserProfileEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public ResponseEntity<UserProfileEntity> GetByUserId(int id)
        {
            DbModel dbModel = new DbModel();
            UserProfile entity;

            try
            {
                entity = dbModel.UserProfiles.Where(e => e.UserId == id).FirstOrDefault();
            }
            catch (Exception)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileGetError
                };
            }

            if (entity == null)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileNotFound
                };
            }

            return new ResponseEntity<UserProfileEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<UserProfileEntity> Get(int id)
        {
            DbModel dbModel = new DbModel();
            UserProfile entity;

            try
            {
                entity = dbModel.UserProfiles.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileGetError
                };
            }

            if (entity == null)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileNotFound
                };
            }

            return new ResponseEntity<UserProfileEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        private bool CheckExisting(int userId)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.UserProfiles.Where(e => e.UserId == userId).FirstOrDefault();
            return entity != null;
        }

        private UserProfile ConvertToDataEntity(UserProfileEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new UserProfile
            {
                Job = entity.UserProfileJob,
                Name = entity.UserProfileName,
                Surname = entity.UserProfileSurname,
                Description = entity.UserProfileDescription,
                Phone = entity.UserProfilePhone,
                Birthday = entity.UserProfileBirthday,
                UserId = entity.UserId,
                OrientationId = entity.OrientationId,
                GenderId = entity.GenderId,
                ReligionId = entity.ReligionId,
                StatusId = entity.StatusId,
                Age = entity.UserAge,
                Motto = entity.Motto,
                StarSignId = entity.StarsignId
            };
        }

        private UserProfileEntity ConvertToEntity(UserProfile dataEntity)
        {
            if (dataEntity == null)
            {
                return null;
            }

            return new UserProfileEntity
            {
                UserProfileId = dataEntity.UserProfileId,

                UserProfileJob = dataEntity.Job,
                UserProfileName = dataEntity.Name,
                UserProfileSurname = dataEntity.Surname,
                UserProfileDescription = dataEntity.Description,
                UserProfilePhone = dataEntity.Phone,
                UserProfileBirthday = dataEntity.Birthday,
                UserId = dataEntity.UserId,
                OrientationId = dataEntity.OrientationId,
                GenderId = dataEntity.GenderId,
                ReligionId = dataEntity.ReligionId,
                StatusId = dataEntity.StatusId,
                UserAge = dataEntity.Age,
                StarsignId = dataEntity.StarSignId,
                Motto = dataEntity.Motto
            };
        }

        public ResponseEntity<ICollection<int>> GetSuggestions(int userProfileId)
        {
            DbModel dbModel = new DbModel();
            List<int> list;

            try
            {
                list = dbModel.Database.SqlQuery<int>("getSuggestions @user_profile", new SqlParameter("user_profile", userProfileId)).ToList();

            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<int>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileGetError
                };
            }

            return new ResponseEntity<ICollection<int>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        public ResponseEntity<UserProfileEntity> GetByUsername(string username)
        {
            DbModel dbModel = new DbModel();
            UserProfile userProfile;

            try
            {
                userProfile = dbModel.Database.SqlQuery<UserProfile>("select up.* from UserProfile up join [User] u on up.UserId=u.UserID where u.Username=@username;", new SqlParameter("username", username)).FirstOrDefault();
            }
            catch (Exception)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileGetError
                };
            }

            if (userProfile == null)
            {
                return new ResponseEntity<UserProfileEntity>
                {
                    CompletedRequest = true,
                    ErrorMessage = ErrorConstants.UserProfileNotFound
                };
            }

            return new ResponseEntity<UserProfileEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(userProfile)
            };
        }

    }
}
