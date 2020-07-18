using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class AddressHandler: IDataAccess<AddressEntity>
    {

        public void Add(AddressEntity entity)
        {
            DbModel dbModel = new DbModel();

            if (entity == null)
            {
                return;
            }

            if (!CheckExisting(entity))
            {
                var dataEntity = ConvertToDataEntity(entity);
                if (dataEntity == null)
                {
                    return;
                }

                dbModel.ADDRESSes.Add(dataEntity);
                dbModel.SaveChanges();
            }

        }

        public void Delete(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.ADDRESSes.Find(id);

            if (entity == null)
            {
                return;
            }

            dbModel.ADDRESSes.Remove(entity);
            dbModel.SaveChanges();
        }

        public AddressEntity Get(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.ADDRESSes.Find(id);

            if (entity == null)
            {
                return null;
            }

            return ConvertToEntity(entity);
        }

        public void Update(AddressEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.ADDRESSes.Find(entity.AddressId);

            if (dataEntity==null)
            {
                return;
            }
         
            dataEntity.ADDRESS_STREET = entity.AddressStreet;
            dataEntity.ADDRESS_STREETNO = entity.AddressStreetNo;
            dataEntity.ADDRESS_CITY = entity.AddressCity;
            dataEntity.ADDRESS_COUNTRY = entity.AddressCountry;
            
            dbModel.SaveChanges();
        }

        private bool CheckExisting(AddressEntity entity)
        {
            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.ADDRESSes.Where(e => e.USER_PROFILE_ID == entity.UserProfileId && e.ADDRESS_STREET==entity.AddressStreet && e.ADDRESS_STREETNO == entity.AddressStreetNo && e.ADDRESS_CITY == entity.AddressCity && e.ADDRESS_COUNTRY == entity.AddressCountry).FirstOrDefault();
            return dataEntity != null;
        }

        private ADDRESS ConvertToDataEntity(AddressEntity addressEntity)
        {
            if (addressEntity == null)
            {
                return null;
            }

            return new ADDRESS
            {
                USER_PROFILE_ID=addressEntity.UserProfileId,
                ADDRESS_STREET = addressEntity.AddressStreet,
                ADDRESS_STREETNO = addressEntity.AddressStreetNo,
                ADDRESS_CITY = addressEntity.AddressCity,
                ADDRESS_COUNTRY = addressEntity.AddressCountry
            };
        }

        private AddressEntity ConvertToEntity (ADDRESS address)
        {
            if (address == null)
            {
                return null;
            }

            return new AddressEntity
            {
                AddressId = address.ADDRESS_ID,
                AddressStreet = address.ADDRESS_STREET,
                AddressStreetNo = address.ADDRESS_STREETNO,
                AddressCity = address.ADDRESS_CITY,
                AddressCountry = address.ADDRESS_COUNTRY,
                UserProfileId = address.USER_PROFILE_ID
            };
        }

    }
}
