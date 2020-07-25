using System;
using System.Collections.Generic;
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
            USER_PROFILE dataEntity = null;

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
                bool registeredUser = CheckExistingProfileForUser(entity.UserId);
                if (!registeredUser)
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
                dbModel.USER_PROFILE.Add(dataEntity);
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
            USER_PROFILE entity = null;

            try
            {
                entity = dbModel.USER_PROFILE.Find(id);

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
                dbModel.USER_PROFILE.Remove(entity);
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
            USER_PROFILE dataEntity = null;

            try
            {
                dbModel.USER_PROFILE.Find(entity.UserProfileId);

                if (dataEntity == null)
                {
                    return new ResponseEntity<UserProfileEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.NullConvertedEntityError
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

            dataEntity.USRPROF_JOB = entity.UserProfileJob;
            dataEntity.USRPROF_NAME = entity.UserProfileName;
            dataEntity.USRPROF_SURNAME = entity.UserProfileSurname;
            dataEntity.USRPROF_DESCRIPTION = entity.UserProfileDescription;
            dataEntity.USRPROF_PHONE = entity.UserProfilePhone;
            dataEntity.USRPROF_BIRTHDAY = entity.UserProfileBirthday;
            dataEntity.STATUS_ID = entity.StatusId;
            dataEntity.GENDER_ID = entity.GenderId;
            dataEntity.ORIENTATION_ID = entity.OrientationId;
            dataEntity.RELIGION_ID = entity.ReligionId;
            dataEntity.USER_AGE = entity.UserAge;

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
            USER_PROFILE entity;

            try
            {
                entity = dbModel.USER_PROFILE.Where(e => e.USER_ID == id).FirstOrDefault();
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
            USER_PROFILE entity = null;

            try
            {
                entity = dbModel.USER_PROFILE.Find(id);
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

        private bool CheckExistingProfileForUser(int userId)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.USER_PROFILE.Where(e => e.USER_ID == userId).FirstOrDefault();
            return entity != null;
        }

        private USER_PROFILE ConvertToDataEntity(UserProfileEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new USER_PROFILE
            {
                USRPROF_JOB = entity.UserProfileJob,
                USRPROF_NAME = entity.UserProfileName,
                USRPROF_SURNAME = entity.UserProfileSurname,
                USRPROF_DESCRIPTION = entity.UserProfileDescription,
                USRPROF_PHONE = entity.UserProfilePhone,
                USRPROF_BIRTHDAY = entity.UserProfileBirthday,
                USER_ID = entity.UserId,
                ORIENTATION_ID = entity.OrientationId,
                GENDER_ID = entity.GenderId,
                RELIGION_ID = entity.ReligionId,
                STATUS_ID = entity.StatusId,
                USER_AGE = entity.UserAge
            };
        }

        private UserProfileEntity ConvertToEntity(USER_PROFILE dataEntity)
        {
            if (dataEntity == null)
            {
                return null;
            }

            return new UserProfileEntity
            {
                UserProfileId = dataEntity.USRPROF_ID,

                UserProfileJob = dataEntity.USRPROF_JOB,
                UserProfileName = dataEntity.USRPROF_NAME,
                UserProfileSurname = dataEntity.USRPROF_SURNAME,
                UserProfileDescription = dataEntity.USRPROF_DESCRIPTION,
                UserProfilePhone = dataEntity.USRPROF_PHONE,
                UserProfileBirthday = dataEntity.USRPROF_BIRTHDAY,
                UserId = dataEntity.USER_ID,
                OrientationId = dataEntity.ORIENTATION_ID,
                GenderId = dataEntity.GENDER_ID,
                ReligionId = dataEntity.RELIGION_ID,
                StatusId = dataEntity.STATUS_ID,
                UserAge = dataEntity.USER_AGE
            };
        }

    }
}
