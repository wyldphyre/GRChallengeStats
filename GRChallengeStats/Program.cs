using System;
using System.Linq;

namespace GRChallengeStats
{
  class Program
  {
    static void Main(string[] args)
    {
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

      var challenge = new Challenge
      {
        Year = year,
        Target = target,
        Completed = completed
      };

      var yearContext = new YearContext(year);

      Console.WriteLine($"Days in Year: {yearContext.DaysInYear}");
      Console.WriteLine($"Days Elapsed: {yearContext.DaysElapsed}");
      Console.WriteLine($"Days Remaining: {yearContext.DaysRemaining}");
      Console.WriteLine();

      var statistics = new ChallengeStatistics(challenge, yearContext);

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

  public class Challenge
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
        DaysElapsed = Now.DayOfYear -1;
        DaysRemaining = DaysInYear - DaysElapsed;
      }
      else
      {
        // Calculate values for past years
        DaysElapsed = DaysInYear;
        DaysRemaining = 0;
      }
    }

    public int Year { get; private set; }
    public int DaysElapsed { get; private set; }
    public int DaysRemaining { get; private set; }
    public int DaysInYear { get; private set; }
  }

  public class ChallengeStatistics
  {
    public ChallengeStatistics(Challenge challenge, YearContext yearContext)
    {
      Challenge = challenge;
      YearContext = yearContext;

      double averageDaysInAMonth = yearContext.DaysInYear / 12;
      var monthsElapsed = Math.Round((yearContext.DaysElapsed / averageDaysInAMonth), 1);

      BooksRemaining = challenge.Target - challenge.Completed;
      MonthsRemaining = Math.Round(yearContext.DaysRemaining / averageDaysInAMonth, 1);
      RequiredBooksPerMonth = Math.Round(BooksRemaining / MonthsRemaining, 2);
      RequiredBookPercentPerDay = Math.Round((double)BooksRemaining / yearContext.DaysRemaining, 2);
      PercentComplete = Math.Round((double)challenge.Completed / challenge.Target, 3);
      CurrentBooksPerMonth = challenge.Completed / monthsElapsed;
      AverageDaysPerBook = Math.Round((double)yearContext.DaysElapsed / challenge.Completed, 2);
      ForecastBookTotal = Math.Round(challenge.Completed + (yearContext.DaysRemaining / AverageDaysPerBook), 2);
      RemaningAverageDaysPerBook = challenge.Target - challenge.Completed <= 0 ? 0 : Math.Round((double)yearContext.DaysRemaining / BooksRemaining, 2);
  }

    public readonly Challenge Challenge;
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
  }
}
