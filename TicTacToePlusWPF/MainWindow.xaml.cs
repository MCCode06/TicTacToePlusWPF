using System.Windows;
using TicTacToePlusWPF.Services;
using TicTacToePlusWPF.ViewModels;

namespace TicTacToePlusWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var navigationService = new NavigationService(this);

            var mainViewModel = new MainViewModel(navigationService);

            DataContext = mainViewModel;

            navigationService.NavigateToMainView();
        }
    }
}
