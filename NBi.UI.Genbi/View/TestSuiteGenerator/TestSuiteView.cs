using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NBi.Service;
using NBi.Service.Dto;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Interface;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.Stateful;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    partial class TestSuiteView : Form, ITestCasesView, ITemplateView, ISettingsView, ITestsGenerationView, ITestSuiteView
    {

        private TestSuiteState State { get; set; }
        private TestCasesPresenter TestCasesPresenter {get; set;}
        private TemplatePresenter TemplatePresenter { get; set; }
        private SettingsPresenter SettingsPresenter { get; set; }
        private TestListPresenter TestListPresenter { get; set; }
        private TestSuitePresenter TestSuitePresenter { get; set; } 


        public TestSuiteView()
        {
            State = new TestSuiteState();
            TestCasesPresenter = new TestCasesPresenter(this, new RenameVariableWindow(), new TestCasesManager(), State.TestCases, State.Variables);
            TemplatePresenter = new TemplatePresenter(this, new TemplateManager(), State.Template);
            SettingsPresenter = new SettingsPresenter(this, new SettingsManager(), State.Settings);
            TestListPresenter = new TestListPresenter(this, new TestListManager(), State.Tests, State.TestCases, State.Variables, State.Template);
            TestSuitePresenter = new TestSuitePresenter(this, new TestSuiteManager(), State.Tests, State.Settings);

            InitializeComponent();
            DeclareBindings();            BindPresenter();
        }

        protected void DeclareBindings()
        {
            testCasesControl.DataBind(TestCasesPresenter);
            settingsControl.DataBind(SettingsPresenter);
            templateControl.DataBind(TemplatePresenter);
            testListControl.DataBind(TestListPresenter);

            TemplatePresenter.PropertyChanged += (sender, e) => TestListPresenter.Template = TemplatePresenter.Template;
            TestListPresenter.PropertyChanged += (sender, e) => TestSuitePresenter.RefreshCommands();

            TestSuitePresenter.TestSuiteLoaded += (sender, e) =>
            {
                TestListPresenter.ReloadTests();
            };
        }


        private void BindPresenter()
        {
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.OpenTestCasesCommand, openTestCasesToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.OpenTestCasesCommand, openTestCasesToolStripButton);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.RemoveVariableCommand, testCasesControl.RemoveCommand);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.RenameVariableCommand, testCasesControl.RenameCommand);

            CommandManager.Instance.Bindings.Add(this.TemplatePresenter.OpenTemplateCommand, openTemplateToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TemplatePresenter.OpenTemplateCommand, openTemplateToolStripButton);
            CommandManager.Instance.Bindings.Add(this.TemplatePresenter.SaveTemplateCommand, saveAsTemplateToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TemplatePresenter.SaveTemplateCommand, saveAsTemplateToolStripButton);

            CommandManager.Instance.Bindings.Add(this.SettingsPresenter.AddReferenceCommand, settingsControl.AddCommand);
            CommandManager.Instance.Bindings.Add(this.SettingsPresenter.RemoveReferenceCommand, settingsControl.RemoveCommand);

            //CommandManager.Instance.Bindings.Add(this.TestListPresenter.OpenTestSuiteCommand, openTestSuiteToolStripMenuItem);
            //CommandManager.Instance.Bindings.Add(this.TestListPresenter.OpenTestSuiteCommand, openTestSuiteToolStripButton);


            CommandManager.Instance.Bindings.Add(this.TestListPresenter.GenerateTestsXmlCommand, generateTestsToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.GenerateTestsXmlCommand, generateTestsToolStripButton);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.ClearTestsXmlCommand, clearTestsToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.ClearTestsXmlCommand, clearTestsToolStripButton);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.UndoGenerateTestsXmlCommand, undoGenerateTestsToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.UndoGenerateTestsXmlCommand, undoGenerateTestsToolStripButton);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.DeleteTestCommand, testListControl.DeleteCommand);

            CommandManager.Instance.Bindings.Add(this.TestSuitePresenter.OpenTestSuiteCommand, openTestSuiteToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestSuitePresenter.OpenTestSuiteCommand, openTestSuiteToolStripButton);
            CommandManager.Instance.Bindings.Add(this.TestSuitePresenter.SaveAsTestSuiteCommand, saveAsTestSuiteToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestSuitePresenter.SaveAsTestSuiteCommand, saveAsTestSuiteToolStripButton);
        }

        private void UnbindPresenter()
        {
            //CommandManager.Instance.Bindings.Remove(this.Presenter.RemoveVariableCommand, apply);
        }


        #region properties

        public DataTable TestCases
        {
            get
            {
                return (DataTable)(testCasesControl.bindingCsv.DataSource);
            }
            set
            {
                testCasesControl.bindingCsv.DataSource = value;
            }
        }

        public bool UseGrouping
        {
            get
            {
                return false;//templateControl.UseGrouping;
            }
            set
            {
                //templateControl.UseGrouping = value;
            }
        }


        public BindingList<Test> Tests
        {
            get
            {
                return null;
            }
            set
            {
                //testListControl.Tests = value;
            }
        }

        public string TemplateValue
        {
            get
            {
                return null;//templateControl.Template;
            }
            set
            {
                //templateControl.Template = value;
            }
        }


        public string SettingsValue
        {
            get
            {
                return null; //settingsControl.Value;
            }
            set
            {
                //settingsControl.Value = value;
            }
        }


        public BindingList<string> SettingsNames
        {
            get
            {
                return null; //settingsControl.Names;
            }
            set
            {
                //settingsControl.Names = value;
            }
        }

        public int SettingsSelectedIndex
        {
            get
            {
                return 0;
            }
            set
            {
                
            }
        }

        public Test TestSelection
        {
            get
            {
                return null; //testListControl.TestSelection;
            }
            set
            {
                //testListControl.TestSelection = value;
            }
        }

        public int TestSelectedIndex
        {
            get
            {
                return 0;// testListControl.TestSelectedIndex;
            }
            set
            {
                //testListControl.TestSelectedIndex = value;
            }
        }

        //public int ProgressValue
        //{
        //    set
        //    {
        //        testListControl.ProgressValue = value;
        //    }
        //}

        #endregion


        private void CsvImporterView_Load(object sender, EventArgs e)
        {

        }


        private void GenerateProjectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Dispatcher.StartRunnerConfig();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var window = new AboutBox();
            window.ShowDialog(this);
        }








        public BindingList<string> Variables
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        BindingList<Test> ITestSuiteView.Tests
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        Test ITestSuiteView.TestSelection
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        int ITestSuiteView.TestSelectedIndex
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler IView.Initialize
        {
            add { }
            remove { throw new NotImplementedException(); }
        }


        public int VariableSelectedIndex
        {
            get { throw new NotImplementedException(); }
        }
    }
}
