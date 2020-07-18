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
    public class OrientationHandler: IDataAccess<OrientationEntity>
    {
        public void Add(OrientationEntity orientationEntity)
        {
            DbModel dbModel = new DbModel();

            if (orientationEntity == null)
            {
                return;
            }

            if (!CheckExisting(orientationEntity.OrientationName))
            {
                var dataEntity = ConvertToDataEntity(orientationEntity);
                if (dataEntity == null)
                {
                    return;
                }

                dbModel.ORIENTATIONs.Add(dataEntity);
                dbModel.SaveChanges();
            }

        }

        public void Delete(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.ORIENTATIONs.Find(id);

            if(entity == null)
            {
                return;
            }

            dbModel.ORIENTATIONs.Remove(entity);
            dbModel.SaveChanges();
        }

        public void Update(OrientationEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.ORIENTATIONs.Find(entity.OrientationId);

            if (dataEntity == null)
            {
                return;
            }

            dataEntity.ORIENT_NAME = entity.OrientationName;
            dbModel.SaveChanges();
        }

        public OrientationEntity Get(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.ORIENTATIONs.Find(id);

            if (entity == null)
            {
                return null;
            }

            return ConvertToEntity(entity);
        }

        private bool CheckExisting(string name)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.ORIENTATIONs.Where(e => e.ORIENT_NAME == name).FirstOrDefault();
            return entity != null;
        }

        private ORIENTATION ConvertToDataEntity(OrientationEntity orientationEntity)
        {
            if (orientationEntity == null)
            {
                return null;
            }

            return new ORIENTATION
            {
                ORIENT_NAME = orientationEntity.OrientationName
            };
        }

        private OrientationEntity ConvertToEntity(ORIENTATION orientation)
        {
            if (orientation == null)
            {
                return null;
            }

            return new OrientationEntity
            {
                OrientationId = orientation.ORIENT_ID,
                OrientationName = orientation.ORIENT_NAME
            };
        }
    }
}
