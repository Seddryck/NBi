using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Service.Dto;
using NBi.Xml;
using NBi.Xml.Settings;

namespace NBi.Service
{
    public class TestListManager
    {

        private IList<TestXml> tests;
        private IList<TestXml> lastGeneration;

        internal IList<TestXml> Tests 
        { 
            get 
            {
                return tests;
            }
            set 
            {
                tests=value;
            }
        }
        
        
        public TestListManager()
        {
            tests = new List<TestXml>();
            lastGeneration = new List<TestXml>();
        }


        public void Build(string template, string[] variables, DataTable dataTable, bool useGrouping)
        {
            var generator = new StringTemplateEngine(template, variables);
            var cases = GetCases(dataTable, useGrouping);
            generator.Progressed += new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
            lastGeneration = generator.Build(cases).ToList();
            generator.Progressed -= new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
            tests = tests.Concat(lastGeneration).ToList();
        }

        public  void OnTestGenerated(object sender, ProgressEventArgs e)
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

        public bool CanUndo
        {
            get { return lastGeneration.Count != 0; }
        }

        public void Undo()
        {
            foreach (var test in lastGeneration)
            {
                tests.Remove(test);
            }
            lastGeneration.Clear();
        }

        public IList<Test> GetTests()
        {
            var value = new List<Test>();
            foreach (var test in tests)
            {
                var t = new Test();
                t.Content = test.Content;
                t.Title = test.Name;
                t.Reference = test;
                value.Add(t);
            }
            return value;
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

        public void Clear()
        {
            tests.Clear();
            lastGeneration.Clear();
        }

        public void RemoveAt(int index)
        {
            tests.RemoveAt(index);
        }

        public void Remove(Test test)
        {
            tests.Remove(test.Reference);
        }


        public void SetTests(IEnumerable<Test> tests)
        {
            Tests.Clear();
            foreach (var testDto in tests)
                Tests.Add(testDto.Reference);
        }
    }
}
