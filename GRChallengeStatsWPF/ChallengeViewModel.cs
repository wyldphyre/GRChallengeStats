using System;
using System.Collections.Generic;
using System.Text;
using GoodReadsLibrary;
using MvvmHelpers;

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
        }

        public ReadingChallenge Challenge
        {
            get => challenge;
            set => SetProperty(ref challenge, value, nameof(Challenge), LoadChallenge);
        }

        public int Year
        {
            get => challenge?.Year ?? 0;
            //set => this.rais SetProperty(ref year, value);
        }

        public int Target
        {
            get => target;
            set => this.SetProperty(ref target, value, nameof(Target), UpdateStatistics);
        }

        public string ChallengeProgress
        {
            get => $"{statistics?.PercentComplete:P}";
        }

        public int BooksCompleted
        {
            get => booksCompleted;
            set => this.SetProperty(ref booksCompleted, value, nameof(BooksCompleted), UpdateStatistics);
        }

        public double CurrentBooksPerMonth => statistics?.CurrentBooksPerMonth ?? 0;

        public double RequiredBooksPerMonth => statistics?.RequiredBooksPerMonth ?? 0;

        public string RequiredBookPercentPerDay => $"{statistics?.RequiredBookPercentPerDay:P}";

        public int BooksRemaining => statistics?.BooksRemaining ?? 0;

        public double MonthsRemaining => statistics?.MonthsRemaining ?? 0;

        public double DaysPerBook => statistics?.AverageDaysPerBook ?? 0;

        public double RemainingDaysPerbook => statistics?.RemaningAverageDaysPerBook ?? 0;

        public double ForecastTotal => statistics?.ForecastBookTotal ?? 0;

        private void LoadChallenge()
        {
            target = challenge.Target;
            booksCompleted = challenge.Completed;
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            yearContext = new YearContext(Year);

            SaveChallenge();

            statistics = new ChallengeStatistics(challenge, yearContext);
            OnPropertyChanged(nameof(ChallengeProgress));
            OnPropertyChanged(nameof(CurrentBooksPerMonth));
            OnPropertyChanged(nameof(RequiredBooksPerMonth));
            OnPropertyChanged(nameof(RequiredBookPercentPerDay));
            OnPropertyChanged(nameof(BooksRemaining));
            OnPropertyChanged(nameof(MonthsRemaining));
            OnPropertyChanged(nameof(DaysPerBook));
            OnPropertyChanged(nameof(RemainingDaysPerbook));
            OnPropertyChanged(nameof(ForecastTotal));
        }

        private void SaveChallenge()
        {
            challenge.Target = Target;
            challenge.Completed = BooksCompleted;
        }
    }
}
