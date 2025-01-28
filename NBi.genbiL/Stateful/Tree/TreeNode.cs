using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful.Tree;

public abstract class TreeNode
{
    protected TreeNode(string name) => Name = name;

    public RootNode? Root { get; internal set; }
    public int Level { get; internal set; }
    public BranchNode? Parent { get; internal set; }
    public string Name { get; }

}
