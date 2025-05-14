using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using TicTacToePlusWPF.Models;
using TicTacToePlusWPF.Services;

namespace TicTacToePlusWPF.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public GameSettings Settings { get; set; }

        public int GridRows
        {
            get => Settings.GridRows;
            set
            {
                if (value > 0)
                {
                    Settings.GridRows = value;
                    OnPropertyChanged();
                    AdjustWinCondition();
                    UpdatePlayerLimit();
                }
            }
        }

        public int GridColumns
        {
            get => Settings.GridColumns;
            set
            {
                if (value > 0)
                {
                    Settings.GridColumns = value;
                    OnPropertyChanged();
                    AdjustWinCondition();
                    UpdatePlayerLimit();
                }
            }
        }

        public int WinCondition
        {
            get => Settings.WinCondition;
            set
            {
                int maxAllowed = Math.Min(GridRows, GridColumns);
                if (value >= 3 && value <= maxAllowed)
                {
                    Settings.WinCondition = value;
                    OnPropertyChanged();
                    UpdatePlayerLimit();
                }
            }
        }

        public int PlayerCount
        {
            get => Settings.PlayerCount;
            set
            {
                int maxPlayers = Math.Min(10, (GridRows * GridColumns - 1) / (WinCondition - 1) - 1);
                if (value >= 2 && value <= maxPlayers && value != Settings.PlayerCount)
                {
                    Settings.PlayerCount = value;
                    OnPropertyChanged();

                    UpdatePlayerSymbols();
                }
            }
        }


        public ObservableCollection<PlayerSymbolViewModel> PlayerSymbols { get; set; }

        public ICommand SaveSettingsCommand { get; }
        public ICommand SwapSymbolsCommand { get; }

        private readonly INavigationService _navigationService;

        public SettingsViewModel(GameSettings settings, INavigationService navigationService)
        {
            Settings = settings;
            PlayerSymbols = new ObservableCollection<PlayerSymbolViewModel>(settings.PlayerSymbols.Select(s => new PlayerSymbolViewModel(s)));

            _navigationService = navigationService;

            SaveSettingsCommand = new RelayCommand(SaveSettings);
            SwapSymbolsCommand = new RelayCommand(SwapSymbols);
        }

        private void AdjustWinCondition()
        {
            if (Settings.WinCondition > Math.Min(GridRows, GridColumns))
                Settings.WinCondition = Math.Min(GridRows, GridColumns);
            OnPropertyChanged(nameof(WinCondition));
        }

        private void UpdatePlayerLimit()
        {
            int maxPlayers = Math.Min(10, (GridRows * GridColumns - 1) / (WinCondition - 1) - 1);
            if (Settings.PlayerCount > maxPlayers)
                Settings.PlayerCount = maxPlayers;
            OnPropertyChanged(nameof(PlayerCount));
        }

        private void UpdatePlayerSymbols()
        {
            while (PlayerSymbols.Count < PlayerCount)
            {
                char newSymbol = (char)('A' + PlayerSymbols.Count);
                if (PlayerSymbols.Any(p => p.Symbol == newSymbol)) { newSymbol = '?'; }

                PlayerSymbols.Add(new PlayerSymbolViewModel(newSymbol));
            }

            while (PlayerSymbols.Count > PlayerCount)
            {
                PlayerSymbols.RemoveAt(PlayerSymbols.Count - 1);
            }

            OnPropertyChanged(nameof(PlayerSymbols));
        }



        private void SaveSettings(object parameter)
        {
            Settings.PlayerSymbols = PlayerSymbols.Select(ps => ps.Symbol).ToList();
            _navigationService.NavigateToMainView();
        }


        private void SwapSymbols(object parameter)
        {
            if (parameter is Tuple<int, int> indices)
            {
                if (indices.Item1 < PlayerSymbols.Count && indices.Item2 < PlayerSymbols.Count)
                {
                    var temp = PlayerSymbols[indices.Item1];
                    PlayerSymbols[indices.Item1] = PlayerSymbols[indices.Item2];
                    PlayerSymbols[indices.Item2] = temp;
                }
            }
        }
    }

}
