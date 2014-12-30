using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NBi.Service
{
    public class TemplateManager
    {
        public string Code { get; set; }

        public IEnumerable<string> PredefinedLabels { get; set; }

        public TemplateManager()
        {

        }
    }
}
