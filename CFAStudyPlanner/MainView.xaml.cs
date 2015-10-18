using System.Windows;

namespace CFAStudyPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void ReadingCheckbox_Click(object sender, RoutedEventArgs e)
        {
            // hack
            textProgress.Text = MainViewModel.CalculateProgressNumber();
            textRemaining.Text = MainViewModel.CalculateProgressMsg();
        }
    }
}
