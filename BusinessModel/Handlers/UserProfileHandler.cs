using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Entities;
using DataAccess;
using BusinessModel.Contracts;

namespace BusinessModel.Handlers
{
    public class UserProfileHandler: IDataAccess<UserProfileEntity>
    {
        public int Add(UserProfileEntity entity)
        {
            DbModel dbModel = new DbModel();

            if (entity == null)
            {
                return -1;
            }

            if (!CheckExisting(entity.UserId))
            {
                var dataEntity = ConvertToDataEntity(entity);
                if (dataEntity == null)
                {
                    return -1;
                }

                dbModel.USER_PROFILE.Add(dataEntity);
                dbModel.SaveChanges();
                return dataEntity.GENDER_ID;
            }

            return -1;
        }

        public void Delete(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.USER_PROFILE.Find(id);

            if (entity == null)
            {
                return;
            }

            dbModel.USER_PROFILE.Remove(entity);
            dbModel.SaveChanges();
        }

        public void Update(UserProfileEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.USER_PROFILE.Find(entity.UserProfileId);

            if (dataEntity == null)
            {
                return;
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

            dbModel.SaveChanges();
        }

        public UserProfileEntity Get(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.USER_PROFILE.Where(e => e.USER_ID == id).FirstOrDefault();

            if (entity == null)
            {
                return null;
            }

            return ConvertToEntity(entity);
        }

        private bool CheckExisting(int userId)
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

        private UserProfileEntity ConvertToEntity(USER_PROFILE userProfile)
        {
            if(userProfile == null)
            {
                return null;
            }

            return new UserProfileEntity
            {
                UserProfileId = userProfile.USRPROF_ID,
                UserId = userProfile.USER_ID,
                UserProfileName = userProfile.USRPROF_NAME,
                UserProfileSurname = userProfile.USRPROF_SURNAME,
                UserProfileJob = userProfile.USRPROF_JOB,
                UserProfileDescription = userProfile.USRPROF_DESCRIPTION,
                UserProfilePhone = userProfile.USRPROF_PHONE,
                UserProfileBirthday = userProfile.USRPROF_BIRTHDAY,
                OrientationId = userProfile.ORIENTATION_ID,
                GenderId = userProfile.GENDER_ID,
                ReligionId = userProfile.RELIGION_ID,
                StatusId = userProfile.STATUS_ID,
                UserAge = userProfile.USER_AGE
            };
        }
    }
}
