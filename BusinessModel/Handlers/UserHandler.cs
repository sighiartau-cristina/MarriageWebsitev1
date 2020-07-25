using System;
using System.Collections.Generic;
using System.Linq;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class UserHandler : IBusinessAccess<UserEntity>
    {

        public ResponseEntity<UserEntity> Add(UserEntity entity)
        {
            DbModel dbModel = new DbModel();
            USER dataEntity = null;

            if (entity == null)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            try
            {
                if (dbModel.USERS.Find(entity.UserId) == null)
                {
                    dataEntity = ConvertToDataEntity(entity);

                    if (dataEntity == null)
                    {
                        return new ResponseEntity<UserEntity>
                        {
                            CompletedRequest = false,
                            ErrorMessage = ErrorConstants.NullConvertedEntityError
                        };
                    }
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserGetError
                };
            }

            try
            {
                dbModel.USERS.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserInsertError
                };
            }

            return new ResponseEntity<UserEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(dataEntity)
            };
        }

        public ResponseEntity<UserEntity> Delete(int id)
        {
            DbModel dbModel = new DbModel();
            USER entity = null;

            try
            {
                entity = dbModel.USERS.Find(id);
                if (entity == null)
                {
                    return new ResponseEntity<UserEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.NullEntityError
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserGetError
                };
            }

            try
            {
                dbModel.USERS.Remove(entity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserDeleteError
                };
            }

            return new ResponseEntity<UserEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<UserEntity> Update(UserEntity entity)
        {

            if (entity == null)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            DbModel dbModel = new DbModel();
            USER dataEntity = null;

            try
            {
                dataEntity = dbModel.USERS.Find(entity.UserId);
                if (dataEntity == null)
                {
                    return new ResponseEntity<UserEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.UserNotFound
                    };
                }

            }
            catch (Exception)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserGetError
                };
            }

            try
            {
                if (!dataEntity.USER_EMAIL.Equals(entity.UserEmail) && CheckExistingEmail(entity.UserEmail))
                {
                    return new ResponseEntity<UserEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.ExistingEmailError
                    };
                }

                if (!dataEntity.USER_USERNAME.Equals(entity.UserUsername) && CheckExistingUsername(entity.UserUsername))
                {
                    return new ResponseEntity<UserEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.ExistingUsernameError
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserGetError
                };
            }

            try
            {
                dataEntity.USER_USERNAME = entity.UserUsername;
                dataEntity.USER_EMAIL = entity.UserEmail;
                dataEntity.USER_PASSWORD = entity.UserPassword;

                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserUpdateError
                };
            }

            return new ResponseEntity<UserEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<UserEntity> GetByUsername(string username)
        {
            DbModel dbModel = new DbModel();
            USER entity = null;

            try
            {
                entity = dbModel.USERS.Where(u => u.USER_USERNAME == username).FirstOrDefault();

                if (entity == null)
                {
                    return new ResponseEntity<UserEntity>
                    {
                        CompletedRequest = true
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserGetError
                };
            }

            return new ResponseEntity<UserEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<UserEntity> Get(int id)
        {
            DbModel dbModel = new DbModel();
            USER entity = null;

            try
            {
                entity = dbModel.USERS.Find(id);

                if (entity == null)
                {
                    return new ResponseEntity<UserEntity>
                    {
                        CompletedRequest = true
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserGetError
                };
            }

            return new ResponseEntity<UserEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<UserEntity> CheckUsernameAndPassword(string username, string password)
        {
            DbModel dbModel = new DbModel();
            USER entity = null;

            try
            {
                entity = dbModel.USERS.FirstOrDefault(e => e.USER_USERNAME == username && e.USER_PASSWORD == password);

                if (entity == null)
                {
                    return new ResponseEntity<UserEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.InvalidCredentials
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserGetError
                };
            }

            return new ResponseEntity<UserEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        public ResponseEntity<ICollection<UserEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool CheckExistingUsername(string username)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.USERS.FirstOrDefault(e => e.USER_USERNAME == username);
            return entity != null;
        }

        public bool CheckExistingEmail(string email)
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
            if (user == null)
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
