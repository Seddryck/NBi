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
        private protected readonly List<TreeNode> ChildrenList = new List<TreeNode>();
        public IReadOnlyList<TreeNode> Children => ChildrenList;

        public string FullPath => Parent == Root ? Name : $@"{Parent.FullPath}\{Name}";

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
    }
}
