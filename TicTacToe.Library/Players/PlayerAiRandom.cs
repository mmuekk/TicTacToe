using System;

namespace TicTacToe.Library.Players
{
    public class PlayerAiRandom : IPlayer
    {
        public ESymbol Symbol { get; set; }

        public int Play(Board board)
        {
            var r = new Random();

            while (true)
            {
                var v = r.Next(0, 9);

                if (board[v] == ESymbol.None)
                {
                    return v;
                }
            }
        }
    }
}