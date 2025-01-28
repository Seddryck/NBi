using NBi.Xml;
using NBi.Xml.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful.Tree;

public class CleanupNode : TreeNode
{
    public CleanupStandaloneXml Content { get; }
    public CleanupNode(CleanupStandaloneXml cleanup)
        : base("cleanup") => Content = cleanup;
}
