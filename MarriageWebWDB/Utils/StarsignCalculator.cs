using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarriageWebWDB.Utils
{
    public static class StarsignCalculator
    {
        public static string GetStarsignName(DateTime birthday)
        {
           if(birthday.Month == 1)
            {
                if (birthday.Day < 21)
                {
                    return "Capricorn";
                }
                else
                {
                    return "Aquarius";
                }
            }
            else if(birthday.Month == 2)
            {
                if (birthday.Day < 20)
                {
                    return "Aquarius";
                }
                else
                {
                    return "Pisces";
                }
            }
            else if (birthday.Month == 3)
            {
                if (birthday.Day < 21)
                {
                    return "Pisces";
                }
                else
                {
                    return "Aries";
                }
            }

            else if (birthday.Month == 4)
            {
                if (birthday.Day < 21)
                {
                    return "Aries";
                }
                else
                {
                    return "Taurus";
                }
            }

            else if (birthday.Month == 5)
            {
                if (birthday.Day < 22)
                {
                    return "Taurus";
                }
                else
                {
                    return "Gemini";
                }
            }
            else if (birthday.Month == 6)
            {
                if (birthday.Day < 22)
                {
                    return "Gemini";
                }
                else
                {
                    return "Cancer";
                }
            }
            else if (birthday.Month == 7)
            {
                if (birthday.Day < 23)
                {
                    return "Cancer";
                }
                else
                {
                    return "Leo";
                }
            }
            else if (birthday.Month == 8)
            {
                if (birthday.Day < 23)
                {
                    return "Leo";
                }
                else
                {
                    return "Virgo";
                }
            }
            else if (birthday.Month == 9)
            {
                if (birthday.Day < 24)
                {
                    return "Virgo";
                }
                else
                {
                    return "Libra";
                }
            }
            else if (birthday.Month == 10)
            {
                if (birthday.Day < 24)
                {
                    return "Libra";
                }
                else
                {
                    return "Scorpio";
                }
            }
            else if (birthday.Month == 11)
            {
                if (birthday.Day < 23)
                {
                    return "Scorpio";
                }
                else
                {
                    return "Sagittarius";
                }
            }
            else if (birthday.Month == 12)
            {
                if (birthday.Day < 22)
                {
                    return "Sagittarius";
                }
                else
                {
                    return "Capricorn";
                }
            }

            return "";
        }
    }
}