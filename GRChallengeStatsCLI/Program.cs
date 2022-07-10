using GoodReadsChallenge.Models;
using GoodReadsLibrary.Services;
using System;
using System.Linq;

namespace GRChallengeStats
{
    class Program
    {
        static void Main(string[] args)
        {
            var readingChallengeService = new ReadingChallengeService();

            var year = DateTime.Now.Year;
            int target = 0;
            int completed = 0;

            if (args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine("Invalid arguments!");
                Console.WriteLine("  Must include at least the 'target' and 'completed' arguments, and optionally the 'year' argument");
                Console.WriteLine("  example: grchallengestats [year:2020] target:30 completed:5");
                return;
            }

            foreach (var arg in args)
            {
                if (!arg.Contains(":"))
                {
                    Console.WriteLine($"Argument '{arg}' is invalid. Arguments must start with a '-' and contain a ':' followed by a value");
                    return;
                }

                var components = arg.Split(':', StringSplitOptions.RemoveEmptyEntries);

                if (components.Length < 2)
                {
                    Console.WriteLine($"Argument '{arg}' does not contains a value after the ':'!");
                    return;
                }

                var argument = components.First();
                var value = components.Last();

                if (argument.Equals("year", StringComparison.OrdinalIgnoreCase))
                {
                    if (!int.TryParse(value, out year))
                    {
                        Console.WriteLine($"Value '{value}' is not a valid number");
                        return;
                    }

                    if (year > DateTime.Now.Year)
                    {
                        Console.WriteLine("Cannot specify a year in the future");
                        return;
                    }
                }
                else if (argument.Equals("target", StringComparison.OrdinalIgnoreCase))
                {
                    if (!int.TryParse(value, out target))
                    {
                        Console.WriteLine($"Value '{value}' is not a valid number");
                        return;
                    }
                }
                else if (argument.Equals("completed", StringComparison.OrdinalIgnoreCase))
                {
                    if (!int.TryParse(value, out completed))
                    {
                        Console.WriteLine($"Value '{value}' is not a valid number");
                        return;
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Year: {year}");
            Console.WriteLine($"Target: {target} {(target == 1 ? "book" : "books")}");
            Console.WriteLine($"Completed: {completed} {(target == 1 ? "book" : "books")}");
            Console.WriteLine();

            var challenge = new ReadingChallenge
            {
                Year = year,
                Target = target,
                Completed = completed
            };

            var yearContext = readingChallengeService.GetYearContext(year);

            Console.WriteLine($"Days in Year: {yearContext.DaysInYear}");
            Console.WriteLine($"Days Elapsed: {yearContext.DaysElapsed}");
            Console.WriteLine($"Days Remaining: {yearContext.DaysRemaining}");
            Console.WriteLine();

            var statistics = readingChallengeService.CalculateStatistics(challenge, yearContext);

            Console.WriteLine($"Progress: {statistics.PercentComplete:P}");

            var normalForegroundColour = Console.ForegroundColor;
            Console.ForegroundColor = statistics.CurrentBooksPerMonth >= statistics.RequiredBooksPerMonth ? ConsoleColor.Green : ConsoleColor.Red;

            Console.WriteLine($"Current Rate(bpm): {statistics.CurrentBooksPerMonth}");

            Console.ForegroundColor = normalForegroundColour;

            Console.WriteLine($"Required Rate(bpm): {statistics.RequiredBooksPerMonth}");
            Console.WriteLine($"Required Rate(Book % per day): {statistics.RequiredBookPercentPerDay:P}");
            Console.WriteLine();
            Console.WriteLine($"Books Remaining: {statistics.BooksRemaining}");
            Console.WriteLine($"Months Remaining: {statistics.MonthsRemaining}");

            Console.ForegroundColor = statistics.ForecastBookTotal >= challenge.Target ? ConsoleColor.Green : ConsoleColor.Red;

            Console.WriteLine($"Forecast Book Total: {statistics.ForecastBookTotal}");

            Console.ForegroundColor = normalForegroundColour;

            Console.WriteLine($"Average Days per Book: {statistics.AverageDaysPerBook}");
            Console.WriteLine($"Remaning Avg Days per Book: {statistics.RemaningAverageDaysPerBook}");
        }
    }
}
