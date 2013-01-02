using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Interface
{
    public class PersistTestSuiteEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        public PersistTestSuiteEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}
