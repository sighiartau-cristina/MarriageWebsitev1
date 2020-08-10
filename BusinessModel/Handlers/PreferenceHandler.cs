using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class PreferenceHandler : IBusinessAccess<PreferenceEntity>
    {
        public ResponseEntity<PreferenceEntity> Add(PreferenceEntity entity)
        {
            DbModel dbModel = new DbModel();
            Preference dataEntity;

            if (entity == null)
            {
                return new ResponseEntity<PreferenceEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            try
            {
                var existing = dbModel.Preferences.Where(x => x.Name == entity.Name).FirstOrDefault();

                //new preference, save it
                if (existing == null)
                {
                    dataEntity = ConvertToDataEntity(entity);

                    if (dataEntity == null)
                    {
                        return new ResponseEntity<PreferenceEntity>
                        {
                            CompletedRequest = false,
                            ErrorMessage = ErrorConstants.NullConvertedEntityError
                        };
                    }
                }
                //already existing preference, retrieve it
                else
                {
                    return new ResponseEntity<PreferenceEntity>
                    {
                        CompletedRequest = true,
                        Entity = ConvertToEntity(existing)
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<PreferenceEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.PreferenceGetError
                };
            }

            try
            {
                dbModel.Preferences.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<PreferenceEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.PreferenceInsertError
                };
            }

            return new ResponseEntity<PreferenceEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(dataEntity)
            };
        }

        public ResponseEntity<PreferenceEntity> AddForUser(int prefId, int userProfileId, bool like)
        {
            DbModel dbModel = new DbModel();

            var existing = dbModel.UserProfile_Preference.Where(u => u.PrefId == prefId && u.UserProfileId == userProfileId && u.Likes==like).FirstOrDefault();

            //do nothing if preference already exists
            if(existing != null)
            {
                return new ResponseEntity<PreferenceEntity>
                {
                    CompletedRequest = true
                };
            }

            var dataEntity = new UserProfile_Preference
            {
                PrefId = prefId,
                UserProfileId = userProfileId,
                Likes = like
            };

            if (dataEntity == null)
            {
                return new ResponseEntity<PreferenceEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            try
            {
                dbModel.UserProfile_Preference.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<PreferenceEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.PreferenceInsertError
                };
            }

            return new ResponseEntity<PreferenceEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<PreferenceEntity> DeleteForUser(int prefId, int userProfileId, bool likes)
        {
            DbModel dbModel = new DbModel();

            var existing = dbModel.UserProfile_Preference.Where(u => u.PrefId == prefId && u.UserProfileId == userProfileId && u.Likes==likes).FirstOrDefault();

            if (existing == null)
            {
                return new ResponseEntity<PreferenceEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.PreferenceNotFound
                };
            }

            try
            {
                dbModel.UserProfile_Preference.Remove(existing);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<PreferenceEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.PreferenceDeleteError
                };
            }

            return new ResponseEntity<PreferenceEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<PreferenceEntity> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResponseEntity<PreferenceEntity> Get(int id)
        {
            throw new NotImplementedException();
        }

        public ResponseEntity<ICollection<PreferenceEntity>> GetAll()
        {
            DbModel dbModel = new DbModel();
            ICollection<PreferenceEntity> list;

            try
            {
                list = dbModel.Preferences.ToList().Select(x => ConvertToEntity(x)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<PreferenceEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.PreferenceGetError
                };
            }

            return new ResponseEntity<ICollection<PreferenceEntity>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        public ResponseEntity<ICollection<PreferenceEntity>> GetAllForTerm(string term)
        {
            DbModel dbModel = new DbModel();
            ICollection<PreferenceEntity> list;

            try
            {
                list = dbModel.Preferences.ToList().FindAll(x => x.Name.ToLower().Contains(term.ToLower())).Select(x=> ConvertToEntity(x)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<PreferenceEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.PreferenceGetError
                };
            }

            return new ResponseEntity<ICollection<PreferenceEntity>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        public ResponseEntity<ICollection<PreferenceEntity>> GetAllForUserProfile(int userProfileId, bool likes)
        {
            DbModel dbModel = new DbModel();
            List<Preference> list;

            try
            {
                list = dbModel.Database.SqlQuery<Preference>("select p.* from Preference p join UserProfile_Preference u on p.Id=u.PrefId where u.UserProfileId=@user_profile and u.Likes=@likes;", new SqlParameter("user_profile", userProfileId), new SqlParameter("likes", likes)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<PreferenceEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.UserProfileGetError
                };
            }

            return new ResponseEntity<ICollection<PreferenceEntity>>
            {
                CompletedRequest = true,
                Entity = list.Select(x => ConvertToEntity(x)).ToList()
            };
        }
   
        public ResponseEntity<PreferenceEntity> Update(PreferenceEntity entity)
        {
            throw new NotImplementedException();
        }

        private PreferenceEntity ConvertToEntity(Preference preference)
        {
            if (preference == null)
            {
                return null;
            }

            return new PreferenceEntity
            {
                Id = preference.Id,
                Name = preference.Name
            };
        }

        private Preference ConvertToDataEntity(PreferenceEntity preference)
        {
            if(preference == null)
            {
                return null;
            }

            return new Preference
            {
                Name = preference.Name
            };
        }
    }
}
