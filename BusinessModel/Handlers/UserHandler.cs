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
            User dataEntity = null;

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
                if (dbModel.Users.Find(entity.UserId) == null)
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
                dbModel.Users.Add(dataEntity);
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
            User entity;

            try
            {
                entity = dbModel.Users.Find(id);
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
                dbModel.Users.Remove(entity);
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
            User dataEntity;

            try
            {
                dataEntity = dbModel.Users.Find(entity.UserId);
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
                if (!dataEntity.Email.Equals(entity.UserEmail) && CheckExistingEmail(entity.UserEmail))
                {
                    return new ResponseEntity<UserEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.ExistingEmailError
                    };
                }

                if (!dataEntity.Username.Equals(entity.UserUsername) && CheckExistingUsername(entity.UserUsername))
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
                dataEntity.Username = entity.UserUsername;
                dataEntity.Email = entity.UserEmail;
                dataEntity.Password = entity.UserPassword;

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
            User entity = null;

            try
            {
                entity = dbModel.Users.Where(u => u.Username == username).FirstOrDefault();

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
            User entity;

            try
            {
                entity = dbModel.Users.Find(id);

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
            User entity = null;

            try
            {
                entity = dbModel.Users.FirstOrDefault(e => e.Username == username && e.Password == password);

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
            var entity = dbModel.Users.FirstOrDefault(e => e.Username == username);
            return entity != null;
        }

        public bool CheckExistingEmail(string email)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.Users.Where(e => e.Email == email).FirstOrDefault();
            return entity != null;
        }

        private User ConvertToDataEntity(UserEntity userEntity)
        {
            if (userEntity == null)
            {
                return null;
            }

            return new User
            {
                Username = userEntity.UserUsername,
                Email = userEntity.UserEmail,
                Password = userEntity.UserPassword,
                CreatedAt = userEntity.CreatedAt
            };
        }

        private UserEntity ConvertToEntity(User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserEntity
            {
                UserId = user.UserID,
                UserEmail = user.Email,
                UserPassword = user.Password,
                UserUsername = user.Username,
                CreatedAt = user.CreatedAt
            };
        }
       

    }
}
