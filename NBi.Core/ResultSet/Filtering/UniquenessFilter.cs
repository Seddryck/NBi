using NBi.Core.Calculation.Grouping;
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

            public ResultSet Apply(ResultSet rs)
                => rs.Rows.Count == 1 ? rs : rs.Clone();
            
            public ResultSet AntiApply(ResultSet rs)
                => rs.Rows.Count != 1 ? rs : rs.Clone();
        }
    }
}
