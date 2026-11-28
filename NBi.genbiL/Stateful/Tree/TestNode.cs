using NBi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful.Tree;

public class TestNode : TreeNode
{
    public StandaloneTest Content { get; }
    public TestNode(StandaloneTest test)
        : base(test.Name) => Content = test;
}
