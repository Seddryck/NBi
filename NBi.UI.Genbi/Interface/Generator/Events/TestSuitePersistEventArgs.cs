using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface.Generator.Events
{
    public class TestSuitePersistEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        public TestSuitePersistEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}
