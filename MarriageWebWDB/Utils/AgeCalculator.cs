using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarriageWebWDB.Utils
{
    public static class AgeCalculator
    {
        public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {

            int years = endDate.Year - startDate.Year;

            if (startDate.Month == endDate.Month && endDate.Day < startDate.Day || endDate.Month < startDate.Month)
            {
                years--;
            }

            return years;
        }
    }
}