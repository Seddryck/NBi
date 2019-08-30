using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy
{
    class ReturnDefaultStrategy : IEmptySeriesStrategy
    {
        private object DefaultValue { get; }

        public ReturnDefaultStrategy(object defaultValue) => DefaultValue = defaultValue;

        public object Execute() => DefaultValue;
    }
}
