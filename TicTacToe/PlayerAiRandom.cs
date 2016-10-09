using System;

namespace TicTacToe
{
    internal class PlayerAiRandom : IPlayer
    {
        public PlayerAiRandom()
        {
        }

        public void Play(bool?[] board, bool playerSymbol)
        {
            var r = new Random();

            while (true)
            {
                var v = r.Next(0, 9);
                if (board[v] == null)
                {
                    board[v] = playerSymbol;
                    break;
                }
            }
        }
    }
}