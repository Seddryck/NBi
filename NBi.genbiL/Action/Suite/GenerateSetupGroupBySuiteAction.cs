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

namespace NBi.GenbiL.Action.Suite
{
    class GenerateSetupGroupBySuiteAction : GenerateSuiteAction<SetupStandaloneXml>
    {
        public GenerateSetupGroupBySuiteAction(string groupByPattern)
            : base(false, groupByPattern) { }

        public override string Display { get => $"Generating setups in groups '{GroupByPattern}'"; }

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
            var locatedSetups = lastGeneration.ToList().Zip(groupNames, (Setup, Location) => new { Setup, Location });

            locatedSetups.ToList().ForEach(x => state.Suite.FindChildBranch(x.Location).AddChild(new SetupNode(x.Setup)));
        }


    }
}
