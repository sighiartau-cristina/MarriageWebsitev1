using System;

namespace MarriageWebWDB.Utils
{
    public static class DateFormatter
    {
        public static string GetDate(DateTime date)
        {
            string result = date.Year + "-";
            result += date.Month < 10 ? "0"+date.Month.ToString() : date.Month.ToString();
            result += "-";
            result += date.Day < 10 ? "0"+date.Day.ToString() : date.Day.ToString();

            return result;
        }
    }
}