using NBi.Core.ResultSet;
using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy;

public class DropStrategy : IMissingValueStrategy
{
    private ColumnType ColumnType { get; }

    public DropStrategy(ColumnType columnType)
        => ColumnType = columnType;

    public IEnumerable<object> Execute(IEnumerable<object?> values)
    {
        var caster = new CasterFactory().Instantiate(ColumnType);
        return values.Where(x => caster.IsValid(x) && x!=null).Select(x => caster.Execute(x)).Cast<object>();
    }
}
