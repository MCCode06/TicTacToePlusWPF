using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TicTacToePlusWPF.Services;
using TicTacToePlusWPF.ViewModels;

namespace TicTacToePlusWPF.Views
{
    public partial class GreetingView : UserControl
    {
        private readonly INavigationService _navigationService;

        public GreetingView()
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                var navigationService = new NavigationService(mainWindow);
                DataContext = new GreetingViewModel(navigationService);
            }

            InitializeComponent();
            Loaded += GreetingView_Loaded;
        }

        private void GreetingView_Loaded(object sender, RoutedEventArgs e)
        {
            var animation = new DoubleAnimation
            {
                From = -800,
                To = 0,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            TitleText.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }
    }
}
