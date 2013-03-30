using System;
using System.Linq;

namespace NBi.UI.Interface
{
    public class NewCsvSelectedEventArgs : EventArgs
    {
        public string FullPath { get; private set; }
        public NewCsvSelectedEventArgs(string fullPath)
        {
            FullPath = fullPath;
        }
    }
}
