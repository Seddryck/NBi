using NBi.Xml;
using NBi.Xml.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful.Tree;

public class SetupNode : TreeNode
{
    public SetupStandaloneXml Content { get; }
    public SetupNode(SetupStandaloneXml setup)
        : base("setup") => Content = setup;
}
