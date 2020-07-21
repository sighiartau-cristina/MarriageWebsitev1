using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using BusinessModel.Contracts;
using BusinessModel.Entities;

namespace BusinessModel.Handlers
{
    class MatchHandler: IDataAccess<MatchEntity>
    {
        public int Add(MatchEntity entity)
        {
            DbModel dbModel = new DbModel();

            if (entity == null)
            {
                return -1;
            }

            if (!CheckExisting(entity))
            {
                var dataEntity = ConvertToDataEntity(entity);
                if (dataEntity == null)
                {
                    return -1;
                }

                dbModel.MATCHes.Add(dataEntity);
                dbModel.SaveChanges();
                return dataEntity.MATCH_ID;
            }

            return -1;
        }

        public void Delete(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.MATCHes.Find(id);

            if (entity == null)
            {
                return;
            }

            dbModel.MATCHes.Remove(entity);
            dbModel.SaveChanges();
        }

        public MatchEntity Get(int id)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.MATCHes.Find(id);

            if (entity == null)
            {
                return null;
            }

            return ConvertToEntity(entity);
        }

        public void Update(MatchEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.MATCHes.Find(entity.MatchId);

            if (dataEntity == null)
            {
                return;
            }

            dataEntity.MATCH_USER_PROFILE_ID = entity.MatchUserProfileId;
            dataEntity.USER_PROFILE_ID = entity.UserProfileId;
            dbModel.SaveChanges();
        }

        private bool CheckExisting(MatchEntity entity)
        {
            DbModel dbModel = new DbModel();
            var dataEntity = dbModel.MATCHes.Where(e => e.MATCH_ID == entity.MatchId && e.USER_PROFILE_ID == entity.UserProfileId && e.MATCH_USER_PROFILE_ID == entity.MatchUserProfileId).FirstOrDefault();
            return dataEntity != null;
        }

        private MATCH ConvertToDataEntity(MatchEntity matchEntity)
        {
            if (matchEntity == null)
            {
                return null;
            }

            return new MATCH
            {
                USER_PROFILE_ID = matchEntity.UserProfileId,
                MATCH_USER_PROFILE_ID = matchEntity.MatchUserProfileId,
                MATCH_DATE = matchEntity.MatchDate
            };
        }

        private MatchEntity ConvertToEntity(MATCH match)
        {
            if (match == null)
            {
                return null;
            }

            return new MatchEntity
            {
                MatchDate = match.MATCH_DATE,
                MatchId = match.MATCH_ID,
                MatchUserProfileId = match.MATCH_USER_PROFILE_ID,
                UserProfileId = match.USER_PROFILE_ID
            };
        }

    }
}
