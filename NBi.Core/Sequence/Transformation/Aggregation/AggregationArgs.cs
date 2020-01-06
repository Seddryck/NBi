using NBi.Core.ResultSet;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation
{
    public class AggregationArgs
    {
        public ColumnType ColumnType { get; }
        public AggregationFunctionType Function { get; }
        public IList<IScalarResolver> Parameters { get; } = new List<IScalarResolver>();
        public IList<IAggregationStrategy> Strategies { get; } = new List<IAggregationStrategy>();

        public AggregationArgs(AggregationFunctionType function, ColumnType columnType)
            => (ColumnType, Function) = (columnType, function);
    }
}
