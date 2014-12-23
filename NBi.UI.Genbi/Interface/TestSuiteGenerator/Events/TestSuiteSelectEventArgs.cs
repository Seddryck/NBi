using System;
using System.Linq;

namespace NBi.UI.Genbi.Interface.TestSuiteGenerator.Events
{
    public class TestSuiteSelectEventArgs : EventArgs
    {
        public string FullPath { get; private set; }
        public TestSuiteSelectEventArgs(string fullPath)
        {
            FullPath = fullPath;
        }
    }
}
