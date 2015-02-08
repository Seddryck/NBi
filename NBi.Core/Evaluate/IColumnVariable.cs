using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Evaluate
{
    public interface IColumnVariable
    {
        int Column { get; set; }
        string Name { get; set; }
    }
}
