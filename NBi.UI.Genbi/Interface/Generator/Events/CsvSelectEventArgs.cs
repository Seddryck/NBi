using System;
using System.Linq;

namespace NBi.UI.Genbi.Interface.Generator.Events
{
    public class CsvSelectEventArgs : EventArgs
    {
        public string FullPath { get; private set; }
        public CsvSelectEventArgs(string fullPath)
        {
            FullPath = fullPath;
        }
    }
}
