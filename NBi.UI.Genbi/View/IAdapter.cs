using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.View
{
    interface IAdapter
    {
        void InvokeInitialize(EventArgs eventArgs);
    }
}
