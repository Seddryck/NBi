using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface.Generator.Events
{
    public class TemplateSelectEventArgs : EventArgs
    {
        public TemplateType Template;
        public string ResourceName { get; private set; }
        public TemplateSelectEventArgs(TemplateType template, string resourceName)
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
