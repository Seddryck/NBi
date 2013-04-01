using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Interface
{
    public class SelectedTestEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public SelectedTestEventArgs(int index)
        {
            Index = index;
        }

    }
}
