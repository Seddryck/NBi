using System;
using System.Linq;
using NBi.Core.ResultSet;

namespace NBi.Core.Evaluate
{
    public interface IColumnExpression
    {
        int Column { get; set; }
        string Value { get; set; }
        ColumnType Type { get; set; }
        string Tolerance { get; set; }
    }
}
