using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class GenderHandler: IDataAccess<GenderEntity>
    {

        public void Add(GenderEntity entity)
        {
            DbModel dbModel = new DbModel();

            if (entity == null)
            {
                return;
            }
            
            if (!CheckExisting(entity.GenderName))
            {
                var dataEntity = ConvertToDataEntity(entity);
                if (dataEntity == null)
                {
                    return;
                }

                dbModel.GENDERs.Add(dataEntity);
                dbModel.SaveChanges();
            }
            
        }

        public void Delete(int id)
        {
            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.GENDERs.Find(id);

            if (dataEntity == null)
            {
                return;
            }

            dbModel.GENDERs.Remove(dataEntity);
            dbModel.SaveChanges();
        }

        public void Update(GenderEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.GENDERs.Find(entity.GenderId);

            if (dataEntity==null)
            {
                return;
            }

            dataEntity.GENDER_NAME = entity.GenderName;
            dbModel.SaveChanges();
        }

        public GenderEntity Get(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.GENDERs.Find(id);

            if (entity == null)
            {
                return null;
            }

            return ConvertToEntity(entity);
        }

        private bool CheckExisting(string name)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.GENDERs.Where(e => e.GENDER_NAME == name).FirstOrDefault();
            return entity != null;
        }

        private GENDER ConvertToDataEntity(GenderEntity genderEntity)
        {
            if (genderEntity == null)
            {
                return null;
            }

            return new GENDER
            {
                GENDER_NAME = genderEntity.GenderName
            };
        }

        private GenderEntity ConvertToEntity(GENDER gender)
        {
            if (gender == null)
            {
                return null;
            }

            return new GenderEntity
            {
                GenderId = gender.GENDER_ID,
                GenderName = gender.GENDER_NAME
            };
        }

    }
}
