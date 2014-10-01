using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface.TestSuiteGenerator.Events
{
    public class TemplatePersistEventArgs : FilePersistEventArgs
    {
        public TemplatePersistEventArgs(string fileName)
            : base(fileName)
        {
        }
    }
}
