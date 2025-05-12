using System.Windows.Input;

namespace TicTacToePlusWPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand StartGameCommand { get; set; }
        public ICommand OpenSettingsCommand { get; set; }
        public ICommand OpenHelpCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public MainViewModel()
        {
            StartGameCommand = new RelayCommand(StartGame);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
            OpenHelpCommand = new RelayCommand(OpenHelp);
            ExitCommand = new RelayCommand(Exit);
        }

        private void StartGame()
        {
            // Logic to start the game
        }

        private void OpenSettings()
        {
            // Logic to open settings
        }

        private void OpenHelp()
        {
            // Logic to open help
        }

        private void Exit()
        {
            // Logic to exit the game
            System.Windows.Application.Current.Shutdown();
        }
    }
}
