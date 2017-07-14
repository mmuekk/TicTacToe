using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe
{
    internal class PlayerAiExtreme : IPlayer
    {
        private TreeNode<Tree4Ai> tree = null;
        private List<TreeNode<Tree4Ai>> finalNodes = null;

        private Tree4Ai[] CreateOptionTree(TreeNode<Tree4Ai> node, bool?[] board, bool moveSymbol, bool playerSymbol, int depth)
        {
            //stop recursive call
            if (Game.IsGameFinished(board))
            {
                finalNodes.Add(node);
                return null;
            }

            //Strong feeling that this might be doable in parallel
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != null)
                    continue;

                var copyBoard = CopyBoard(board);
                copyBoard[i] = moveSymbol;
                var childNode = node.AddChild(new Tree4Ai
                {
                    Board = copyBoard,
                    Move = i,
                    PlayerSymbol = playerSymbol,
                    MoveSymbol = moveSymbol,
                    Depth = depth
                });

                // recursive
                CreateOptionTree(childNode, copyBoard, !moveSymbol, playerSymbol, depth + 1);
            }

            return null;
        }

        public void Play(bool?[] board, bool playerSymbol)
        {
            // reset
            finalNodes = new List<TreeNode<Tree4Ai>>();
            Tree4Ai.ResetId();

            //create Tree
            tree = new TreeNode<Tree4Ai>(new Tree4Ai() { Board = CopyBoard(board), MinMaxValue = 0, PlayerSymbol = playerSymbol, Depth = 0 });
            CreateOptionTree(tree, board, playerSymbol, playerSymbol, 1);

            //TODO: check Last nodes

            //evaluate tree
            var f = tree.FlattenTree(tree);

            foreach (var node in f.Where(p => p.Children.Count == 0 && p.Value.MinMaxValue == null))
            {
                node.Value.EvaluateEndNodes();
            }

            ApplyMinAndMaxAlgorithm(f);

            //export
            //{
            //    //should disable 'Children' in TreeNode
            //    Console.WriteLine("Exporting");
            //    var json = JsonConvert.SerializeObject(f, Formatting.Indented);
            //    File.WriteAllText("c:/temp/ticTacToe.json", json);
            //}
            //{
            //    Console.WriteLine("Exporting tree");
            //    var json = JsonConvert.SerializeObject(tree, Formatting.Indented);
            //    File.WriteAllText("c:/temp/ticTacToe_tree.json", json);
            //}
            //{
            //    Console.WriteLine("Exporting tree 2");

            //    int i = 0;
            //    var sb = new StringBuilder();
            //    foreach (var node in finalNodes.OrderBy(p=>p.Value.MinMax))
            //    {
            //        sb.AppendLine("#" + ++i);
            //        sb.AppendLine(Game.ShowBoard(node.Value.Board));
            //        sb.AppendLine("MinMax: " + (node.Value.MinMax).ToString());
            //        sb.AppendLine("Depth: " + (node.Value.Depth).ToString());
            //        sb.AppendLine("Move: " + (node.Value.Move+1).ToString());
            //        sb.AppendLine("Player: " + (node.Value.IsMax ? "X" : "O").ToString());
            //        sb.AppendLine("-------------------------------------");
            //    }
            //    File.WriteAllText("c:/temp/ticTacToe_finalNodes.json", sb.ToString());
            //}

            //choose right Move
            var a = tree.Children.OrderByDescending(p => p.Value.MinMaxValue).ThenBy(p => ChildrenCount(p)).FirstOrDefault();

            //{
            //    Console.WriteLine("Exporting winning tree");
            //    var json = JsonConvert.SerializeObject(a, Formatting.Indented);
            //    File.WriteAllText("c:/temp/ticTacToe_winning_tree.json", json);
            //}

            board[a.Value.Move] = playerSymbol;
        }

        private int ChildrenCount(TreeNode<Tree4Ai> p)
        {
            var sum = p.Children.Count + p.Children.Sum(x => ChildrenCount(x));

            return sum;
        }

        private void ApplyMinAndMaxAlgorithm(List<TreeNode<Tree4Ai>> f)
        {
            var maxLevel = finalNodes.Max(p => p.Value.Depth);

            for (int level = maxLevel; level >= 0; level--)
            {
                var a = f.Where(p => p.Value.Depth == level).ToList();
                var b = a.Where(p => p.Value.MinMaxValue == null).ToList();

                foreach (var node in b)
                {
                    if (node.Value.MinMaxValue != null)
                        continue;

                    if (node.Children.All(p => p.Value.MinMaxValue != null))
                    {
                        node.Value.MinMaxValue = !node.Value.IsMax
                            ? node.Children.OrderByDescending(p => p.Value.MinMaxValue).FirstOrDefault().Value.MinMaxValue
                            : node.Children.OrderBy(p => p.Value.MinMaxValue).FirstOrDefault().Value.MinMaxValue;
                    }
                    else
                    {
                        throw new ArgumentNullException();
                    }
                }
            }
        }

        private bool?[] CopyBoard(bool?[] board)
        {
            var output = new bool?[board.Length];

            for (int i = 0; i < board.Length; i++)
            {
                output[i] = board[i];
            }

            return output;
        }
    }

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