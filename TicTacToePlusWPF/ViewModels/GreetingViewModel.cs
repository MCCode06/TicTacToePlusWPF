using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TicTacToePlusWPF.Services;

namespace TicTacToePlusWPF.ViewModels
{
    public class GreetingViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly DispatcherTimer _animationTimer = new();
        private string _greeting = "Welcome, fellow player!\n\nA We hope you enjoy it.";

        public string GreetingText { get; set; } = "";
        private int _charIndex = 0;

        public GreetingViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            StartGreetingAnimation();

            Application.Current.MainWindow.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    _navigationService.NavigateToMainView();
                }
            };
        }

        private void StartGreetingAnimation()
        {
            _animationTimer.Interval = TimeSpan.FromMilliseconds(40);
            _animationTimer.Tick += (s, e) =>
            {
                if (_charIndex < _greeting.Length)
                {
                    GreetingText += _greeting[_charIndex];
                    _charIndex++;
                    OnPropertyChanged(nameof(GreetingText));
                }
                else
                {
                    _animationTimer.Stop();
                }
            };
            _animationTimer.Start();
        }
    }
}
