using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet
{
    public interface ICell
    {
        object Value { get; set; }
    }
}
