using GoodReadsChallenge.Models;

namespace GoodReadsLibrary.Services
{
    public class ReadingChallengeService
    {
        public ChallengeStatistics CalculateStatistics(ReadingChallenge challenge, YearContext yearContext)
        {
            double averageDaysInAMonth = yearContext.DaysInYear / 12;
            var monthsElapsed = Math.Round(yearContext.DaysElapsed / averageDaysInAMonth, 1);

            var booksRemaining = challenge.Target - challenge.Completed;
            var monthsRemaining = Math.Round(yearContext.DaysRemaining / averageDaysInAMonth, 1);
            var averageDaysPerBook = challenge.Completed > 0 ? Math.Round((double)yearContext.DaysElapsed / challenge.Completed, 2) : 0;
            var currentBooksPerMonth = challenge.Completed / monthsElapsed;
            var requiredBooksPerMonth = Math.Round(booksRemaining / monthsRemaining, 2);

            return new ChallengeStatistics(challenge, yearContext)
            {
                BooksRemaining = booksRemaining,
                MonthsRemaining = monthsRemaining,
                RequiredBooksPerMonth = requiredBooksPerMonth,
                RequiredBookPercentPerDay = (double)booksRemaining / yearContext.DaysRemaining,
                PercentComplete = challenge.Target > 0 ? Math.Round((double)challenge.Completed / challenge.Target, 3) : 0,
                CurrentBooksPerMonth = currentBooksPerMonth,
                AverageDaysPerBook = averageDaysPerBook,
                ForecastBookTotal = Math.Round(challenge.Completed + yearContext.DaysRemaining / averageDaysPerBook, 2),
                RemaningAverageDaysPerBook = challenge.Target - challenge.Completed <= 0 ? 0 : Math.Round((double)yearContext.DaysRemaining / booksRemaining, 2),
                BooksPerMonthDifference = currentBooksPerMonth - requiredBooksPerMonth
            };
        }

        public YearContext GetYearContext(int year)
        {
            var Now = DateTime.Now;
            var daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
            int daysElapsed;
            int daysRemaining;

            if (year == Now.Year)
            {
                // Calculate values for current year
                daysElapsed = Now.DayOfYear - 1;
                daysRemaining = daysInYear - daysElapsed;
            }
            else
            {
                // Calculate values for past years
                daysElapsed = daysInYear;
                daysRemaining = 0;
            }

            return new YearContext
            {
                Year = year,
                DaysInYear = daysInYear,
                DaysElapsed = daysElapsed,
                DaysRemaining = daysRemaining
            };
        }
    }
}
