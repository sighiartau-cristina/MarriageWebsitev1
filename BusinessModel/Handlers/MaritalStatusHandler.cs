using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using BusinessModel.Entities;
using BusinessModel.Contracts;
using System.Data.Entity.Migrations;

namespace BusinessModel.Handlers
{
    public class MaritalStatusHandler : IDataAccess<MaritalStatusEntity>
    {

        public void Add(MaritalStatusEntity entity)
        {
            DbModel dbModel = new DbModel();

            if (entity == null)
            {
                return;
            }

            if (!CheckExisting(entity.MaritalStatusName))
            {
                var dataEntity = ConvertToDataEntity(entity);
                if (dataEntity == null)
                {
                    return;
                }

                dbModel.MARITAL_STATUS.Add(dataEntity);
                dbModel.SaveChanges();
            }

        }

        public void Delete(int id)
        {

            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.MARITAL_STATUS.Find(id);

            if (dataEntity == null)
            {
                return;
            }

            dbModel.MARITAL_STATUS.Remove(dataEntity);
            dbModel.SaveChanges();
        }

        public void Update(MaritalStatusEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.MARITAL_STATUS.Find(entity.MaritalStatusId);

            if (dataEntity == null)
            {
                return;
            }

            dataEntity.MRTSTS_NAME = entity.MaritalStatusName;
            dbModel.SaveChanges();
        }

        public MaritalStatusEntity Get(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.MARITAL_STATUS.Find(id);

            if (entity == null)
            {
                return null;
            }

            return ConvertToEntity(entity);
        }

        private bool CheckExisting(string name)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.MARITAL_STATUS.Where(e => e.MRTSTS_NAME == name).FirstOrDefault();
            return entity != null;
        }

        private MARITAL_STATUS ConvertToDataEntity(MaritalStatusEntity maritalStatusEntity)
        {
            if (maritalStatusEntity == null)
            {
                return null;
            }

            return new MARITAL_STATUS
            {
                MRTSTS_NAME = maritalStatusEntity.MaritalStatusName
            };
        }

        private MaritalStatusEntity ConvertToEntity(MARITAL_STATUS maritalStatus)
        {
            if (maritalStatus == null)
            {
                return null;
            }

            return new MaritalStatusEntity
            {
                MaritalStatusId = maritalStatus.MRTSTS_ID,
                MaritalStatusName = maritalStatus.MRTSTS_NAME
            };
        }
    }
}
