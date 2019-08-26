using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Summarization
{
    public class SummerizeArgs : ISummarizationArgs
    {
        public List<IColumnDefinitionLight> GroupBys { get; set; }
        public List<ColumnAggregationArgs> Aggregations { get; set; }

        public SummerizeArgs(List<ColumnAggregationArgs> aggregations, List<IColumnDefinitionLight> groupBys)
            => (Aggregations, GroupBys) = (aggregations, groupBys);
    }
}
