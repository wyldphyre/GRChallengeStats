using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GRChallengeStatsWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ChallengeViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            var challenge = new GoodReadsLibrary.ReadingChallenge { Year = 2020, Target = 30, Completed = 6 };

            viewModel = new ChallengeViewModel
            {
                Challenge = challenge
            };

            DataContext = viewModel;
        }
    }
}
