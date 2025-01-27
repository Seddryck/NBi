using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful.Tree;

public class RootNode : BranchNode
{
    public RootNode()
        : base(string.Empty)
    {
        Parent = null;
        Root = this;
        Level = 0;
    }

    public static string Path { get => "."; }
}
