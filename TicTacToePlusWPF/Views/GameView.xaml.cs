using System.Windows.Controls;
using TicTacToePlusWPF.ViewModels;
using TicTacToePlusWPF.Services;
using System.Windows;

namespace TicTacToePlusWPF.Views
{
    public partial class GameView : UserControl
    {
        public GameView()
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                var navigationService = new NavigationService(mainWindow);
                DataContext = new GameViewModel(navigationService);
            }

            InitializeComponent();
        }
    }

}
