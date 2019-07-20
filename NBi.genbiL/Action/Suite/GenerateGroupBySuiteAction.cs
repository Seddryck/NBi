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
    class GenerateGroupBySuiteAction : GenerateSuiteAction
    {
        public string GroupByPattern { get; }

        public GenerateGroupBySuiteAction(string groupByPattern)
            : base(false) => GroupByPattern = groupByPattern;

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

        private void GenerateBranches(RootNode rootNode, IEnumerable<string[]> groupNames)
        {
            foreach (var groupName in groupNames)
            {
                BranchNode parentNode = rootNode;
                foreach (var nodeName in groupName)
                {
                    var groupNode = (parentNode.Children.FirstOrDefault(x => x.Name == nodeName) as GroupNode) ?? new GroupNode(nodeName);
                    if (!parentNode.Children.Any(x => x == groupNode))
                        parentNode.AddChild(groupNode);
                    parentNode = groupNode;
                }
            }
        }

        protected IEnumerable<string> RenderGroupNames(IEnumerable<string> templates, string[] variables, DataTable dataTable, IDictionary<string, object> globalVariables)
        {
            var cases = GetCases(dataTable, false);
            var names = new List<string>();
            foreach (var template in templates)
            {
                var engine = new StringTemplateEngine(template, variables);
                foreach (var indiv in cases)
                {
                    var newNames = engine.Build<string>(new List<List<List<object>>>() { indiv }, globalVariables).ToList();
                    names.AddRange(newNames);
                }
                    
            }
            return names;
        }
    }
}
