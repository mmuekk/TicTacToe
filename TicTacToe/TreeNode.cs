using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TicTacToe
{
    [Serializable]
    public class TreeNode<T>
    {
        private readonly T value;
        private readonly List<TreeNode<T>> children = new List<TreeNode<T>>();

        public TreeNode()
        {
            
        }

        public TreeNode(T value)
        {
            this.value = value;
        }

        public TreeNode<T> this[int i]
        {
            get { return children[i]; }
        }

        [JsonIgnore]
        public TreeNode<T> Parent { get; private set; }
 
        public T Value { get { return value; } }

        //[JsonIgnore]
        public ReadOnlyCollection<TreeNode<T>> Children
        {
            get { return children.AsReadOnly(); }
        }

        public TreeNode<T> AddChild(T value)
        {
            var node = new TreeNode<T>(value) { Parent = this };
            children.Add(node);
            return node;
        }

        public TreeNode<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        public bool RemoveChild(TreeNode<T> node)
        {
            return children.Remove(node);
        }

        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in children)
                child.Traverse(action);
        }

        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Union(children.SelectMany(x => x.Flatten()));
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