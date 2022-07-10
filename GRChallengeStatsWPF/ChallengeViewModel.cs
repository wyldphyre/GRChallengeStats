using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MvvmLibrary;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using GoodReadsLibrary.Models;

namespace GRChallengeStatsWPF
{
    public class ChallengeViewModel : ObservableObject
    {
        private ReadingChallenge selectedReadingChallenge;
        private YearContext yearContext;
        private readonly ObservableCollection<ReadingChallenge> challenges;
        private ChallengeStatistics statistics;

        private int target;
        private int booksCompleted;

        private const string DataFilename = "data.json";

        public ChallengeViewModel()
        {
            challenges = new ObservableCollection<ReadingChallenge>();

            InitialiseCommands();
        }

        public void LoadData()
        {
            LoadDataFromDisk();
        }

        public bool SaveData()
        {
            try
            {
                SaveDataToDisk();

                return true;
            }
            catch
            {
                // TODO: pass information about the failure back
                return false; 
            }
        }

        public ReadingChallenge SelectedReadingChallenge
        {
            get => selectedReadingChallenge;
            set => SetProperty(ref selectedReadingChallenge, value, RestoreChallenge);
        }

        public ObservableCollection<ReadingChallenge> Challenges
        {
            get => challenges;
            //set => SetProperty(ref challenges, value);
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
                this.SetProperty(ref booksCompleted, value, UpdateStatistics);
                RaisePropertyChanged(nameof(HasBooksCompleted));
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

        public double BooksPerMonthDifference => statistics?.BooksPerMonthDifference ?? 0;

        public bool HasPositiveBooksPerMonthDifference => BooksPerMonthDifference > 0;

        public ICommand IncrementBooksCompletedCommand { get; set; }

        public ICommand DecrementBooksCompletedCommand { get; set; }

        private void InitialiseCommands()
        {
            IncrementBooksCompletedCommand = new RelayCommand(() => BooksCompleted++);
            DecrementBooksCompletedCommand = new RelayCommand(() => BooksCompleted--, () => HasBooksCompleted);
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
            RaisePropertyChanged(nameof(BooksPerMonthDifference));
            RaisePropertyChanged(nameof(HasPositiveBooksPerMonthDifference));
        }

        private void RestoreChallenge()
        {
            // not using the public properties here to avoid unwanted updating behaviour
            target = SelectedReadingChallenge.Target;
            RaisePropertyChanged(nameof(Target));

            booksCompleted = SelectedReadingChallenge.Completed;
            RaisePropertyChanged(nameof(booksCompleted));
            
            RaisePropertyChanged(nameof(Year));
            RaisePropertyChanged(nameof(HasBooksCompleted));

            UpdateStatistics();
        }

        private void SaveChallenge()
        {
            SelectedReadingChallenge.Target = Target;
            SelectedReadingChallenge.Completed = BooksCompleted;
        }

        private void LoadDataFromDisk()
        {
            Challenges.Clear();

            if (File.Exists(DataFilename))
            {
                var data = File.ReadAllText(DataFilename);

                var loadedChallenges = JsonConvert.DeserializeObject<List<ReadingChallenge>>(data);
                foreach (var challenge in loadedChallenges)
                {
                    Challenges.Add(challenge);
                }
            }

            var currentYearsChallenge = Challenges.FirstOrDefault(c => c.Year == DateTime.Now.Year);

            if (currentYearsChallenge == null)
            {
                challenges.Add(new ReadingChallenge { Year = DateTime.Now.Year, Target = 0, Completed = 0 });
            }

            SelectedReadingChallenge = Challenges.FirstOrDefault(c => c.Year == DateTime.Now.Year);
        }

        private void SaveDataToDisk()
        {
            if (File.Exists(DataFilename))
            {
                // TODO: make a back up the existing data before deleting
                File.Delete(DataFilename);
            }

            var data = JsonConvert.SerializeObject(Challenges);
            File.WriteAllText(DataFilename, data);

            // TODO: Remove the backup file made above
        }
    }

    public class DesignTimeChallengeViewModel : ChallengeViewModel
    {
        public DesignTimeChallengeViewModel()
        {
            // design time data
            Challenges.Add(new ReadingChallenge { Year = 2020, Target = 30, Completed = 23 });
            SelectedReadingChallenge = Challenges.First();
        }
    }
}
