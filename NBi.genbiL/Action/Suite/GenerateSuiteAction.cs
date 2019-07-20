using NBi.GenbiL.Stateful;
using NBi.GenbiL.Stateful.Tree;
using NBi.GenbiL.Templating;
using NBi.Xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.GenbiL.Action.Suite
{
    public class GenerateSuiteAction : ISuiteAction
    {
        public bool Grouping { get; set; }

        public GenerateSuiteAction(bool grouping)
        {
            Grouping = grouping;
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
            lastGeneration.ToList().ForEach(x => state.Suite.AddChild(new TestNode(x)));
        }

        public string Display => $"Generating Tests {(Grouping ? "with" : "without")} grouping option";

        protected List<List<List<object>>> GetCases(DataTable dataTable, bool useGrouping)
        {
            if (dataTable.Rows.Count == 0)
                return new List<List<List<object>>>();

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
                    if (grouping[k].ToString() != variableTests[variableTests.Count - 1][k][0].ToString())
                        isIdentical = false;
                    k++;
                }


                if (!isIdentical)
                {
                    variableTests.Add(new List<List<object>>());
                    for (int j = 0; j < dataTable.Rows[i].ItemArray.Length; j++)
                    {
                        variableTests[variableTests.Count - 1].Add(new List<object>());
                        if (dataTable.Rows[i].ItemArray[j] is IEnumerable<string>)
                        {
                            foreach (var item in (IEnumerable<string>)dataTable.Rows[i].ItemArray[j])
                                variableTests[variableTests.Count - 1][j].Add(item);
                        }
                        else
                            variableTests[variableTests.Count - 1][j].Add(dataTable.Rows[i].ItemArray[j].ToString());
                    }
                }
                else
                    variableTests[variableTests.Count - 1][groupedColumn].Add(dataTable.Rows[i].ItemArray[groupedColumn].ToString());
            }

            return variableTests;
        }

        private IEnumerable<TestStandaloneXml> Build(string template, string[] variables, DataTable dataTable, bool useGrouping, IDictionary<string, object> globalVariables)
        {
            var generator = new StringTemplateEngine(template, variables);
            var cases = GetCases(dataTable, useGrouping);
            generator.Progressed += new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
            var lastGeneration = generator.Build<TestStandaloneXml>(cases, globalVariables).ToList();
            generator.Progressed -= new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
            return lastGeneration;
        }

        protected IEnumerable<TestStandaloneXml> Build(IEnumerable<string> templates, string[] variables, DataTable dataTable, bool useGrouping, IDictionary<string, object> globalVariables)
        {
            if (templates.Count() == 0)
                throw new ArgumentException("No template was specified. You must at least define a template before generating a test suite.");

            if (templates.Count() == 1)
                return Build(templates.ElementAt(0), variables, dataTable, useGrouping, globalVariables);
            else
            {
                var lastGeneration = new List<TestStandaloneXml>();
                var cases = GetCases(dataTable, useGrouping);
                foreach (var indiv in cases)
                {
                    foreach (var template in templates)
                    {
                        var engine = new StringTemplateEngine(template, variables);
                        //engine.Progressed += new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
                        lastGeneration.AddRange(engine.Build<TestStandaloneXml>(new List<List<List<object>>>() { indiv }, globalVariables).ToList());
                        //engine.Progressed -= new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
                    }
                }
                return lastGeneration;
            }
        }

        public void OnTestGenerated(object sender, ProgressEventArgs e) => InvokeProgress(e);
        public event EventHandler<ProgressEventArgs> Progressed;
        public void InvokeProgress(ProgressEventArgs e) => Progressed?.Invoke(this, e);
    }
}
