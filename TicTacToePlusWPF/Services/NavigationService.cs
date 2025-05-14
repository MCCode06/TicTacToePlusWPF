using System.Windows;
using System.Windows.Controls;
using TicTacToePlusWPF.Views;

namespace TicTacToePlusWPF.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Window _mainWindow;

        // Ensure that the window is passed correctly to the service
        public NavigationService(Window mainWindow)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
        }

        public void NavigateToMainView()
        {
            var mainView = new MainView();  
            var contentControl = _mainWindow.Content as ContentControl;

            if (contentControl != null)
            {
                contentControl.Content = mainView;  
            }
            else
            {
                throw new InvalidOperationException("MainContent ContentControl not found.");
            }
        }

        public void NavigateToSettingsView()
        {
            var settingsView = new SettingsView();  
            var contentControl = _mainWindow.Content as ContentControl;

            if (contentControl != null)
            {
                contentControl.Content = settingsView;
            }
            else
            {
                throw new InvalidOperationException("MainContent ContentControl not found.");
            }
        }

        public void NavigateToGameView()
        {
            var gameView = new GameView();  
            var contentControl = _mainWindow.Content as ContentControl;

            if (contentControl != null)
            {
                contentControl.Content = gameView;
            }
            else
            {
                throw new InvalidOperationException("MainContent ContentControl not found.");
            }
        }
    }
}
