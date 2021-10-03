using GoodReadsLibrary;
using MvvmLibrary;

namespace GRChallengeStatsWPF
{
    public class ChallengeEditViewModel : ObservableObject
    {
        private ReadingChallenge readingChallenge;

        public ChallengeEditViewModel(ReadingChallenge readingChallenge)
        {
            this.readingChallenge = readingChallenge;
        }

        public ChallengeEditViewModel()
        {
            // Design time data
            readingChallenge = new ReadingChallenge
            {
                Year = 2015,
                Target = 10,
                Completed = 5
            };
        }

        public int Year
        {
            get => readingChallenge.Year;
            set
            {
                readingChallenge.Year = value;
                RaisePropertyChanged();
            }
        }

        public int Target
        {
            get => readingChallenge.Target;
            set
            {
                readingChallenge.Target = value;
                RaisePropertyChanged();
            }
        }

        public int Completed
        {
            get => readingChallenge.Completed;
            set
            {
                readingChallenge.Completed = value;
                RaisePropertyChanged();
            }
        }
    }

    public class DesignTimeChallengeEditViewModel : ChallengeEditViewModel
    {
        public DesignTimeChallengeEditViewModel()
        {
            Year = 2015;
            Target = 10;
            Completed = 5;
        }
    }
}