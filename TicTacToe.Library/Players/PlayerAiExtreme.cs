using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.Library.Helpers;

namespace TicTacToe.Library.Players
{
    public class PlayerAiExtreme : IPlayer
    {
        public ESymbol Symbol { get; set; }

        private TreeNode<Tree4Ai> _tree = null;
        private List<TreeNode<Tree4Ai>> _finalNodes = null;

        private Tree4Ai[] CreateOptionTree(TreeNode<Tree4Ai> node, Board board, bool moveSymbol, bool playerSymbol, int depth)
        {
            //stop recursive call
            if (Game.IsGameFinished(board))
            {
                _finalNodes.Add(node);
                return null;
            }

            //Strong feeling that this might be doable in parallel
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != null)
                    continue;

                var copyBoard = board.CopyBoard();
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

        public int Play(Board board)
        {
            // reset
            _finalNodes = new List<TreeNode<Tree4Ai>>();
            Tree4Ai.ResetId();

            //create Tree
            _tree = new TreeNode<Tree4Ai>(new Tree4Ai() { Board = board.CopyBoard(), MinMaxValue = 0, PlayerSymbol = playerSymbol, Depth = 0 });
            CreateOptionTree(_tree, board, playerSymbol, playerSymbol, 1);

            //TODO: check Last nodes

            //evaluate tree
            var f = _tree.FlattenTree(_tree);

            foreach (var node in f.Where(p => p.Children.Count == 0 && p.Value.MinMaxValue == null))
            {
                node.Value.EvaluateEndNodes();
            }

            ApplyMinAndMaxAlgorithm(f);

            //export
            //{
            //    //should disable 'Children' in TreeNode
            //    Log.InfoFormat("Exporting");
            //    var json = JsonConvert.SerializeObject(f, Formatting.Indented);
            //    File.WriteAllText("c:/temp/ticTacToe.json", json);
            //}
            //{
            //    Log.InfoFormat("Exporting tree");
            //    var json = JsonConvert.SerializeObject(tree, Formatting.Indented);
            //    File.WriteAllText("c:/temp/ticTacToe_tree.json", json);
            //}
            //{
            //    Log.InfoFormat("Exporting tree 2");

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
            var a = _tree.Children.OrderByDescending(p => p.Value.MinMaxValue).ThenBy(p => ChildrenCount(p)).FirstOrDefault();

            //{
            //    Log.InfoFormat("Exporting winning tree");
            //    var json = JsonConvert.SerializeObject(a, Formatting.Indented);
            //    File.WriteAllText("c:/temp/ticTacToe_winning_tree.json", json);
            //}

            board[a.Value.Move] = Symbol;
        }

        private int ChildrenCount(TreeNode<Tree4Ai> p)
        {
            var sum = p.Children.Count + p.Children.Sum(x => ChildrenCount(x));

            return sum;
        }

        private void ApplyMinAndMaxAlgorithm(List<TreeNode<Tree4Ai>> f)
        {
            var maxLevel = _finalNodes.Max(p => p.Value.Depth);

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
    }
}