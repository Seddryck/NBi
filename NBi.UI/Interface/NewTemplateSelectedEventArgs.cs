using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Interface
{
    public class NewTemplateSelectedEventArgs : EventArgs
    {
        public TemplateType Template;
        public string ResourceName { get; private set; }
        public NewTemplateSelectedEventArgs(TemplateType template, string resourceName)
        {
            Template = template;
            ResourceName = resourceName;
        }

        public enum TemplateType
        {
            Embedded,
            External
        }
    }
}
