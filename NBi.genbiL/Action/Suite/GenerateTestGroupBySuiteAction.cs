using NBi.GenbiL.Stateful;
using NBi.GenbiL.Stateful.Tree;
using NBi.GenbiL.Templating;
using NBi.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Suite;

public class GenerateTestGroupBySuiteAction : GenerateSuiteAction<StandaloneTest>
{
    public GenerateTestGroupBySuiteAction(string groupByPattern)
        : base(false, groupByPattern) { }

    protected override TreeNode BuildNode(StandaloneTest content)
        => new TestNode(content);

    public override string Display { get => $"Generating tests in groups '{GroupByPattern}'"; }
}
