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
    public abstract class GenerateSuiteAction<T> : ISuiteAction
    {
        public bool Grouping { get; }
        public string GroupByPattern { get; }

        protected GenerateSuiteAction(bool grouping, string groupByPattern)
            => (Grouping, GroupByPattern) = (grouping, groupByPattern);

        public abstract string Display { get; }

        protected List<List<List<object>>> GetCases(DataTable dataTable, bool useGrouping)
        {
            if (dataTable.Rows.Count == 0)
                return [];

            int groupedColumn = dataTable.Rows[0].ItemArray.Length - 1;

            var variableTests = new List<List<List<object>>>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var isIdentical = (i != 0) && useGrouping;
                var grouping = dataTable.Rows[i].ItemArray.ToList();
                grouping.RemoveAt(groupedColumn);
                var k = 0;
                while (k < grouping.Count && isIdentical)
                {
                    if (grouping[k]!.ToString() != variableTests[^1][k][0].ToString())
                        isIdentical = false;
                    k++;
                }


                if (!isIdentical)
                {
                    variableTests.Add([]);
                    for (int j = 0; j < dataTable.Rows[i].ItemArray.Length; j++)
                    {
                        variableTests[^1].Add([]);
                        if (dataTable.Rows[i].ItemArray[j] is IEnumerable<string>)
                        {
                            foreach (var item in (IEnumerable<string>)dataTable.Rows[i]!.ItemArray[j]!)
                                variableTests[^1][j].Add(item);
                        }
                        else
                            variableTests[^1][j].Add(dataTable.Rows[i]!.ItemArray[j]!.ToString()!);
                    }
                }
                else
                    variableTests[^1][groupedColumn].Add(dataTable.Rows[i]!.ItemArray[groupedColumn]!.ToString()!);
            }

            return variableTests;
        }

        public virtual void Execute(GenerationState state)
        {
            var lastGeneration = Build(
                    state.Templates,
                    state.CaseCollection.CurrentScope.Variables.ToArray(),
                    state.CaseCollection.CurrentScope.Content,
                    Grouping,
                    state.Consumables
            );

            var patternArray = new List<string>();
            for (int i = 0; i < state.Templates.Count; i++)
                patternArray.Add(GroupByPattern);

            var groupNames = RenderGroupNames(
                    patternArray,
                    state.CaseCollection.CurrentScope.Variables.ToArray(),
                    state.CaseCollection.CurrentScope.Content,
                    state.Consumables
            );

            GenerateBranches(state.Suite, groupNames.Distinct().Select(x => x.Split('|')));
            var locatedItems = lastGeneration.ToList().Zip(groupNames, (Item, Location) => new { Item, Location });

            locatedItems.ToList().ForEach(x => state.Suite.FindChildBranch(x.Location).AddChild(BuildNode(x.Item)));
        }

        protected abstract TreeNode BuildNode(T content);

        protected IEnumerable<T> Build(string template, string[] variables, DataTable dataTable, bool useGrouping, IDictionary<string, object> globalVariables)
        {
            var generator = new StringTemplateEngine(template, variables);
            var cases = GetCases(dataTable, useGrouping);
            generator.Progressed += new EventHandler<ProgressEventArgs>(OnTestGenerated!);
            var lastGeneration = generator.Build<T>(cases, globalVariables).ToList();
            generator.Progressed -= new EventHandler<ProgressEventArgs>(OnTestGenerated!);
            return lastGeneration;
        }

        protected IEnumerable<T> Build(IEnumerable<string> templates, string[] variables, DataTable dataTable, bool useGrouping, IDictionary<string, object> globalVariables)
        {
            if (!templates.Any())
                throw new ArgumentException("No template was specified. You must at least define a template before generating a test suite.");

            if (templates.Count() == 1)
                return Build(templates.ElementAt(0), variables, dataTable, useGrouping, globalVariables);
            else
            {
                var lastGeneration = new List<T>();
                var cases = GetCases(dataTable, useGrouping);
                foreach (var indiv in cases)
                {
                    foreach (var template in templates)
                    {
                        var engine = new StringTemplateEngine(template, variables);
                        engine.Progressed += new EventHandler<ProgressEventArgs>(OnTestGenerated!);
                        lastGeneration.AddRange(engine.Build<T>([indiv], globalVariables).ToList());
                        engine.Progressed -= new EventHandler<ProgressEventArgs>(OnTestGenerated!);
                    }
                }
                return lastGeneration;
            }
        }

        protected void GenerateBranches(RootNode rootNode, IEnumerable<string[]> groupNames)
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
                    var newNames = engine.Build<string>([indiv], globalVariables).ToList();
                    names.AddRange(newNames);
                }

            }
            return names;
        }


        public void OnTestGenerated(object sender, ProgressEventArgs e) => InvokeProgress(e);
        public event EventHandler<ProgressEventArgs>? Progressed;
        public void InvokeProgress(ProgressEventArgs e) => Progressed?.Invoke(this, e);
    }
}
