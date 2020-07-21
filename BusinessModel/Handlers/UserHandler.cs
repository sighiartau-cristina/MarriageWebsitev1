using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Entities;
using BusinessModel.Contracts;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class UserHandler: IDataAccess<UserEntity>
    {

        public int Add(UserEntity entity)
        {
            DbModel dbModel = new DbModel();

            if (entity == null)
            {
                return -1;
            }

            if (dbModel.USERS.Find(entity.UserId) == null)
            {
                var dataEntity = ConvertToDataEntity(entity);
                if (dataEntity == null)
                {
                    return -1;
                }

                dbModel.USERS.Add(dataEntity);
                dbModel.SaveChanges();
                return dataEntity.USER_ID;
            }

            return -1;
        }

        public void Delete(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.USERS.Find(id);

            if (entity == null)
            {
                return;
            }

            dbModel.USERS.Remove(entity);
            dbModel.SaveChanges();
        }

        public void Update(UserEntity entity)
        {

            if (entity == null)
            {
                return;
            }

            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.USERS.Find(entity.UserId);

            if (dataEntity == null)
            {
                return;
            }

            if (!dataEntity.USER_EMAIL.Equals(entity.UserEmail) && CheckExistingEmail(entity.UserEmail))
            {
                return;
            }

            if (!dataEntity.USER_USERNAME.Equals(entity.UserUsername) && CheckExistingUsername(entity.UserUsername))
            {
                return;
            }

            dataEntity.USER_USERNAME = entity.UserUsername;
            dataEntity.USER_EMAIL = entity.UserEmail;
            dataEntity.USER_PASSWORD = entity.UserPassword;

            dbModel.SaveChanges();
        }

        public UserEntity Get(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.USERS.Find(id);

            if (entity == null)
            {
                return null;
            }

            return ConvertToEntity(entity);
        }

        public UserEntity CheckUsernameAndPassword(string username, string password)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.USERS.FirstOrDefault( e=> e.USER_USERNAME==username && e.USER_PASSWORD==password);

            if(entity == null)
            {
                return null;
            }

            return ConvertToEntity(entity);

        }

        public UserEntity GetByUsername(string username)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.USERS.FirstOrDefault(e => e.USER_USERNAME == username);

            if (entity == null)
            {
                return null;
            }

            return ConvertToEntity(entity);
        }

        public UserEntity GetByUsernameOrEmail(string username, string email)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.USERS.FirstOrDefault(e => e.USER_USERNAME == username || e.USER_EMAIL == email);

            if (entity == null)
            {
                return null;
            }

            return ConvertToEntity(entity);
        }

        private bool CheckExistingUsername(string username)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.USERS.FirstOrDefault(e => e.USER_USERNAME == username);
            return entity != null;
        }

        private bool CheckExistingEmail(string email)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.USERS.Where(e => e.USER_EMAIL == email).FirstOrDefault();
            return entity != null;
        }

        private USER ConvertToDataEntity(UserEntity userEntity)
        {
            if (userEntity == null)
            {
                return null;
            }

            return new USER
            {
                USER_USERNAME = userEntity.UserUsername,
                USER_EMAIL = userEntity.UserEmail,
                USER_PASSWORD = userEntity.UserPassword,
                USER_CREATED_AT = userEntity.CreatedAt
            };
        }

        private UserEntity ConvertToEntity(USER user)
        {
            if(user == null)
            {
                return null;
            }

            return new UserEntity
            {
                UserId = user.USER_ID,
                UserEmail = user.USER_EMAIL,
                UserPassword = user.USER_PASSWORD,
                UserUsername = user.USER_USERNAME,
                CreatedAt = user.USER_CREATED_AT
            };
        }

    }
}
