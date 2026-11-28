using NBi.Model;
using NBi.Model.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful.Tree;

public class SetupNode : TreeNode
{
    public SetupStandalone Content { get; }
    public SetupNode(SetupStandalone setup)
        : base("setup") => Content = setup;
}
