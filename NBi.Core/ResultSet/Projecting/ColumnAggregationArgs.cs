using NBi.Core.Sequence.Transformation.Aggregation;
using NBi.Extensibility;

namespace NBi.Core.ResultSet.Projecting
{
    public class ColumnAggregationArgs : AggregationArgs
    {
        public IColumnIdentifier Source { get; }
        public IColumnIdentifier Destination { get; }

        public ColumnAggregationArgs(IColumnIdentifier column, AggregationArgs aggregation)
            : this(column, column, aggregation) { }

        public ColumnAggregationArgs(IColumnIdentifier source, IColumnIdentifier destination, AggregationArgs aggregation)
            : base(aggregation.Function, aggregation.ColumnType, [])
            => (Source, Destination) = (source, destination);
    }
}
