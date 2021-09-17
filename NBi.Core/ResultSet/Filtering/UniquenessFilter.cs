using NBi.Core.Calculation.Grouping;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering
{
    class UniquenessFilter : GroupByFilter
    {
        public UniquenessFilter(IGroupBy groupBy)
            : base(new SingleFilter(), groupBy) { }

        private class SingleFilter : IResultSetFilter
        {
            public string Describe() => "Unique rows";

            public IResultSet Apply(IResultSet rs)
                => rs.RowCount == 1 ? rs : rs.Clone();
            
            public IResultSet AntiApply(IResultSet rs)
                => rs.RowCount != 1 ? rs : rs.Clone();
        }
    }
}
