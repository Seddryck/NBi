using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using NBi.UI.Genbi.Interface.Generator.Events;
using NBi.Xml;

namespace NBi.UI.Genbi.Interface.Generator
{
    public interface ICsvGeneratorView : IView
    {
        DataTable CsvContent { get; set; }
        bool UseGrouping { get; set; }
        BindingList<string> Variables { get;  set; }
        BindingList<TestXml> Tests { get; set; }
        BindingList<string> EmbeddedTemplates { get; set; }
        string Template { get; set; }
        TestXml TestSelected { get; set; }
        bool CanUndo { set; }
        bool CanGenerate { set; }
        bool CanClear { set; }
        bool CanSaveAs { set; }
        bool CanSaveTemplate { set; }

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
        //Create a serie of tests based on template and CSV
        event EventHandler TestsGenerate;
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
