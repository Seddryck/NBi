using NBi.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful.Tree;

public class TestNode : TreeNode
{
    public TestStandaloneXml Content { get; }
    public TestNode(TestStandaloneXml test)
        : base(test.Name) => Content = test;
}
