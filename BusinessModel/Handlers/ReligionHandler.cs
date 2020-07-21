using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using BusinessModel.Entities;
using System.Data.Entity.Migrations;
using BusinessModel.Contracts;

namespace BusinessModel.Handlers
{
    public class ReligionHandler: IDataAccess<ReligionEntity>
    {
        public int Add(ReligionEntity entity)
        {
            DbModel dbModel = new DbModel();

            if (entity == null)
            {
                return -1;
            }

            if (!CheckExisting(entity.ReligionName))
            {
                var dataEntity = ConvertToDataEntity(entity);
                if (dataEntity == null)
                {
                    return -1;
                }

                dbModel.RELIGIONs.Add(dataEntity);
                dbModel.SaveChanges();
                return dataEntity.RELIGION_ID;
            }

            return -1;
        }

        public void Delete(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.RELIGIONs.Find(id);

            if (entity == null)
            {
                return;
            }

            dbModel.RELIGIONs.Remove(entity);
            dbModel.SaveChanges();
        }

        public void Update(ReligionEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.RELIGIONs.Find(entity.ReligionId);

            if (dataEntity==null)
            {
                return;
            }

            dataEntity.RELIGION_NAME = entity.ReligionName;
            dbModel.SaveChanges();
        }

        public ReligionEntity Get(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.RELIGIONs.Find(id);

            if (entity == null)
            {
                return null;
            }

            return ConvertToEntity(entity);
        }

        public List<ReligionEntity> GetAll()
        {
            DbModel dbModel = new DbModel();
            return dbModel.RELIGIONs.ToList().Select(x => ConvertToEntity(x)).ToList(); ;
        }

        private bool CheckExisting(string name)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.RELIGIONs.Where(e => e.RELIGION_NAME == name).FirstOrDefault();
            return entity != null;
        }

        private RELIGION ConvertToDataEntity(ReligionEntity religionEntity)
        {
            if (religionEntity == null)
            {
                return null;
            }

            return new RELIGION
            {
                RELIGION_NAME = religionEntity.ReligionName
            };
        }

        private ReligionEntity ConvertToEntity(RELIGION religion)
        {
            if (religion == null)
            {
                return null;
            }

            return new ReligionEntity
            {
                ReligionId = religion.RELIGION_ID,
                ReligionName = religion.RELIGION_NAME
            };
        }
    }
}
