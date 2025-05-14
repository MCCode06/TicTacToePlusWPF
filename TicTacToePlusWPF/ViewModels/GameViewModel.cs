using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using TicTacToePlusWPF.Models;
using TicTacToePlusWPF.Services;

namespace TicTacToePlusWPF.ViewModels
{
    public class GameCell : BaseViewModel
    {
        public int Row { get; set; }
        public int Column { get; set; }
        private string _symbol = "";
        public string Symbol
        {
            get => _symbol;
            set => SetProperty(ref _symbol, value); 
        }
        public ICommand ClickCommand { get; set; }
    }

    public class GameViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly GameSettings _settings;

        public ObservableCollection<GameCell> Cells { get; } = new();

        private int _currentPlayerIndex = 0;
        public char CurrentPlayerSymbol => _settings.PlayerSymbols[_currentPlayerIndex];

        public int GridRows => _settings.GridRows;
        public int GridColumns => _settings.GridColumns;

        public GameViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _settings = App.Current.Resources["GameSettings"] as GameSettings;

            if (_settings == null)
            {
                _settings = new GameSettings(); 
                MessageBox.Show("⚠️ GameSettings not found. Using default.");
            }

            MessageBox.Show($"🔧 Initializing GameViewModel with grid {_settings.GridRows}x{_settings.GridColumns}");

            GenerateBoard();
        }

        private void GenerateBoard()
        {
            Cells.Clear();

            for (int row = 0; row < _settings.GridRows; row++)
            {
                for (int col = 0; col < _settings.GridColumns; col++)
                {
                    var cell = new GameCell
                    {
                        Row = row,
                        Column = col,
                        ClickCommand = new RelayCommand(_ => HandleCellClick(row, col))
                    };
                    Cells.Add(cell);
                }
            }
            OnPropertyChanged(nameof(GridRows));
            OnPropertyChanged(nameof(GridColumns));
        }


        private void HandleCellClick(int row, int col)
        {
            var cell = Cells.First(c => c.Row == row && c.Column == col);
            if (!string.IsNullOrWhiteSpace(cell.Symbol))
                return; // already filled

            cell.Symbol = CurrentPlayerSymbol.ToString();
            OnPropertyChanged(nameof(Cells));

            // TODO: Check win/draw condition here

            _currentPlayerIndex = (_currentPlayerIndex + 1) % _settings.PlayerCount;
        }
    }
}
