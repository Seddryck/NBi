using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Scalar.Comparer;

class ComparerFactory
{
    public BaseComparer Get(ColumnType type)
    {
        return type switch
        {
            ColumnType.Text => new TextComparer(),
            ColumnType.Numeric => new NumericComparer(),
            ColumnType.DateTime => new DateTimeComparer(),
            ColumnType.Boolean => new BooleanComparer(),
            _ => throw new ArgumentException(),
        };
    }
}
