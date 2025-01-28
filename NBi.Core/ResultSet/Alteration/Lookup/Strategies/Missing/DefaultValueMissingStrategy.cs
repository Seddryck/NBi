using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Lookup.Strategies.Missing;

public class DefaultValueMissingStrategy : IMissingStrategy
{
    public object Value { get; }

    public DefaultValueMissingStrategy(object defaultValue)
        => Value = defaultValue;

    public void Execute(IResultRow row, IResultColumn originalColumn, IResultColumn newColumn)
        => row[newColumn.Ordinal] = Value;
}
