using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface.TestSuiteGenerator.Events
{
    public abstract class FilePersistEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        public FilePersistEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}
