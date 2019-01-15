using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace TicTacToe.Library.Helpers
{
    [Serializable]
    public class TreeNode<T>
    {
        private readonly List<TreeNode<T>> _children = new List<TreeNode<T>>();

        public TreeNode()
        {
        }

        public TreeNode(T value)
        {
            this.Value = value;
        }

        public TreeNode<T> this[int i] => _children[i];

        [JsonIgnore]
        public TreeNode<T> Parent { get; private set; }
 
        public T Value { get; }

        //[JsonIgnore]
        public ReadOnlyCollection<TreeNode<T>> Children => _children.AsReadOnly();

        public TreeNode<T> AddChild(T value)
        {
            var node = new TreeNode<T>(value) { Parent = this };
            _children.Add(node);
            return node;
        }

        public TreeNode<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        public bool RemoveChild(TreeNode<T> node)
        {
            return _children.Remove(node);
        }

        public void Traverse(Action<T> action)
        {
            action(Value);

            foreach (var child in _children)
                child.Traverse(action);
        }

        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Union(_children.SelectMany(x => x.Flatten()));
        }

        public List<TreeNode<T>> FlattenTree(TreeNode<T> node)
        {
            var output = new List<TreeNode<T>> {node};

            if (node.Children == null)
                return output;

            foreach (var treeNode in node.Children)
            {
                output.AddRange(FlattenTree(treeNode));
            }

            return output;
        }
    }
}