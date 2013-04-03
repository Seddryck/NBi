using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface.Generator.Events
{
    public class VariableRenameEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public string NewName { get; private set; }
        public VariableRenameEventArgs(int columnIndex, string newName)
        {
            Index = columnIndex;
            NewName = newName;
        }
    }
}
