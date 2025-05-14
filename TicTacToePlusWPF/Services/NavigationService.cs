// NavigationService.cs
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TicTacToePlusWPF.Services
{
    public class NavigationService : INavigationService
    {
        private readonly ContentControl _contentControl;

        public NavigationService(ContentControl contentControl)
        {
            _contentControl = contentControl ?? throw new ArgumentNullException(nameof(contentControl));
        }

        private void AnimateAndSetContent(UserControl newView)
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));

            fadeOut.Completed += (s, e) =>
            {
                _contentControl.Content = newView;
                newView.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            };

            if (_contentControl.Content is UserControl oldView)
                oldView.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            else
                AnimateAndSetContent(newView); 
        }

        public void NavigateToMainView() => AnimateAndSetContent(new Views.MainView());
        public void NavigateToSettingsView() => AnimateAndSetContent(new Views.SettingsView());
        public void NavigateToGameView() => AnimateAndSetContent(new Views.GameView());
        public void NavigateToGreetingView() => AnimateAndSetContent(new Views.GreetingView());
    }
}
