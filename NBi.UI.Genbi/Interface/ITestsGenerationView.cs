using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using NBi.GenbiL.Stateful;
using NBi.IO.Genbi.Dto;

namespace NBi.UI.Genbi.Interface
{
    interface ITestsGenerationView : IView
    {
        DataTable TestCases { get; set; }
        string TemplateValue { get; }
        BindingList<string> Variables { get; set; }
        bool UseGrouping { get; set; }

        BindingList<Test> Tests { get; set; }

        //int ProgressValue { set; }
    }
}
