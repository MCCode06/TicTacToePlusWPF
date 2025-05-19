using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TicTacToePlusWPF.Models;
using TicTacToePlusWPF.Services;

namespace TicTacToePlusWPF.ViewModels
{
    public class SettingsViewModel : BaseViewModel, IDataErrorInfo
    {
        public GameSettings Settings { get; set; }

        public int GridRows
        {
            get => Settings.GridRows;
            set
            {
                if (value != Settings.GridRows)
                {
                    Settings.GridRows = value;
                    OnPropertyChanged();
                    AdjustWinCondition();
                    UpdatePlayerLimit();
                    CommandManager.InvalidateRequerySuggested(); 
                }
            }
        }
        
        public int GridColumns
        {
            get => Settings.GridColumns;
            set
            {
                if (value != Settings.GridColumns)
                {
                    Settings.GridColumns = value;
                    OnPropertyChanged();
                    AdjustWinCondition();
                    UpdatePlayerLimit();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public int WinCondition
        {
            get => Settings.WinCondition;
            set
            {
                int maxAllowed = Math.Min(GridRows, GridColumns);
                if (value >= 2 && value <= maxAllowed) 
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
                if (value != Settings.PlayerCount)
                {
                    Settings.PlayerCount = value;
                    OnPropertyChanged();
                    UpdatePlayerSymbols();
                    CommandManager.InvalidateRequerySuggested();
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

            SaveSettingsCommand = new RelayCommand(SaveSettings, _ => !HasErrors);
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
            int safeWinCondition = Math.Max(2, WinCondition);
            int maxPlayers = Math.Min(10, (GridRows * GridColumns - 1) / (safeWinCondition - 1) - 1);

            if (Settings.PlayerCount > maxPlayers)
            {
                Settings.PlayerCount = maxPlayers;
                OnPropertyChanged(nameof(PlayerCount));
            }
        }


        private void UpdatePlayerSymbols()
        {
            UpdatePlayerLimit();
            while (PlayerSymbols.Count < PlayerCount)
            {
                char newSymbol = (char)('A' + PlayerSymbols.Count);
                if (PlayerSymbols.Any(p => p.Symbol == newSymbol))
                {
                    newSymbol = '?';
                }
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


        public string Error => null; 

        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case nameof(GridRows):
                        if (GridRows <= 0)
                            error = "Grid Rows must be positive.";
                        break;
                    case nameof(GridColumns):
                        if (GridColumns <= 0)
                            error = "Grid Columns must be positive.";
                        break;
                    case nameof(WinCondition):
                        int maxAllowed = Math.Min(GridRows, GridColumns);
                        if (WinCondition < 3 || WinCondition > maxAllowed)
                            error = $"Win Condition must be between 3 and {maxAllowed}.";
                        break;
                    case nameof(PlayerCount):
                        int safeWinCondition = Math.Max(2, WinCondition); 
                        int maxPlayers = Math.Min(10, (GridRows * GridColumns - 1) / (safeWinCondition - 1) - 1);
                        if (PlayerCount < 2 || PlayerCount > maxPlayers)
                            error = $"Player Count must be between 2 and {maxPlayers}.";
                        break;

                }
                return error;
            }
        }

        public bool HasErrors =>
            !string.IsNullOrEmpty(this[nameof(GridRows)]) ||
            !string.IsNullOrEmpty(this[nameof(GridColumns)]) ||
            !string.IsNullOrEmpty(this[nameof(WinCondition)]) ||
            !string.IsNullOrEmpty(this[nameof(PlayerCount)]);
    }
}
