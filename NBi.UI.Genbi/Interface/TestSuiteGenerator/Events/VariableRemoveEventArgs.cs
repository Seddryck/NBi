using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface.TestSuiteGenerator.Events
{
    public class VariableRemoveEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public VariableRemoveEventArgs(int columnIndex)
        {
            Index = columnIndex;

        }
    }
}
