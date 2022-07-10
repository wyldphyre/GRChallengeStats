namespace GoodReadsChallenge.Models
{
    public class ChallengeStatistics
    {
        public ChallengeStatistics(ReadingChallenge challenge, YearContext yearContext)
        {
            Challenge = challenge;
            YearContext = yearContext;
        }

        public ReadingChallenge Challenge { get; init; }
        public YearContext YearContext { get; init; }
        public int BooksRemaining { get; init; }
        public double MonthsRemaining { get; init; }
        public double RequiredBooksPerMonth { get; init; }
        public double RequiredBookPercentPerDay { get; init; }
        public double PercentComplete { get; init; }
        public double CurrentBooksPerMonth { get; init; }
        public double ForecastBookTotal { get; init; }
        public double AverageDaysPerBook { get; init; }
        public double RemaningAverageDaysPerBook { get; init; }

        /// <summary>
        /// The difference between the required books per month rate and the current books per month rate.
        /// </summary>
        public double BooksPerMonthDifference { get; init; }
    }
}