using System;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace NBi.UI.Genbi.Interface
{
    interface ITestCasesView : IView
    {
        DataTable TestCases { get; set; }
        BindingList<string> Variables { get; set; }

        int VariableSelectedIndex { get; }
    }
}
