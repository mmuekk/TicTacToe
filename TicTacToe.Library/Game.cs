using System.Collections.Generic;

namespace TicTacToe.Library
{
    public class Game
    {
        private IPlayer[] _players;
        private Board _board;

        public void Run(IPlayer player1, IPlayer player2)
        {
            _board = new Board();

            SetupPlayers(player1, player2);

            OnStartingGame

            for (int turn = 1; turn < 10; turn++)
            {
                OnTurnStarting

                Log.InfoFormat("Move {0}:", turn);

                var playerIndex = turn % 2;

                var player = _players[playerIndex];

                GetPlayerMove(player);

                if (DoesPlayerWins(player.Symbol))
                {
                    Log.InfoFormat(_board.ToString());
                    Log.InfoFormat("Player '{1}' with '{0}' won the game",
                        SymbolExtension.Symbol2String(player.Symbol),
                        _players[playerIndex].ToString());
                    break;
                }
                else if (_board.All(p => p != ESymbol.None))
                {
                    Log.InfoFormat("Draw");
                    break;
                }
                else
                {
                    Log.InfoFormat(_board.ToString());
                }

                OnGameEnded 

                OnTurnEnding
            }
        }

        private void GetPlayerMove(IPlayer player)
        {
            do
            {
                var move = player.Play(_board);

                if (_board[move] != ESymbol.None)
                {
                    OnErrorMessage
                    Log.WarnFormat("Invalid move! Position already occupied");
                }
                else
                {
                    _board[move] = player.Symbol;
                    break;
                }

            } while (true);
        }

        private void SetupPlayers(IPlayer player1, IPlayer player2)
        {
            if (player1.Symbol == ESymbol.None ||
                player2.Symbol == ESymbol.None ||
                player1.Symbol == player2.Symbol)
            {
                player1.Symbol = ESymbol.Circle;
                player2.Symbol = ESymbol.Cross;
            }

            _players = new[] { player1, player2 };
        }

        public bool IsGameFinished()
        {
            return DoesPlayerWins(ESymbol.Circle) ||
                   DoesPlayerWins(ESymbol.Cross) ||
                   _board.All(p => p != ESymbol.None);
        }

        public bool DoesPlayerWins(ESymbol playerSymbol)
        {
            var winningPossibilities = new List<int[]>
            {
                new[] {1, 2, 3},
                new[] {4, 5, 6},
                new[] {7, 8, 9},
                new[] {1, 4, 7},
                new[] {2, 5, 8},
                new[] {3, 6, 9},
                new[] {1, 5, 9},
                new[] {7, 5, 3},
            };

            foreach (var possibility in winningPossibilities)
            {
                var won = true;

                foreach (var posItem in possibility)
                {
                    //zero based
                    var i = posItem - 1;

                    if (_board[i] == ESymbol.None || _board[i] != playerSymbol)
                        won = false;
                }

                if (won)
                    return true;
            }

            return false;
        }
    }
}