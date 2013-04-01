using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using NBi.Xml;

namespace NBi.UI.Interface
{
    public interface ICsvImporterView : IView
    {
        DataTable CsvContent { get; set; }
        bool UseGrouping { get; set; }
        BindingList<string> Variables { get;  set; }
        BindingList<TestXml> Tests { get; set; }
        BindingList<string> EmbeddedTemplates { get; set; }
        string Template { get; set; }
        TestXml TestSelected { get; set; }

        //A new csv file is selected to be displayed in the screen
        event EventHandler<NewCsvSelectedEventArgs> NewCsvSelected;
        //A new template resource is selected to be displayed in the screen
        event EventHandler<NewTemplateSelectedEventArgs> NewTemplateSelected;
        //A variable is renamed
        event EventHandler<VariableRenamedEventArgs> VariableRenamed;
        //Create a serie of tests based on template and CSV
        event EventHandler GenerateTests;
        //Persist the testsuite created
        event EventHandler<PersistTestSuiteEventArgs> PersistTestSuite;
        //Select a new Test
        event EventHandler<SelectedTestEventArgs> NewTestSelected;
        //Undo las generation
        event EventHandler UndoGenerateTests;
    }
}
