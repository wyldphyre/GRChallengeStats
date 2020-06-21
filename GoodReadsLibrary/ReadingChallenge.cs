using System;

namespace GoodReadsLibrary
{
    public class ReadingChallenge
    {
        public int Year { get; set; }
        public int Target { get; set; }
        public int Completed { get; set; }
    }

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

    public class ChallengeStatistics
    {
        public ChallengeStatistics(ReadingChallenge challenge, YearContext yearContext)
        {
            Challenge = challenge;
            YearContext = yearContext;

            double averageDaysInAMonth = yearContext.DaysInYear / 12;
            var monthsElapsed = Math.Round((yearContext.DaysElapsed / averageDaysInAMonth), 1);

            BooksRemaining = challenge.Target - challenge.Completed;
            MonthsRemaining = Math.Round(yearContext.DaysRemaining / averageDaysInAMonth, 1);
            RequiredBooksPerMonth = Math.Round(BooksRemaining / MonthsRemaining, 2);
            RequiredBookPercentPerDay = (double)BooksRemaining / yearContext.DaysRemaining;
            PercentComplete = challenge.Target > 0 ? Math.Round((double)challenge.Completed / challenge.Target, 3) : 0;
            CurrentBooksPerMonth = challenge.Completed / monthsElapsed;
            AverageDaysPerBook = challenge.Completed > 0 ? Math.Round((double)yearContext.DaysElapsed / challenge.Completed, 2) : 0;
            ForecastBookTotal = Math.Round(challenge.Completed + (yearContext.DaysRemaining / AverageDaysPerBook), 2);
            RemaningAverageDaysPerBook = challenge.Target - challenge.Completed <= 0 ? 0 : Math.Round((double)yearContext.DaysRemaining / BooksRemaining, 2);
            BooksPerMonthDifference = CurrentBooksPerMonth - RequiredBooksPerMonth;
        }

        public readonly ReadingChallenge Challenge;
        public readonly YearContext YearContext;
        public readonly int BooksRemaining;
        public readonly double MonthsRemaining;
        public readonly double RequiredBooksPerMonth;
        public readonly double RequiredBookPercentPerDay;
        public readonly double PercentComplete;
        public readonly double CurrentBooksPerMonth;
        public readonly double ForecastBookTotal;
        public readonly double AverageDaysPerBook;
        public readonly double RemaningAverageDaysPerBook;

        /// <summary>
        /// The difference between the required books per month rate and the current books per month rate.
        /// </summary>
        public readonly double BooksPerMonthDifference;
    }
}
