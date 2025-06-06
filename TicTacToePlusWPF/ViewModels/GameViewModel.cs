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

        public ICommand? ClickCommand { get; set; }
    }

    public class GameViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly GameSettings _settings;

        public ObservableCollection<GameCell> Cells { get; private set; } = new();

        private int _currentPlayerIndex = 0;
        public char CurrentPlayerSymbol => _settings.PlayerSymbols[_currentPlayerIndex];

        public int GridRows => _settings.GridRows;
        public int GridColumns => _settings.GridColumns;
        public string PlayerTurnDisplay => $"Player '{CurrentPlayerSymbol}' Turn";

        private string _gameMessage;
        public string GameMessage
        {
            get => _gameMessage;
            set => SetProperty(ref _gameMessage, value);
        }
        private CancellationTokenSource? _restartCancellationTokenSource;

        private string _countdownMessage;
        public string CountdownMessage
        {
            get => _countdownMessage;
            set => SetProperty(ref _countdownMessage, value);
        }

        public ICommand RestartCommand { get; }
        public ICommand BackToMenuCommand { get; }


        public GameViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _settings = App.Current.Resources["GameSettings"] as GameSettings;

            if (_settings == null)
            {
                _settings = new GameSettings(); 
                MessageBox.Show("⚠️ GameSettings not found. Using default.");
            }
            RestartCommand = new RelayCommand(_ => RestartGame());           
            BackToMenuCommand = new RelayCommand(_ => _navigationService.NavigateToMainView()); 


            GenerateBoard();
        }

        private void GenerateBoard()
        {
            Cells.Clear();

            for (int row = 0; row < _settings.GridRows; row++)
            {
                for (int col = 0; col < _settings.GridColumns; col++)
                {
                    int r = row;
                    int c = col;

                    var cell = new GameCell
                    {
                        Row = r,
                        Column = c,
                        ClickCommand = new RelayCommand(cellObj =>
                        {
                            var clickedCell = cellObj as GameCell;
                            Debug.WriteLine($"Clicked: {clickedCell?.Row},{clickedCell?.Column}");
                            HandleCellClick(r, c);
                        })
                    };

                    Cells.Add(cell);
                }
            }
            OnPropertyChanged(nameof(GridRows));
            OnPropertyChanged(nameof(GridColumns));
            OnPropertyChanged(nameof(CurrentPlayerSymbol));
            OnPropertyChanged(nameof(PlayerTurnDisplay));
        }

        private void RestartGame()
        {
            _restartCancellationTokenSource?.Cancel();
            _restartCancellationTokenSource?.Dispose();
            _restartCancellationTokenSource = null;

            _isGameOver = false;
            _currentPlayerIndex = 0;
            GameMessage = string.Empty;
            CountdownMessage = string.Empty;

            GenerateBoard();
            OnPropertyChanged(nameof(CurrentPlayerSymbol));
            OnPropertyChanged(nameof(PlayerTurnDisplay));
        }





        private bool _isGameOver = false;

        private void HandleCellClick(int row, int col)
        {
            if (_isGameOver) return;

            var cell = Cells.FirstOrDefault(c => c.Row == row && c.Column == col);
            if (cell == null || !string.IsNullOrWhiteSpace(cell.Symbol))
                return;

            cell.Symbol = CurrentPlayerSymbol.ToString();

            if (CheckWin(row, col))
            {
                _isGameOver = true;
                GameMessage = $"🎉 Player '{CurrentPlayerSymbol}' wins!";
                StartAutoRestartTimer();
                return;
            }

            if (Cells.All(c => !string.IsNullOrWhiteSpace(c.Symbol)))
            {
                _isGameOver = true;
                GameMessage = "🤝 It's a draw!";
                StartAutoRestartTimer();
                return;
            }

            _currentPlayerIndex = (_currentPlayerIndex + 1) % _settings.PlayerCount;
            OnPropertyChanged(nameof(CurrentPlayerSymbol));
            OnPropertyChanged(nameof(PlayerTurnDisplay));

            if (!_isGameOver && _settings.IsVsAi && _currentPlayerIndex == 1)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var (aiRow, aiCol) = MinimaxAIService.FindBestMove(Cells.ToList(), _settings, _settings.PlayerSymbols[1], _settings.PlayerSymbols[0]);
                    HandleCellClick(aiRow, aiCol);
                }));
            }
        }

        private async void StartAutoRestartTimer()
        {
            _restartCancellationTokenSource?.Cancel();
            _restartCancellationTokenSource?.Dispose();

            _restartCancellationTokenSource = new CancellationTokenSource();
            var token = _restartCancellationTokenSource.Token;

            try
            {
                for (int secondsLeft = 10; secondsLeft > 0; secondsLeft--)
                {
                    CountdownMessage = $"⏳ Restarting in {secondsLeft} second{(secondsLeft == 1 ? "" : "s")}...";
                    await Task.Delay(1000, token);
                }

                if (!token.IsCancellationRequested)
                {
                    CountdownMessage = string.Empty;
                    RestartGame();
                }
            }
            catch (TaskCanceledException)
            {
                CountdownMessage = string.Empty;
            }
        }





        private bool CheckWin(int row, int col)
        {
            string symbol = CurrentPlayerSymbol.ToString();

            return CountConsecutive(row, col, 1, 0, symbol) + CountConsecutive(row, col, -1, 0, symbol) - 1 >= _settings.WinCondition || 
                   CountConsecutive(row, col, 0, 1, symbol) + CountConsecutive(row, col, 0, -1, symbol) - 1 >= _settings.WinCondition || 
                   CountConsecutive(row, col, 1, 1, symbol) + CountConsecutive(row, col, -1, -1, symbol) - 1 >= _settings.WinCondition || 
                   CountConsecutive(row, col, 1, -1, symbol) + CountConsecutive(row, col, -1, 1, symbol) - 1 >= _settings.WinCondition;   
        }

        private int CountConsecutive(int row, int col, int dRow, int dCol, string symbol)
        {
            int count = 0;
            int r = row;
            int c = col;

            while (r >= 0 && r < _settings.GridRows && c >= 0 && c < _settings.GridColumns)
            {
                var cell = Cells.FirstOrDefault(cell => cell.Row == r && cell.Column == c);
                if (cell == null || cell.Symbol != symbol)
                    break;

                count++;
                r += dRow;
                c += dCol;
            }

            return count;
        }


    }
}
