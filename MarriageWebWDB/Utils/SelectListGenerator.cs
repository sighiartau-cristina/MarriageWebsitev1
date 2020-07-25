using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessModel.Entities;
using BusinessModel.Handlers;

namespace MarriageWebWDB.Utils
{
    public static class SelectListGenerator
    {
        //TODO check for nulls in controller for each
        public static IEnumerable<SelectListItem> GetSelectedReligions(UserProfileEntity profile)
        {
            ReligionHandler religionHandler = new ReligionHandler();
            var religions = religionHandler.GetAll();

            if (religions.CompletedRequest)
            {
                var list = religions.Entity.Select(x =>
                                new SelectListItem
                                {
                                    Selected = (x.ReligionId == profile.ReligionId),
                                    Value = x.ReligionId.ToString(),
                                    Text = x.ReligionName
                                });
                return new SelectList(list, "Value", "Text");
            }
            else
            {
                return null;
            }

        }

        public static IEnumerable<SelectListItem> GetReligions()
        {
            ReligionHandler religionHandler = new ReligionHandler();
            var religions = religionHandler.GetAll();

            if (religions.CompletedRequest)
            {
                var list = religionHandler.GetAll().Entity
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.ReligionId.ToString(),
                                    Text = x.ReligionName
                                });

                return new SelectList(list, "Value", "Text");
            }
            else
            {
                return null;
            }

        }

        public static IEnumerable<SelectListItem> GetSelectedGenders(UserProfileEntity profile)
        {
            GenderHandler genderHandler = new GenderHandler();
            var genders = genderHandler.GetAll();

            if (genders.CompletedRequest)
            {
                var list = genders.Entity.Select(x =>
                               new SelectListItem
                               {
                                   Selected = (x.GenderId == profile.GenderId),
                                   Value = x.GenderId.ToString(),
                                   Text = x.GenderName
                               });

                return new SelectList(list, "Value", "Text");
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<SelectListItem> GetGenders()
        {
            GenderHandler genderHandler = new GenderHandler();
            var genders = genderHandler.GetAll();

            if (genders.CompletedRequest)
            {
                var list = genders.Entity.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.GenderId.ToString(),
                                    Text = x.GenderName
                                });

                return new SelectList(list, "Value", "Text");
            }
            else
            {
                return null;
            }

        }

        public static IEnumerable<SelectListItem> GetSelectedOrientations(UserProfileEntity profile)
        {
            OrientationHandler orientationHandler = new OrientationHandler();
            var orientations = orientationHandler.GetAll();

            if (orientations.CompletedRequest)
            {
                var list = orientations.Entity.Select(x =>
                                new SelectListItem
                                {
                                    Selected = (x.OrientationId == profile.OrientationId),
                                    Value = x.OrientationId.ToString(),
                                    Text = x.OrientationName
                                });

                return new SelectList(list, "Value", "Text");
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<SelectListItem> GetOrientations()
        {
            OrientationHandler orientationHandler = new OrientationHandler();
            var orientations = orientationHandler.GetAll();

            if (orientations.CompletedRequest)
            {
                var list = orientations.Entity.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.OrientationId.ToString(),
                                    Text = x.OrientationName
                                });

                return new SelectList(list, "Value", "Text");
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<SelectListItem> GetSelectedStatuses(UserProfileEntity profile)
        {
            MaritalStatusHandler statusHandler = new MaritalStatusHandler();
            var statuses = statusHandler.GetAll();

            if (statuses.CompletedRequest)
            {
                var list = statuses.Entity.Select(x =>
                                new SelectListItem
                                {
                                    Selected = (x.MaritalStatusId == profile.StatusId),
                                    Value = x.MaritalStatusId.ToString(),
                                    Text = x.MaritalStatusName
                                });

                return new SelectList(list, "Value", "Text");
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<SelectListItem> GetStatuses()
        {
            MaritalStatusHandler statusHandler = new MaritalStatusHandler();
            var statuses = statusHandler.GetAll();

            if (statuses.CompletedRequest)
            {
                var list = statuses.Entity.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.MaritalStatusId.ToString(),
                                    Text = x.MaritalStatusName
                                });

                return new SelectList(list, "Value", "Text");
            }
            else
            {
                return null;
            }
        }

    }
}