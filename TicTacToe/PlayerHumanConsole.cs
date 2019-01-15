using System;
using TicTacToe.Library;

namespace TicTacToe
{
    public class PlayerHumanConsole : IPlayer
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ESymbol Symbol { get; set; }

        public int Play(Board board)
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

                if (!isNumber ||
                    move < 0 ||
                    move >= 9)
                {
                    Log.WarnFormat("Invalid move! Must be between [1,9]");
                    continue;
                }
         
                return move;

            } while (true);
        }
    }
}