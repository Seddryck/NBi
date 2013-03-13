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

        public CsvImportPresenter(ICsvImporterView view)
            : base(view)
        {
            
            
        }

        
        protected override void OnViewInitialize(object sender, EventArgs e)
        {
            base.OnViewInitialize(sender, e);
            Initialize();
        
            View.VariableRenamed += OnViewVariableRenamed;
            View.GenerateTests += OnViewGenerateTests;
            View.NewCsvSelected += OnViewNewCsvSelected;
            View.NewTemplateSelected += OnViewNewTemplateSelected;
            View.PersistTestSuite += OnViewPersistTestSuite;
        }

        protected void OnViewGenerateTests(object sender, EventArgs e)
        {
            var rows = new List<List<string>>();
            foreach (DataRow row in View.CsvContent.Rows)
                rows.Add(row.ItemArray.Cast<string>().ToList());

            var genericTestMaker = new GenericTestMaker(View.Template, View.Variables.ToArray());
            try
            {
                var tests = genericTestMaker.Build(rows);
                foreach (var test in tests)
                    View.Tests.Add(test);
            }
            catch (ExpectedVariableNotFoundException)
            {
                View.ShowException("The template has at least one variable which wasn't supplied by the Csv. Check the name of the variables.");
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
            //Template
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("NBi.UI.Templates.{0}.txt",e.ResourceName)))
            {
                using (var reader = new StreamReader(stream))
                {
                    View.Template = reader.ReadToEnd();
                }
            }
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
