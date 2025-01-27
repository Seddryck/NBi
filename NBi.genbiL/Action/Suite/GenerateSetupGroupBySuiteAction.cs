using NBi.GenbiL.Stateful;
using NBi.GenbiL.Stateful.Tree;
using NBi.GenbiL.Templating;
using NBi.Xml.Decoration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Suite;

class GenerateSetupGroupBySuiteAction : GenerateSuiteAction<SetupStandaloneXml>
{
    public GenerateSetupGroupBySuiteAction(string groupByPattern)
        : base(false, groupByPattern) { }

    public override string Display { get => $"Generating Items in groups '{GroupByPattern}'"; }      

    protected override TreeNode BuildNode(SetupStandaloneXml content)
        => new SetupNode(content);
}
