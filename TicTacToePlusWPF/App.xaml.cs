using System.Configuration;
using System.Data;
using System.Windows;
using TicTacToePlusWPF.Models;
using TicTacToePlusWPF.Services;

namespace TicTacToePlusWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static GameSettings GameSettingsInstance { get; } = new GameSettings();
        protected override void OnStartup(StartupEventArgs e)
        {
            Resources["GameSettings"] = GameSettingsInstance;
            base.OnStartup(e);
            

            ApplyTheme("Resources/DarkTheme.xaml");

           
        }

        public void ApplyTheme(string themePath)
        {
            Resources.MergedDictionaries.Clear();

            var theme = new ResourceDictionary
            {
                Source = new Uri(themePath, UriKind.Relative)
            };

            Resources.MergedDictionaries.Add(theme);
        }
    }


}
