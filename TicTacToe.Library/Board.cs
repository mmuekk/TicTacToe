using System;
using System.Linq;
using System.Text;

namespace TicTacToe.Library
{
    public class Board
    {
        private ESymbol[] _board;

        public ESymbol this[int key]
        {
            get => _board[key];
            set => _board[key] = value;
        }

        public Board()
        {
            _board = new ESymbol[9];
        }

        public ESymbol[] CopyBoard()
        {
            var output = new ESymbol[_board.Length];

            for (int i = 0; i < _board.Length; i++)
            {
                output[i] = _board[i];
            }

            return output;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int boardIndex = 0; boardIndex < 9; boardIndex++)
            {
                if ((1 + boardIndex) % 3 == 0)
                {
                    sb.AppendLine(SymbolExtension.Symbol2String(_board[boardIndex]));
                    continue;
                }

                sb.AppendFormat("{0}|", SymbolExtension.Symbol2String(_board[boardIndex]));
            }

            return sb.ToString();
        }

        public bool All(Func<ESymbol, bool> predicate)
        {
            return _board.All(predicate);
        }
    }
}