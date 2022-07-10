using System;

namespace GoodReadsLibrary.Models
{
    public class YearContext
    {
        public YearContext(int year)
        {
            Year = year;

            var Now = DateTime.Now;

            DaysInYear = DateTime.IsLeapYear(year) ? 366 : 365;

            if (year == Now.Year)
            {
                // Calculate values for current year
                DaysElapsed = Now.DayOfYear - 1;
                DaysRemaining = DaysInYear - DaysElapsed;
            }
            else
            {
                // Calculate values for past years
                DaysElapsed = DaysInYear;
                DaysRemaining = 0;
            }
        }

        public readonly int Year;
        public readonly int DaysElapsed;
        public readonly int DaysRemaining;
        public readonly int DaysInYear;
    }


}
