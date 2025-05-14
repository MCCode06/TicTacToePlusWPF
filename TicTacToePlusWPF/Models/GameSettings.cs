using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToePlusWPF.Models
{
    public class GameSettings
    {
        public int GridRows { get; set; } = 3;
        public int GridColumns { get; set; } = 3;
        public int WinCondition { get; set; } = 3;
        public int PlayerCount { get; set; } = 2;
        public List<char> PlayerSymbols { get; set; } = new() { 'X', 'O' };
    }

}
