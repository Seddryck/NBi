using System;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace NBi.UI.Genbi.Interface
{
    interface ITestsGenerationView : IView
    {
        DataTable TestCases { get; set; }
        string TemplateValue { get; }
        BindingList<string> Variables { get; set; }
        bool UseGrouping { get; set; }

        BindingList<Service.Dto.Test> Tests { get; set; }

        //int ProgressValue { set; }

        
    }
}
