using System;
using System.Collections.Generic;
using System.Text;
using GoodReadsLibrary;
using System.Windows.Input;
using MvvmLibrary;

namespace GRChallengeStatsWPF
{
    class ChallengeViewModel : ObservableObject
    {
        private YearContext yearContext;
        private ReadingChallenge challenge;
        private ChallengeStatistics statistics;

        private int target;
        private int booksCompleted;

        public ChallengeViewModel()
        {
            // design time data
            Challenge = new ReadingChallenge { Year = 2020, Target = 30, Completed = 6 };

            InitialiseCommands();
        }

        public ReadingChallenge Challenge
        {
            get => challenge;
            set => SetProperty(ref challenge, value, RestoreChallenge);
        }

        public int Year
        {
            get => challenge?.Year ?? 0;
            //set => this.rais SetProperty(ref year, value);
        }

        public int Target
        {
            get => target;
            set => this.SetProperty(ref target, value, UpdateStatistics);
        }

        public string ChallengeProgress
        {
            get => $"{statistics?.PercentComplete:P}";
        }

        public int BooksCompleted
        {
            get => booksCompleted;
            set
            {
                if (this.SetProperty(ref booksCompleted, value, UpdateStatistics))
                {
                    RaisePropertyChanged(nameof(HasBooksCompleted));
                }
            }
        }

        public bool HasBooksCompleted => BooksCompleted > 0;

        public double CurrentBooksPerMonth => statistics?.CurrentBooksPerMonth ?? 0;

        public double RequiredBooksPerMonth => statistics?.RequiredBooksPerMonth ?? 0;

        public string RequiredBookPercentPerDay => $"{statistics?.RequiredBookPercentPerDay:P}";

        public int BooksRemaining => statistics?.BooksRemaining ?? 0;

        public double MonthsRemaining => statistics?.MonthsRemaining ?? 0;

        public double DaysPerBook => statistics?.AverageDaysPerBook ?? 0;

        public double RemainingDaysPerbook => statistics?.RemaningAverageDaysPerBook ?? 0;

        public double ForecastTotal => statistics?.ForecastBookTotal ?? 0;

        public ICommand IncrementBooksCompletedCommand { get; set; } 

        public ICommand DecrementBooksCompletedCommand { get; set; }

        private void InitialiseCommands()
        {
            IncrementBooksCompletedCommand = new RelayCommand(() => BooksCompleted++);
            DecrementBooksCompletedCommand = new RelayCommand(() => BooksCompleted--, () => BooksCompleted > 0);
        }

        private void UpdateStatistics()
        {
            yearContext = new YearContext(Year);

            SaveChallenge();

            statistics = new ChallengeStatistics(challenge, yearContext);
            RaisePropertyChanged(nameof(ChallengeProgress));
            RaisePropertyChanged(nameof(CurrentBooksPerMonth));
            RaisePropertyChanged(nameof(RequiredBooksPerMonth));
            RaisePropertyChanged(nameof(RequiredBookPercentPerDay));
            RaisePropertyChanged(nameof(BooksRemaining));
            RaisePropertyChanged(nameof(MonthsRemaining));
            RaisePropertyChanged(nameof(DaysPerBook));
            RaisePropertyChanged(nameof(RemainingDaysPerbook));
            RaisePropertyChanged(nameof(ForecastTotal));
        }

        private void RestoreChallenge()
        {
            target = challenge.Target;
            booksCompleted = challenge.Completed;
            UpdateStatistics();
        }

        private void SaveChallenge()
        {
            challenge.Target = Target;
            challenge.Completed = BooksCompleted;
        }
    }
}
