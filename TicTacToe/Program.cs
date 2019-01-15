using TicTacToe.Library;
using TicTacToe.Library.Players;

namespace TicTacToe
{
    internal class Program
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            Log.InfoFormat("Game Starting");

            var game = new Game();

            var p1 = new PlayerAiRandom();
            var p2 = new PlayerHumanConsole();
            //var p2 = new PlayerAiExtreme();

            game.Run(p1, p2);
        }
    }
}