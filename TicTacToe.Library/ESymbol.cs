using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Library
{
    public enum ESymbol
    {
        None,
        Cross,
        Circle
    }

    public class SymbolExtension
    {
        public static string Symbol2String(ESymbol symbol)
        {
            switch (symbol)
            {
                case ESymbol.None:
                    return " ";
                case ESymbol.Cross:
                    return "X";
                case ESymbol.Circle:
                    return "O";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
