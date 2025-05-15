using System;
using System.Windows;

namespace TicTacToePlusWPF.Services
{
    public static class ThemeManager
    {
        private static readonly Uri LightTheme = new Uri("/Resources/LightTheme.xaml", UriKind.Relative);
        private static readonly Uri DarkTheme = new Uri("/Resources/DarkTheme.xaml", UriKind.Relative);

        private static bool _isDark = true;

        public static void ToggleTheme()
        {
            ResourceDictionary newTheme = new ResourceDictionary
            {
                Source = _isDark ? LightTheme : DarkTheme
            };

            for (int i = 0; i < Application.Current.Resources.MergedDictionaries.Count; i++)
            {
                var dict = Application.Current.Resources.MergedDictionaries[i];
                if (dict.Source == LightTheme || dict.Source == DarkTheme)
                {
                    Application.Current.Resources.MergedDictionaries.RemoveAt(i);
                    break;
                }
            }

            Application.Current.Resources.MergedDictionaries.Add(newTheme);

            _isDark = !_isDark;
        }
    }
}
