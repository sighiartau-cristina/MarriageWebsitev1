using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class FileEntityHandler
    {

        public void Add(FileEntity fileEntity)
        {
            DbModel dbModel = new DbModel();
            FILE file = ConvertToDataEntity(fileEntity);

            dbModel.FILEs.Add(file);
            dbModel.SaveChanges();
        }

        public FileEntity Get(int id)
        {
            DbModel dbModel = new DbModel();
            FILE file = dbModel.FILEs.Find(id);

            return ConvertToEntity(file);
        }

        public FileEntity GetByUserId(int id)
        {
            DbModel dbModel = new DbModel();
            FILE file = dbModel.FILEs.Where(f => f.USER_PROFILE_ID == id).FirstOrDefault();

            return ConvertToEntity(file);
        }

        private FILE ConvertToDataEntity(FileEntity fileEntity)
        {
            if (fileEntity == null)
            {
                return null;
            }

            return new FILE
            {
                USER_PROFILE_ID = fileEntity.UserProfileId,
               // FileId = fileEntity.FileId,
                FileName = fileEntity.FileName,
                FileType = fileEntity.FileType.ToString(),
                Content = fileEntity.Content,
                ContentType = fileEntity.ContentType
            };
        }

        private FileEntity ConvertToEntity(FILE file)
        {
            if (file == null)
            {
                return null;
            }

            //var result;
            Enum.TryParse(file.FileType, out FileType result);

            return new FileEntity
            {
                FileId = file.FileId,
                FileName = file.FileName,
                FileType = result,
                Content = file.Content,
                ContentType = file.ContentType,
                UserProfileId = file.USER_PROFILE_ID
            };
        }
    }
}
