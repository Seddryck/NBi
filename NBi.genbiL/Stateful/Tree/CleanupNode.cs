using NBi.Model;
using NBi.Model.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful.Tree;

public class CleanupNode : TreeNode
{
    public CleanupStandalone Content { get; }
    public CleanupNode(CleanupStandalone cleanup)
        : base("cleanup") => Content = cleanup;
}
