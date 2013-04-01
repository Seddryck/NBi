using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Core;
using NBi.Service;
using NBi.UI.Interface;
using NBi.Xml;

namespace NBi.UI.Presenter.GenericTest
{
    public class CsvImportPresenter: BasePresenter<ICsvImporterView>
    {
        private const string TEMPLATE_DIRECTORY = "NBi.UI.Templates.";

        private readonly List<TestXml> lastGeneration;

        public CsvImportPresenter(ICsvImporterView view)
            : base(view)
        {
            lastGeneration = new List<TestXml>();
        }

        
        protected override void OnViewInitialize(object sender, EventArgs e)
        {
            base.OnViewInitialize(sender, e);
            Initialize();
        
            View.VariableRenamed += OnViewVariableRenamed;
            View.GenerateTests += OnViewGenerateTests;
            View.UndoGenerateTests += OnViewUndoGenerateTests;
            View.NewCsvSelected += OnViewNewCsvSelected;
            View.NewTemplateSelected += OnViewNewTemplateSelected;
            View.PersistTestSuite += OnViewPersistTestSuite;
            View.NewTestSelected += OnViewTestSelected;
        }

        protected void OnViewGenerateTests(object sender, EventArgs e)
        {
            int groupedColumn = View.CsvContent.Rows[0].ItemArray.Length-1;

            var table = new List<List<List<object>>>();
            for (int i = 0; i < View.CsvContent.Rows.Count; i++)
            {
                var isIdentical = (i != 0) && View.UseGrouping;
                var grouping = View.CsvContent.Rows[i].ItemArray.ToList();
                grouping.RemoveAt(groupedColumn);
                var k = 0;
                while (k < grouping.Count && isIdentical)
                {
                    if (grouping[k].ToString() != table[table.Count - 1][k][0].ToString())
                        isIdentical = false;
                    k++;
                }


                if (!isIdentical)
                {
                    table.Add(new List<List<object>>());
                    for (int j = 0; j < View.CsvContent.Rows[i].ItemArray.Length; j++)
                    {
                        table[table.Count - 1].Add(new List<object>());
                        table[table.Count - 1][j].Add(View.CsvContent.Rows[i].ItemArray[j].ToString());
                    }
                }
                else
                    table[table.Count - 1][groupedColumn].Add(View.CsvContent.Rows[i].ItemArray[groupedColumn].ToString());
            }
                

            var genericTestMaker = new StringTemplateEngine(View.Template, View.Variables.ToArray());
            try
            {
                var tests = genericTestMaker.Build(table);
                lastGeneration.Clear();
                foreach (var test in tests)
                {
                    View.Tests.Add(test);
                    lastGeneration.Add(test);
                }

            }
            catch (ExpectedVariableNotFoundException)
            {
                View.ShowException("The template has at least one variable which wasn't supplied by the Csv. Check the name of the variables.");
            }
        }

        protected void OnViewUndoGenerateTests(object sender, EventArgs e)
        {
            foreach (var test in lastGeneration)
            {
                View.Tests.Remove(test);
            }
        }

        protected void OnViewPersistTestSuite(object sender, PersistTestSuiteEventArgs e)
        {
            var testSuite = new TestSuiteXml();
            var array = View.Tests.ToArray();
            testSuite.Load(array);

            var manager = new XmlManager();
            manager.Persist(e.FileName, testSuite);
            View.ShowInform(String.Format("Test-suite '{0}' persisted.", e.FileName));
        }

        public void OnViewVariableRenamed(object sender, VariableRenamedEventArgs e)
        {
            View.CsvContent.Columns[e.Index].ColumnName = e.NewName;
            View.Variables[e.Index] = e.NewName;
        }

        public void OnViewNewCsvSelected(object sender, NewCsvSelectedEventArgs e)
        {
            //Content of Csv
            var csvReader = new CsvReader(e.FullPath, true);
            View.CsvContent = csvReader.Read();
            //Variables
            View.Variables.Clear();
            foreach (DataColumn col in View.CsvContent.Columns)
                View.Variables.Add(col.ColumnName);
        }

        public void OnViewNewTemplateSelected(object sender, NewTemplateSelectedEventArgs e)
        {
            var tpl = string.Empty;
            //Template
            switch (e.Template)
            {
                case NewTemplateSelectedEventArgs.TemplateType.Embedded:
                    tpl = ReadEmbeddedTemplate(e.ResourceName);
                    break;
                case NewTemplateSelectedEventArgs.TemplateType.External:
                    tpl = ReadExternalTemplate(e.ResourceName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            View.Template = tpl;
        }

        public void OnViewTestSelected(object sender, SelectedTestEventArgs e)
        {
            if (e.Index != -1)
                View.TestSelected = View.Tests[e.Index];
            else
                View.TestSelected = null;
        }
  
        private string ReadEmbeddedTemplate(string resourceName)
        {
            var tpl = string.Empty;           //Template
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("NBi.UI.Templates.{0}.txt", resourceName)))
            {
                using (var reader = new StreamReader(stream))
                {
                    tpl = reader.ReadToEnd();
                }
            }
            return tpl;
        }

        private string ReadExternalTemplate(string resourceName)
        {
            var tpl = string.Empty;           //Template
            using (var stream = new StreamReader(resourceName))
            {
                tpl = stream.ReadToEnd();
            }
            return tpl;
        }

        protected void Initialize()
        {
            View.Variables = new BindingList<string>();
            View.EmbeddedTemplates = new BindingList<string>();
            View.Tests = new BindingList<TestXml>();

            //var csvReader = new CsvReader(@"C:\mydims.csv", true);
            //View.CsvContent = csvReader.Read();
            View.CsvContent = new DataTable();
            foreach (DataColumn col in View.CsvContent.Columns)
                View.Variables.Add(col.ColumnName);
            //Template
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NBi.UI.Templates.ExistsDimension.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    View.Template = reader.ReadToEnd();
                }
            }
            //EmbeddedResources
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            var templates = resources.Where(t => t.StartsWith(TEMPLATE_DIRECTORY) && t.EndsWith(".txt"));
            templates = templates.Select(t => t.Replace(TEMPLATE_DIRECTORY, ""));
            templates = templates.Select(t => t.Substring(0, t.Length-4));
            View.EmbeddedTemplates = new BindingList<string>(templates.ToArray());
        }
    }
}
