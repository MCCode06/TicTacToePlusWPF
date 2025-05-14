using System.Configuration;
using System.Data;
using System.Windows;
using TicTacToePlusWPF.Models;

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
        }
    }


}
