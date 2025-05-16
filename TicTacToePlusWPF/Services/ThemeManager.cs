using System;
using System.Linq;
using System.Windows;

namespace TicTacToePlusWPF.Services
{
    public static class ThemeManager
    {
        private static readonly Uri LightTheme = new Uri("/Resources/LightTheme.xaml", UriKind.Relative);
        private static readonly Uri DarkTheme = new Uri("/Resources/DarkTheme.xaml", UriKind.Relative);

        private static bool _isDark = true;

        public static bool IsDarkTheme => _isDark;

        public static void ToggleTheme()
        {
            Uri newThemeSource = _isDark ? LightTheme : DarkTheme;

            var dictionaries = Application.Current.Resources.MergedDictionaries;

            for (int i = dictionaries.Count - 1; i >= 0; i--)
            {
                var dict = dictionaries[i];
                if (dict.Source == LightTheme || dict.Source == DarkTheme)
                {
                    dictionaries.RemoveAt(i);
                }
            }

            dictionaries.Add(new ResourceDictionary { Source = newThemeSource });

            _isDark = !_isDark;
        }
    }
}
