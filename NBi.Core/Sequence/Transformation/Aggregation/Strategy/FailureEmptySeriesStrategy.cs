using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy;

class FailureEmptySeriesStrategy : IEmptySeriesStrategy
{
    public object Execute() => throw new ArgumentNullException();
}
