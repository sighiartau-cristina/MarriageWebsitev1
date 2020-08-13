using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class FileHandler : IBusinessAccess<FileEntity>
    {

        public ResponseEntity<FileEntity> Add(FileEntity fileEntity)
        {
            DbModel dbModel = new DbModel();
            File file; 

            if (fileEntity == null)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }
            
            file= ConvertToDataEntity(fileEntity);

            if (file == null)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullConvertedEntityError
                };
            }

            try
            {
                dbModel.Files.Add(file);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.FileInsertError
                };
            }

            return new ResponseEntity<FileEntity>
            {
                CompletedRequest = true,
                Entity = fileEntity
            };
        }

        public ResponseEntity<FileEntity> Delete(int id)
        {
            DbModel dbModel = new DbModel();
            File file;

            try
            {
                file = dbModel.Files.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.FileGetError
                };
            }

            if (file == null)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.FileNotFound
                };
            }

            try
            {
                dbModel.Files.Remove(file);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.FileDeleteError
                };
            }

            return new ResponseEntity<FileEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<FileEntity> Get(int id)
        {
            DbModel dbModel = new DbModel();
            File file;

            try
            {
                file = dbModel.Files.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.FileGetError
                };
            }

            if (file == null)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest =true
                };
            }

            FileEntity fileEntity = ConvertToEntity(file);

            if(fileEntity == null)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullConvertedEntityError
                };
            }

            return new ResponseEntity<FileEntity>
            {
                CompletedRequest = true,
                Entity = fileEntity
            };
        }

        public ResponseEntity<ICollection<FileEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public ResponseEntity<ICollection<int>> GetGalleryForUserProfile(int userProfileId)
        {
            DbModel dbModel = new DbModel();
            List<int> list;

            try
            {
                list = dbModel.Database.SqlQuery<int>("select f.FileId from Files f where f.FileType='Gallery' and f.UserProfileId=@user_profile;", new SqlParameter("user_profile", userProfileId)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<int>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.FileGetError
                };
            }

            return new ResponseEntity<ICollection<int>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        public ResponseEntity<FileEntity> GetAvatarForUserProfileId(int id)
        {
            DbModel dbModel = new DbModel();
            File file;

            try
            {
                file = dbModel.Files.Where(f => f.UserProfileId == id && f.FileType.Equals(FileType.Avatar.ToString())).FirstOrDefault();
            }
            catch (Exception)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.FileGetError
                };
            }

            if (file == null)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = true
                };
            }

            FileEntity fileEntity = ConvertToEntity(file);

            if (fileEntity == null)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullConvertedEntityError
                };
            }

            return new ResponseEntity<FileEntity>
            {
                CompletedRequest = true,
                Entity = fileEntity
            };
        }

        public ResponseEntity<FileEntity> Update(FileEntity entity)
        {
            DbModel dbModel = new DbModel();
            File file;

            try
            {
                file = dbModel.Files.Find(entity.FileId);
            }
            catch (Exception)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.FileGetError
                };
            }

            if (file == null)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.FileNotFound
                };
            }

            file.FileName = entity.FileName;
            file.FileType = entity.FileType.ToString();
            file.Content = entity.Content;
            file.ContentType = entity.ContentType;
            try
            {
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.FileUpdateError
                };
            }

            return new ResponseEntity<FileEntity>
            {
                CompletedRequest = true
            };
        }

        private File ConvertToDataEntity(FileEntity fileEntity)
        {
            if (fileEntity == null)
            {
                return null;
            }

            return new File
            {
                UserProfileId = fileEntity.UserProfileId,
               // FileId = fileEntity.FileId,
                FileName = fileEntity.FileName,
                FileType = fileEntity.FileType.ToString(),
                Content = fileEntity.Content,
                ContentType = fileEntity.ContentType
            };
        }

        private FileEntity ConvertToEntity(File file)
        {
            if (file == null)
            {
                return null;
            }

            Enum.TryParse(file.FileType, out FileType result);

            return new FileEntity
            {
                FileId = file.FileId,
                FileName = file.FileName,
                FileType = result,
                Content = file.Content,
                ContentType = file.ContentType,
                UserProfileId = file.UserProfileId
            };
        }

        public ResponseEntity<FileEntity> GetForUserUsername(string username)
        {
            DbModel dbModel = new DbModel();
            File file;

            try
            {
                file = dbModel.Database.SqlQuery<File>("select f.* from UserProfile up join [User] u on up.UserId = u.UserID join Files f on f.UserProfileId = up.UserProfileId where @user=u.Username;", new SqlParameter("user", username)).FirstOrDefault();
            }
            catch (Exception)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.FileGetError
                };
            }

            if (file == null)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = true
                };
            }

            FileEntity fileEntity = ConvertToEntity(file);

            if (fileEntity == null)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullConvertedEntityError
                };
            }

            return new ResponseEntity<FileEntity>
            {
                CompletedRequest = true,
                Entity = fileEntity
            };
        }

        public ResponseEntity<FileEntity> GetAvatarForUserUsername(string username)
        {
            DbModel dbModel = new DbModel();
            File file;

            try
            {
                file = dbModel.Database.SqlQuery<File>("select f.* from UserProfile up join [User] u on up.UserId = u.UserID join Files f on f.UserProfileId = up.UserProfileId where @user=u.Username and f.FileType='Avatar';", new SqlParameter("user", username)).FirstOrDefault();
            }
            catch (Exception)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.FileGetError
                };
            }

            if (file == null)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = true
                };
            }

            FileEntity fileEntity = ConvertToEntity(file);

            if (fileEntity == null)
            {
                return new ResponseEntity<FileEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullConvertedEntityError
                };
            }

            return new ResponseEntity<FileEntity>
            {
                CompletedRequest = true,
                Entity = fileEntity
            };
        }

    }
}
