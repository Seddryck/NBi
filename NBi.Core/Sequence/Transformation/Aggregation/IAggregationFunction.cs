using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation;

public interface IAggregationFunction : ISequenceTransformation
{
    object? Execute(IEnumerable<object> values);
}
