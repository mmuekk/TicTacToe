using System;
using System.Text;
using Newtonsoft.Json;

namespace TicTacToe.Library.Players
{
    [Serializable]
    internal class Tree4Ai
    {
        private static int idGlobal = 0;
        private int id;

        public static void ResetId()
        {
            idGlobal = 0;
        }

        public Tree4Ai()
        {
            id = idGlobal++;
        }

        public int Id { get { return id; } }

        [JsonIgnore]
        public bool?[] Board { get; set; }

        public int? MinMaxValue { get; set; }

        public int Move { get; set; }

        public bool PlayerSymbol { get; set; }

        public bool MoveSymbol { get; set; }

        public int Depth { get; set; }

        public string MinOrMax { get { return IsMax ? "MAX" : "MIN"; } }

        public bool IsMax { get { return MoveSymbol == PlayerSymbol; } }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Board.Length; i++)
            {
                sb.AppendFormat("{0}", Game.BoardValue(Board, i));
                sb.Append((i + 1) % 3 == 0 ? " $ " : ",");
            }

            sb.Append("(move: ");
            sb.Append(Move);
            sb.Append(" $ player: ");
            sb.Append(PlayerSymbol);
            sb.Append(" $ depth: ");
            sb.Append(Depth); ;
            sb.Append(" $ Max: ");
            sb.Append(MinMaxValue);
            sb.Append(" $ id: ");
            sb.Append(id);
            sb.Append(")");

            return sb.ToString();
        }

        public void EvaluateEndNodes()
        {
            var endConstant = 10;

            if (!Game.IsGameFinished(Board))
                throw new Exception();

            //this define lowest Nodes MinMax values
            if (Game.DoesPlayerWins(Board, MoveSymbol))
                MinMaxValue = IsMax ? endConstant : -endConstant;
            else if (Game.DoesPlayerWins(Board, !MoveSymbol))
                MinMaxValue = IsMax ? -endConstant : endConstant;
            else
                MinMaxValue = 0;

            MinMaxValue += IsMax ? 2 : -2;
        }
    }
}