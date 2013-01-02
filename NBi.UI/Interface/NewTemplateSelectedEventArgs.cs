using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Interface
{
    public class NewTemplateSelectedEventArgs : EventArgs
    {
        public string ResourceName { get; private set; }
        public NewTemplateSelectedEventArgs(string resourceName)
        {
            ResourceName = resourceName;
        }
    }
}
