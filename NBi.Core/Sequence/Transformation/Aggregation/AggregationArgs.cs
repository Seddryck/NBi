using NBi.Core.ResultSet;
using NBi.Extensibility.Resolving;
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
        public IList<IScalarResolver> Parameters { get; } = [];
        public IList<IAggregationStrategy> Strategies { get; } = [];

        public AggregationArgs(AggregationFunctionType function, ColumnType columnType, IList<IScalarResolver> parameters)
            => (ColumnType, Function, Parameters) = (columnType, function, parameters ?? []);
    }
}
