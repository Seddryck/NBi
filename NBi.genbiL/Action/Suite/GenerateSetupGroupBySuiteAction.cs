using NBi.GenbiL.Stateful;
using NBi.GenbiL.Stateful.Tree;
using NBi.GenbiL.Templating;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Suite
{
    class GenerateGroupBySuiteAction : GenerateTestSuiteAction
    {
        public GenerateGroupBySuiteAction(string groupByPattern)
            : base(groupByPattern) { }

        public override void Execute(GenerationState state)
        {
            var lastGeneration = Build(
                    state.Templates,
                    state.CaseCollection.CurrentScope.Variables.ToArray(),
                    state.CaseCollection.CurrentScope.Content,
                    Grouping,
                    state.Consumables
            );

            var patternArray = new List<string>();
            for (int i = 0; i < state.Templates.Count(); i++)
                patternArray.Add(GroupByPattern);

            var groupNames = RenderGroupNames(
                    patternArray,
                    state.CaseCollection.CurrentScope.Variables.ToArray(),
                    state.CaseCollection.CurrentScope.Content,
                    state.Consumables
            );

            GenerateBranches(state.Suite, groupNames.Distinct().Select(x => x.Split('|')));
            var locatedTests = lastGeneration.ToList().Zip(groupNames, (Test, Location) => new { Test, Location });

            locatedTests.ToList().ForEach(x => state.Suite.FindChildBranch(x.Location).AddChild(new TestNode(x.Test)));
        }
    }
}
