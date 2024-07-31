using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful.Tree
{
    public class BranchNode : TreeNode, IEnumerable<TreeNode>
    {
        protected readonly List<TreeNode> ChildrenList = [];
        public IReadOnlyList<TreeNode> Children => ChildrenList;

        public string FullPath => Parent == Root ? Name : $@"{Parent?.FullPath}\{Name}";

        public BranchNode(string name) 
            : base(name) { }

        public IEnumerator<TreeNode> GetEnumerator() => ChildrenList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void AddChild(TreeNode item)
        {
            item.Parent = this;
            item.Root = Root;
            item.Level = Level + 1;
            ChildrenList.Add(item);
        }

        public void AddChildren(IEnumerable<TreeNode> items)
            => items.ToList().ForEach(x => AddChild(x));

        public BranchNode FindChildBranch(string path)
        {
            var node = this;
            var subPathes = path.Split(['|']);
            foreach (var subPath in subPathes)
                node = node?.Children.FirstOrDefault(x => x is BranchNode && x.Name == subPath) as BranchNode;
            return node!;
        }

        public BranchNode? GetChildBranch(string path)
        {
            var node = this;
            if (path == RootNode.Path)
                return Root;

            var subPathes = path.Split(['|']);
            foreach (var subPath in subPathes)
            {
                if (node.Children.FirstOrDefault(x => x is BranchNode && x.Name == subPath) is not BranchNode child)
                {
                    child = new GroupNode(subPath);
                    node.AddChild(child);
                }
                node = child;
            }
            return node;
        }
    }
}
