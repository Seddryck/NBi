using NBi.GenbiL.Stateful;
using NBi.GenbiL.Stateful.Tree;
using NBi.GenbiL.Templating;
using NBi.Xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.GenbiL.Action.Suite;

public class GenerateTestSuiteAction : GenerateSuiteAction<TestStandaloneXml>
{
    public GenerateTestSuiteAction(bool grouping)
        : base(grouping, RootNode.Path) { }

    protected GenerateTestSuiteAction(string groupByPattern)
        : base(false, groupByPattern) { }

    public override void Execute(GenerationState state)
    {
        var lastGeneration = Build(
                state.Templates, 
                state.CaseCollection.CurrentScope.Variables.ToArray(), 
                state.CaseCollection.CurrentScope.Content, 
                Grouping,
                state.Consumables
        );
        lastGeneration.ToList().ForEach(x => state.Suite.AddChild(BuildNode(x)));
    }
    protected override TreeNode BuildNode(TestStandaloneXml content)
        => new TestNode(content);

    public override string Display { get => $"Generating Tests {(Grouping ? "with" : "without")} grouping option"; }
}
