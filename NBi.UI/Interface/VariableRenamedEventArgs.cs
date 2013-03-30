using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Interface
{
    public class VariableRenamedEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public string NewName { get; private set; }
        public VariableRenamedEventArgs(int columnIndex, string newName)
        {
            Index = columnIndex;
            NewName = newName;
        }
    }
}
