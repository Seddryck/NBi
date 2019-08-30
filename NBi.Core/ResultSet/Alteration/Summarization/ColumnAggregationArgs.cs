using NBi.Core.Sequence.Transformation.Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Summarization
{
    public class ColumnAggregationArgs : AggregationArgs
    {
        public IColumnIdentifier Identifier { get; }
        
        public ColumnAggregationArgs(IColumnIdentifier identifier, AggregationFunctionType function, ColumnType columnType)
            : base(function, columnType)
            => (Identifier) = (identifier);
    }
}
