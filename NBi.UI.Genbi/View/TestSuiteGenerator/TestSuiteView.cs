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
using NBi.GenbiL.Stateful;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    partial class TestSuiteView : Form
    {

        private GenerationState State { get; set; }
        private TestCasesPresenter TestCasesPresenter {get; set;}
        private TemplatePresenter TemplatePresenter { get; set; }
        private SettingsPresenter SettingsPresenter { get; set; }
        private TestListPresenter TestListPresenter { get; set; }
        private TestSuitePresenter TestSuitePresenter { get; set; }
        public MacroPresenter MacroPresenter { get; private set; }


        public TestSuiteView()
        {
            State = new GenerationState();
            TestCasesPresenter = new TestCasesPresenter(new RenameVariableWindow(), new FilterWindow(), new ConnectionStringWindow(), State);
            TemplatePresenter = new TemplatePresenter(State);
            SettingsPresenter = new SettingsPresenter(State);
            TestListPresenter = new TestListPresenter(State);
            TestSuitePresenter = new TestSuitePresenter(State);
            MacroPresenter = new MacroPresenter();

            InitializeComponent();
            DeclareBindings();            
            BindPresenter();
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
                    SettingsPresenter.Refresh();
                    //TODO TestListPresenter.Refresh();
                };
        }


        private void BindPresenter()
        {
            //TestCases & Variables
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.OpenTestCasesCommand, openTestCasesToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.OpenTestCasesQueryCommand, openTestCasesQueryToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.OpenTestCasesCommand, openTestCasesToolStripButton);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.RemoveVariableCommand, testCasesControl.RemoveCommand);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.RenameVariableCommand, testCasesControl.RenameCommand);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.MoveLeftVariableCommand, testCasesControl.MoveLeftCommand);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.MoveRightVariableCommand, testCasesControl.MoveRightCommand);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.FilterCommand, testCasesControl.FilterCommand);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.FilterDistinctCommand, testCasesControl.FilterDistinctCommand);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.AddConnectionStringCommand, testCasesControl.AddConnectionStringCommand);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.RemoveConnectionStringCommand, testCasesControl.RemoveConnectionStringCommand);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.EditConnectionStringCommand, testCasesControl.EditConnectionStringCommand);
            CommandManager.Instance.Bindings.Add(this.TestCasesPresenter.RunQueryCommand, testCasesControl.RunQueryCommand);

            //Template
            CommandManager.Instance.Bindings.Add(this.TemplatePresenter.OpenTemplateCommand, openTemplateToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TemplatePresenter.OpenTemplateCommand, openTemplateToolStripButton);
            CommandManager.Instance.Bindings.Add(this.TemplatePresenter.SaveTemplateCommand, saveAsTemplateToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TemplatePresenter.SaveTemplateCommand, saveAsTemplateToolStripButton);

            //Settings
            CommandManager.Instance.Bindings.Add(this.SettingsPresenter.AddReferenceCommand, settingsControl.AddCommand);
            CommandManager.Instance.Bindings.Add(this.SettingsPresenter.RemoveReferenceCommand, settingsControl.RemoveCommand);

            //Tests
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.GenerateTestsXmlCommand, generateTestsToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.GenerateTestsXmlCommand, generateTestsToolStripButton);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.ClearTestsXmlCommand, clearTestsToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.ClearTestsXmlCommand, clearTestsToolStripButton);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.UndoGenerateTestsXmlCommand, undoGenerateTestsToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.UndoGenerateTestsXmlCommand, undoGenerateTestsToolStripButton);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.DeleteTestCommand, testListControl.DeleteCommand);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.DisplayTestCommand, testListControl.DisplayCommand);
            CommandManager.Instance.Bindings.Add(this.TestListPresenter.AddCategoryCommand, testListControl.AddCategoryCommand);

            //Test-suite
            CommandManager.Instance.Bindings.Add(this.TestSuitePresenter.OpenTestSuiteCommand, openTestSuiteToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestSuitePresenter.OpenTestSuiteCommand, openTestSuiteToolStripButton);
            CommandManager.Instance.Bindings.Add(this.TestSuitePresenter.SaveAsTestSuiteCommand, saveAsTestSuiteToolStripMenuItem);
            CommandManager.Instance.Bindings.Add(this.TestSuitePresenter.SaveAsTestSuiteCommand, saveAsTestSuiteToolStripButton);

            CommandManager.Instance.Bindings.Add(this.MacroPresenter.PlayMacroCommand, playMacroToolStripMenuItem);
        }

        private void UnbindPresenter()
        {
            //CommandManager.Instance.Bindings.Remove(this.Presenter.RemoveVariableCommand, apply);
        }


        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var window = new AboutBox();
            window.ShowDialog(this);
        }

        private void generateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var view = Bootstrapper.GetRunnerConfigView();
            view.ShowDialog(this);
        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }








        
    }
}
