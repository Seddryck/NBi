using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Summarization;

public class SummarizeArgs : ISummarizationArgs
{
    public IEnumerable<IColumnDefinitionLight> GroupBys { get; set; }
    public IEnumerable<ColumnAggregationArgs> Aggregations { get; set; }

    public SummarizeArgs(IEnumerable<ColumnAggregationArgs> aggregations, IEnumerable<IColumnDefinitionLight> groupBys)
        => (Aggregations, GroupBys) = (aggregations, groupBys);
}
