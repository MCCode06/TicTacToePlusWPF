using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using TicTacToePlusWPF.Models;
using TicTacToePlusWPF.ViewModels;

public static class MinimaxAIService
{
    public static (int row, int col) FindBestMove(List<GameCell> cells, GameSettings settings, char aiSymbol, char humanSymbol)
    {
        int bestScore = int.MinValue;
        (int row, int col) bestMove = (-1, -1);

        foreach (var cell in cells.Where(c => string.IsNullOrEmpty(c.Symbol)))
        {
            cell.Symbol = aiSymbol.ToString();
            int score = Minimax(cells, settings, 0, false, aiSymbol, humanSymbol);
            cell.Symbol = string.Empty;

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = (cell.Row, cell.Column);
            }
        }

        return bestMove;
    }

    private static int Minimax(List<GameCell> cells, GameSettings settings, int depth, bool isMaximizing, char aiSymbol, char humanSymbol)
    {
        var result = EvaluateBoard(cells, settings, aiSymbol, humanSymbol);
        if (result.HasValue)
            return result.Value;

        if (isMaximizing)
        {
            int best = int.MinValue;
            foreach (var cell in cells.Where(c => string.IsNullOrEmpty(c.Symbol)))
            {
                cell.Symbol = aiSymbol.ToString();
                best = Math.Max(best, Minimax(cells, settings, depth + 1, false, aiSymbol, humanSymbol));
                cell.Symbol = string.Empty;
            }
            return best;
        }
        else
        {
            int best = int.MaxValue;
            foreach (var cell in cells.Where(c => string.IsNullOrEmpty(c.Symbol)))
            {
                cell.Symbol = humanSymbol.ToString();
                best = Math.Min(best, Minimax(cells, settings, depth + 1, true, aiSymbol, humanSymbol));
                cell.Symbol = string.Empty;
            }
            return best;
        }
    }

    private static int? EvaluateBoard(List<GameCell> cells, GameSettings settings, char ai, char human)
    {
        bool Check(char symbol)
        {
            foreach (var cell in cells.Where(c => c.Symbol == symbol.ToString()))
            {
                var vm = new GameViewModelStub(settings, cells);
                if (vm.CheckWin(cell.Row, cell.Column, symbol))
                    return true;
            }
            return false;
        }

        if (Check(ai)) return 10;
        if (Check(human)) return -10;
        if (cells.All(c => !string.IsNullOrEmpty(c.Symbol))) return 0;

        return null;
    }

    private class GameViewModelStub : GameViewModel
    {
        public GameViewModelStub(GameSettings settings, List<GameCell> existingCells) : base(null)
        {
            typeof(GameViewModel).GetField("_settings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(this, settings);
            var cellsProperty = typeof(GameViewModel).GetProperty("Cells", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            cellsProperty?.SetValue(this, new ObservableCollection<GameCell>(existingCells));

        }

        public bool CheckWin(int row, int col, char symbol)
        {
            typeof(GameViewModel).GetField("_currentPlayerIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(this, 0);
            return (bool)typeof(GameViewModel).GetMethod("CheckWin", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(this, new object[] { row, col });
        }
    }
}
