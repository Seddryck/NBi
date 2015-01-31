using NBi.GenbiL.Stateful;
using NBi.Service;
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

        public void Execute(GenerationState state)
        {
            var generator = new StringTemplateEngine(state.Template.Code, state.TestCaseSetCollection.Scope.Variables.ToArray());
            var cases = GetCases(state.TestCaseSetCollection.Scope.Content, Grouping);
            generator.Progressed += new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
            var newStandaloneTests = generator.Build(cases).ToList();
            generator.Progressed -= new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
            state.Suite.Tests.AddRange(newStandaloneTests);
        }

        protected List<List<List<object>>> GetCases(DataTable dataTable, bool useGrouping)
        {
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
                        variableTests[variableTests.Count - 1][j].Add(dataTable.Rows[i].ItemArray[j].ToString());
                    }
                }
                else
                    variableTests[variableTests.Count - 1][groupedColumn].Add(dataTable.Rows[i].ItemArray[groupedColumn].ToString());
            }

            return variableTests;
        }

        public void OnTestGenerated(object sender, ProgressEventArgs e)
        {
            InvokeProgress(e);
        }

        public event EventHandler<ProgressEventArgs> Progressed;
        public void InvokeProgress(ProgressEventArgs e)
        {
            var handler = Progressed;
            if (handler != null)
                handler(this, e);
        }

        public string Display
        {
            get
            {
                return string.Format("Generating Tests {0} grouping option"
                    , Grouping ? "with" : "without"
                    );
            }
        }
    }
}
