using System;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.Core.Transformation;

namespace NBi.Core.Evaluate;

public interface IColumnExpression
{
    int Column { get; set; }
    string Name { get; set; }
    string Value { get; set; }
    LanguageType Language { get; }
    ColumnType Type { get; set; }
    string Tolerance { get; set; }
}
