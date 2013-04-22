using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using NBi.Service;
using NBi.Service.Dto;
using NBi.UI.Genbi.Interface.Generator;
using NBi.UI.Genbi.Interface.Generator.Events;

namespace NBi.UI.Genbi.Presenter.Generator
{
    public class CsvGeneratorPresenter: BasePresenter<ICsvGeneratorView>
    {
        private readonly TestManager testManager;
        private readonly SettingsManager settingsManager;
        private readonly TestSuiteManager testSuiteManager;

        public CsvGeneratorPresenter(ICsvGeneratorView view)
            : base(view)
        {
            testManager = new TestManager();
            settingsManager = new SettingsManager();
            testSuiteManager = new TestSuiteManager();
        }

        
        protected override void OnViewInitialize(object sender, EventArgs e)
        {
            base.OnViewInitialize(sender, e);
            Initialize();
        
            View.VariableRename += OnRenameVariable;
            View.VariableRemove += OnRemoveVariable;
            View.TestsGenerate += OnTestsGenerate;
            View.TestsUndoGenerate += OnTestsUndoGenerate;
            View.CsvSelect += OnCsvSelect;
            View.TemplateSelect += OnTemplateSelect;
            View.TemplatePersist += OnTemplatePersist;
            View.TemplateUpdate += OnTemplateUpdate;
            View.TestSuitePersist += OnTestSuitePersist;
            View.TestSelect += OnTestSelect;
            View.TestDelete += OnTestDelete;
            View.TestsClear += OnTestsClear;
            View.SettingsSelect += OnSettingsSelect;
            View.SettingsUpdate += OnSettingsUpdate;
        }

        private void OnTemplatePersist(object sender, TemplatePersistEventArgs e)
        {
            var manager = new TemplateManager();
            manager.Persist(e.FileName, View.Template);
            View.ShowInform(String.Format("Template '{0}' persisted.", e.FileName));
        }

        protected void OnTestsGenerate(object sender, EventArgs e)
        {
            
            try
            {
                testManager.Build(View.Template, View.Variables.ToArray(), View.CsvContent, View.UseGrouping);
                var tests = testManager.GetTests();
                View.Tests = new BindingList<Test>(tests.ToArray());
            }
            catch (ExpectedVariableNotFoundException)
            {
                View.ShowException("The template has at least one variable which wasn't supplied by the Csv. Check the name of the variables.");
            }
            catch (TemplateExecutionException ex)
            {
                View.ShowException(ex.Message);
            }
            finally
            {
                CalculateValidAction();
            }
        }

        protected void OnTestsUndoGenerate(object sender, EventArgs e)
        {
            testManager.Undo();
            var tests = testManager.GetTests();
            View.Tests = new BindingList<Test>(tests.ToArray());
            CalculateValidAction();
        }

        protected void OnTestSuitePersist(object sender, TestSuitePersistEventArgs e)
        {
            testSuiteManager.DefineSettings(settingsManager);
            testSuiteManager.DefineTests(testManager);
            testSuiteManager.SaveAs(e.FileName);
            View.ShowInform(String.Format("Test-suite '{0}' persisted.", e.FileName));
        }

        public void OnRenameVariable(object sender, VariableRenameEventArgs e)
        {
            View.CsvContent.Columns[e.Index].ColumnName = e.NewName;
            View.Variables[e.Index] = e.NewName;
        }

        public void OnRemoveVariable(object sender, VariableRemoveEventArgs e)
        {
            View.CsvContent.Columns.RemoveAt(e.Index);
            View.Variables.RemoveAt(e.Index);
            CalculateValidAction();
        }

        public void OnSettingsSelect(object sender, SettingsSelectEventArgs e)
        {
            View.SettingsValue = settingsManager.GetValue(e.Name);
        }

        public void OnSettingsUpdate(object sender, SettingsUpdateEventArgs e)
        {
            settingsManager.SetValue(e.Name, e.Value);
            CalculateValidAction();
        }

        public void OnCsvSelect(object sender, CsvSelectEventArgs e)
        {
            //Content of Csv
            var csvManager = new CsvManager(e.FullPath);
            View.CsvContent = csvManager.Content;
            //Variables
            View.Variables= new BindingList<string>(csvManager.ColumnHeaders);
            //Re-calculate the actions available
            CalculateValidAction();
        }

        public void OnTemplateSelect(object sender, TemplateSelectEventArgs e)
        {
            var tpl = string.Empty;
            var templateManager = new TemplateManager();
            //Template
            switch (e.Template)
            {
                case TemplateSelectEventArgs.TemplateType.Embedded:
                    tpl = templateManager.GetEmbeddedTemplate(e.ResourceName);
                    break;
                case TemplateSelectEventArgs.TemplateType.External:
                    tpl = templateManager.GetExternalTemplate(e.ResourceName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            View.Template = tpl;
            CalculateValidAction();
        }

        public void OnTemplateUpdate(object sender, EventArgs e)
        {
            CalculateValidAction();
        }

        public void OnTestSelect(object sender, TestSelectEventArgs e)
        {
            if (e.Index != -1)
                View.TestSelected = View.Tests[e.Index];
            else
                View.TestSelected = null;
        }

        public void OnTestDelete(object sender, EventArgs e)
        {
            if (View.TestSelected != null)
            {
                testManager.RemoveAt(View.TestSelectedIndex);
                var tests = testManager.GetTests();
                View.Tests = new BindingList<Test>(tests.ToArray());
                View.TestSelected = null;
            }
            else
                View.ShowInform(String.Format("No test to delete."));
        }

        public void OnTestsClear(object sender, EventArgs e)
        {
            testManager.Clear();
            var tests = testManager.GetTests();
            View.Tests = new BindingList<Test>(tests.ToArray());
            View.TestSelected = null;
            View.ShowInform(String.Format("Generated test-suite has been cleared."));
            CalculateValidAction();
        }

        private void CalculateValidAction()
        {
            View.CanGenerate = View.Template.Length > 0 && View.CsvContent.Rows.Count > 0;
            View.CanUndo = testManager.CanUndo;
            View.CanClear = View.Tests.Count != 0;
            View.CanSaveAs = View.Tests.Count != 0;
            View.CanSaveTemplate = View.Template.Length > 0;
            View.CanRename = View.Variables.Count > 0;
            View.CanRemove = View.Variables.Count > 1;
            //View.CanSelectSettings = View.SettingsNames.Count > 0;
            //View.CanEditSettings = View.SettingsNameSelected != string.Empty;
        }
  
        protected void Initialize()
        {
            View.Variables = new BindingList<string>();
            View.EmbeddedTemplates = new BindingList<string>();
            View.Tests = new BindingList<Test>();

            //CsvContent
            View.CsvContent = new DataTable();
            foreach (DataColumn col in View.CsvContent.Columns)
                View.Variables.Add(col.ColumnName);
            
            //Template
            var templateManager = new TemplateManager();
            View.Template = templateManager.GetDefaultContent();
            View.EmbeddedTemplates = new BindingList<string>(templateManager.GetEmbeddedLabels());

            //Settings
            View.SettingsNames = new BindingList<string>(settingsManager.GetNames());

            CalculateValidAction();
        }

    }
}
