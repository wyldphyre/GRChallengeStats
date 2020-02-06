using System;
using System.Collections.Generic;
using System.Text;
using GoodReadsLibrary;
using System.Windows.Input;
using MvvmLibrary;
using System.Collections.ObjectModel;
using System.Linq;

namespace GRChallengeStatsWPF
{
    class ChallengeViewModel : ObservableObject
    {
        private ReadingChallenge selectedReadingChallenge;
        private YearContext yearContext;
        private ObservableCollection<ReadingChallenge> challenges;
        private ChallengeStatistics statistics;

        private int target;
        private int booksCompleted;

        public ChallengeViewModel()
        {
            Challenges = new ObservableCollection<ReadingChallenge>();

            // design time data
            challenges.Add(new ReadingChallenge { Year = 2020, Target = 30, Completed = 7 });
            SelectedReadingChallenge = Challenges.First();
            
            InitialiseCommands();
        }

        public void LoadData()
        {
            LoadDataFromDisk();
        }

        public ReadingChallenge SelectedReadingChallenge
        {
            get => selectedReadingChallenge;
            set => SetProperty(ref selectedReadingChallenge, value, RestoreChallenge);
        }

        public ObservableCollection<ReadingChallenge> Challenges
        {
            get => challenges;
            set => SetProperty(ref challenges, value);
        }

        public int Year => SelectedReadingChallenge?.Year ?? 0;

        public int Target
        {
            get => target;
            set => this.SetProperty(ref target, value, UpdateStatistics);
        }

        public double ChallengeProgress => statistics?.PercentComplete ?? 0;

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

        public double RequiredBookPercentPerDay => statistics?.RequiredBookPercentPerDay ?? 0;

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

            statistics = new ChallengeStatistics(SelectedReadingChallenge, yearContext);
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
            target = SelectedReadingChallenge.Target;
            booksCompleted = SelectedReadingChallenge.Completed;
            RaisePropertyChanged(nameof(Year));
            RaisePropertyChanged(nameof(Target));
            RaisePropertyChanged(nameof(BooksCompleted));

            UpdateStatistics();
        }

        private void SaveChallenge()
        {
            SelectedReadingChallenge.Target = Target;
            SelectedReadingChallenge.Completed = BooksCompleted;
        }

        private void LoadDataFromDisk()
        {
            // TODO: load data from disk into Challenges, and set the current years challenge as the SelectedReadingChallenge
            Challenges.Clear();

            var currentYearsChallenge = Challenges.FirstOrDefault(c => c.Year == DateTime.Now.Year);

            if (currentYearsChallenge == null)
            {
                SelectedReadingChallenge = new ReadingChallenge { Year = DateTime.Now.Year, Target = 0, Completed = 0 };
                challenges.Add(selectedReadingChallenge);
            }
        }
    }
}
