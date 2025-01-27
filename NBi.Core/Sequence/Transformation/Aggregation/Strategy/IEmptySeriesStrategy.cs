using NBi.Core.Sequence.Transformation.Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy;

public interface IEmptySeriesStrategy : IAggregationStrategy
{
    object Execute();
}
