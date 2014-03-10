using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface
{
    interface ITemplateView : IView
    {
        string TemplateValue { get; set; }
    }
}
