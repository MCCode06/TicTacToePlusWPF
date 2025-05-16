using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToePlusWPF.ViewModels
{
    public class PlayerSymbolViewModel : BaseViewModel
    {
        private char _symbol;

        public char Symbol
        {
            get => _symbol;
            set => SetProperty(ref _symbol, value);
        }

        public PlayerSymbolViewModel(char symbol)
        {
            _symbol = symbol;
        }

    }
}
