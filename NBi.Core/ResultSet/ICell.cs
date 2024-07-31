using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet
{
    public interface ICell
    {
        string? ColumnName { get; set; }
        object Value { get; set; }
    }
}
