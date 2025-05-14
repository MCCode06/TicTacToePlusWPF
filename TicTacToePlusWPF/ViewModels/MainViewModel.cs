using System.Windows;
using System.Windows.Input;
using TicTacToePlusWPF.Services;
using TicTacToePlusWPF.Views;

namespace TicTacToePlusWPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand StartGameCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand OpenHelpCommand { get; }
        public ICommand ExitCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            StartGameCommand = new RelayCommand(OpenGameView);
            OpenSettingsCommand = new RelayCommand(OpenSettingsView);
            OpenHelpCommand = new RelayCommand(OpenHelpView);
            ExitCommand = new RelayCommand(ExitApp);
        }

        private void OpenGameView(object obj)
        {
            _navigationService.NavigateToGameView();
        }

        private void OpenSettingsView(object obj)
        {
            _navigationService.NavigateToSettingsView();
        }

        private void OpenHelpView(object obj)
        {
            // Handle Help View
        }

        private void ExitApp(object obj)
        {
            Application.Current.Shutdown();
        }
    }
}