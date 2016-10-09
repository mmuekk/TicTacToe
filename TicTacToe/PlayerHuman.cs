using System;

namespace TicTacToe
{
    internal class PlayerHuman : IPlayer
    {
        public void Play(bool?[] board, bool playerSymbol)
        {
            int move;
            bool isNumber;

            do
            {
                Console.Write("Select a move (1-9): ");

                var vStr = Console.ReadLine();
                isNumber = int.TryParse(vStr, out move);

                //zero based
                move--;
                if(move<0 || move>=9)
                    continue;

                if(isNumber && board[move] == null)
                    break;

            } while (true);

            board[move] = playerSymbol;
        }
    }
}