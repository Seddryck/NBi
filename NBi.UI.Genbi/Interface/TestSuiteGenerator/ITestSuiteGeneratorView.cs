using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NBi.Service.Dto;
using NBi.UI.Genbi.Interface.TestSuiteGenerator.Events;

namespace NBi.UI.Genbi.Interface.TestSuiteGenerator
{
    public interface ITestSuiteGeneratorView : IView
    {
        Form MainForm { get; }
        DataTable CsvContent { get; set; }
        bool UseGrouping { get; set; }
        BindingList<string> Variables { get;  set; }
        int SelectedVariableIndex { get; }
        BindingList<Test> Tests { get; set; }
        BindingList<string> EmbeddedTemplates { get; set; }
        string Template { get; set; }
        Test TestSelected { get; set; }
        int TestSelectedIndex { get; set; }
        BindingList<string> SettingsNames { get; set; }
        //string SettingsNameSelected { get; set; }
        string SettingsValue { get; set; }
        bool CanUndo { set; }
        bool CanGenerate { set; }
        bool CanClear { set; }
        bool CanSaveAs { set; }
        bool CanSaveTemplate { set; }
        bool CanRename { set; }
        bool CanRemove { set; }
        int ProgressValue { set; }

        //A new csv file is selected to be displayed in the screen
        event EventHandler<CsvSelectEventArgs> CsvSelect;
        //A new template resource is selected to be displayed in the screen
        event EventHandler<TemplateSelectEventArgs> TemplateSelect;
        //A new template resource is updated
        event EventHandler TemplateUpdate;
        //Persist the template used
        event EventHandler<TemplatePersistEventArgs> TemplatePersist;
        //A variable is renamed
        event EventHandler<VariableRenameEventArgs> VariableRename;
        //A variable is removed
        event EventHandler<VariableRemoveEventArgs> VariableRemove;
        //A variable is renamed
        event EventHandler<SettingsSelectEventArgs> SettingsSelect;
        //A variable is removed
        event EventHandler<SettingsUpdateEventArgs> SettingsUpdate;
        //Create a serie of tests based on template and CSV
        event EventHandler TestsGenerate;
        //Open existing testsuite 
        event EventHandler<TestSuiteSelectEventArgs> TestSuiteSelect;
        //Persist the testsuite created
        event EventHandler<TestSuitePersistEventArgs> TestSuitePersist;
        //Select a test
        event EventHandler<TestSelectEventArgs> TestSelect;
        //Delete a test
        event EventHandler TestDelete;
        //Undo last generation
        event EventHandler TestsUndoGenerate;
        //Clear all the tests generated
        event EventHandler TestsClear;











        
    }
}
