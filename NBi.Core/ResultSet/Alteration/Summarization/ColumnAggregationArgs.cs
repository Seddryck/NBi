using NBi.Core.Sequence.Transformation.Aggregation;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Summarization;

public class ColumnAggregationArgs : AggregationArgs
{
    public IColumnIdentifier Identifier { get; }

    public ColumnAggregationArgs(IColumnIdentifier identifier, AggregationFunctionType function, ColumnType columnType, IList<IScalarResolver> parameters)
        : base(function, columnType, parameters)
        => (Identifier) = (identifier);
}
