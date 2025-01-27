using NBi.Core.ResultSet;
using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy;

public class ReplaceByDefaultStrategy : IMissingValueStrategy
{
    private object DefaultValue { get; }

    private ColumnType ColumnType { get; }

    public ReplaceByDefaultStrategy(ColumnType columnType, object defaultValue) 
        => (ColumnType, DefaultValue) = (columnType, defaultValue);

    public IEnumerable<object> Execute(IEnumerable<object?> values)
    {
        var caster = new CasterFactory().Instantiate(ColumnType);
        return values.Select(x => caster.IsStrictlyValid(x) ? caster.Execute(x) : DefaultValue).Cast<object>();
    }
}
