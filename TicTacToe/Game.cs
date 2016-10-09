using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe
{
    public class Game
    {
        private IPlayer[] players;
        private bool?[] board;

        public Game()
        {
        }

        private void InitGame()
        {
            players = new IPlayer[]
            {
                new PlayerAiRandom(),
                //new PlayerHuman(),
                new PlayerAiExtreme()
            };

            board = new bool?[9];
        }

        public void Run()
        {
            InitGame();

            for (int boardIndex = 0; boardIndex < 9; boardIndex++)
            {
                Console.WriteLine("Move {0}:", boardIndex + 1);

                var playerIndex = boardIndex % 2;

                players[playerIndex].Play(board, playerIndex == 0);

                if (DoesPlayerWins(board, playerIndex == 0))
                {
                    Console.WriteLine(ShowBoard(board));
                    Console.WriteLine("Player '{1}' with '{0}'  won the game", playerIndex == 0 ? "X" : "O",
                        players[playerIndex].ToString());
                    break;
                }
                else if(DoesPlayerWins(board, playerIndex != 0))
                {
                    Console.WriteLine(ShowBoard(board));
                    Console.WriteLine("Player '{1}' with '{0}'  won the game", playerIndex != 0 ? "X" : "O",
                        players[playerIndex].ToString());
                    break;
                }
                else if (board.All(p => p != null))
                {
                    Console.WriteLine("Draw");
                    break;
                }
                else
                {
                    Console.WriteLine(ShowBoard(board));
                }
            }
        }

        public static string ShowBoard(bool?[] board)
        {
            var sb = new StringBuilder();

            for (int boardIndex = 0; boardIndex < 9; boardIndex++)
            {
                if ((1 + boardIndex) % 3 == 0)
                {
                    sb.AppendLine(BoardValue(board, boardIndex));
                    continue;
                }

                sb.AppendFormat("{0}|", BoardValue(board, boardIndex));
            }

            return sb.ToString();
        }

        public static string BoardValue(bool?[] board, int boardIndex)
        {
            return board[boardIndex] == null ? " " : (board[boardIndex].Value ? "X" : "O");
        }

        public static bool IsGameFinished(bool?[] board)
        {
            return DoesPlayerWins(board, true) ||
                   DoesPlayerWins(board, false) ||
                   board.All(p => p != null);
        }

        public static bool DoesPlayerWins(bool?[] board, bool playerSymbol)
        {
            var winnningPossibilities = new List<int[]>
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

            foreach (var possibility in winnningPossibilities)
            {
                var won = true;

                foreach (var posItem in possibility)
                {
                    //zero based
                    var i = posItem - 1;

                    if (board[i] == null || board[i].Value != playerSymbol)
                        won = false;
                }

                if (won)
                    return true;
            }

            return false;
        }
    }
}