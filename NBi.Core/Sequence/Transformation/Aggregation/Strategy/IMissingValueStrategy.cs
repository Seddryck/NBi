using NBi.Core.ResultSet;
using NBi.Core.Scalar.Casting;
using NBi.Core.Sequence.Transformation.Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy
{
    public interface IMissingValueStrategy : IAggregationStrategy
    {
        IEnumerable<object> Execute(IEnumerable<object?> values);
    }
}
