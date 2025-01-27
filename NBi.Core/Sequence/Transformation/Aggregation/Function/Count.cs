using Deedle;
using NBi.Core.Scalar.Casting;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Function;

abstract class Count : IAggregationFunction
{
    public object Execute(IEnumerable<object> values) => values.Count();
}

class CountNumeric : Count { }
class CountText : Count { }
class CountDateTime : Count { }
class CountBoolean : Count { }
