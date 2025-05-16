using System.Windows;
using System.Windows.Controls;
using TicTacToePlusWPF.Services;
using TicTacToePlusWPF.ViewModels;

namespace TicTacToePlusWPF.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            var navigationService = new NavigationService(Application.Current.MainWindow);
            var settingsViewModel = new SettingsViewModel(App.GameSettingsInstance, navigationService);
            DataContext = settingsViewModel;
        }
    }
}
