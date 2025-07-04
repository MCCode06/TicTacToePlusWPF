﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TicTacToePlusWPF.ViewModels;
using TicTacToePlusWPF.Services;

namespace TicTacToePlusWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            Loaded += MainView_Loaded;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this);
            if (mainWindow != null)
            {
                var navigationService = new NavigationService(mainWindow);
                var mainViewModel = new MainViewModel(navigationService);
                DataContext = mainViewModel;
            }
            else
            {
                MessageBox.Show("Main window is not found.");
            }

            UpdateThemeToggleButtonContent();

            Loaded -= MainView_Loaded; // detach the event
        }

        private void ThemeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Services.ThemeManager.ToggleTheme();
            UpdateThemeToggleButtonContent();
        }

        private void UpdateThemeToggleButtonContent()
        {
            if (ThemeToggleButton == null) return;

            ThemeToggleButton.Content = ThemeManager.IsDarkTheme ? "🌞" : "🌙";
            ThemeToggleButton.ToolTip = ThemeManager.IsDarkTheme ? "Switch to Light Theme" : "Switch to Dark Theme";
        }
    }

}