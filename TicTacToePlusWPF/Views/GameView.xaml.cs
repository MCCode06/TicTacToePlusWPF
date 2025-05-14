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
            InitializeComponent();
            Loaded += GameView_Loaded;
        }

        private void GameView_Loaded(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this);
            if (mainWindow != null)
            {
                var navigationService = new NavigationService(mainWindow);
                var gameViewModel = new GameViewModel(navigationService);
                DataContext = gameViewModel;
            }
            else
            {
                MessageBox.Show("Main window is not found.");
            }

            Loaded -= GameView_Loaded; // detach the event
        }
    }
}
