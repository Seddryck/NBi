using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface.Generator.Events
{
    public class TestSelectEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public TestSelectEventArgs(int index)
        {
            Index = index;
        }

    }
}
