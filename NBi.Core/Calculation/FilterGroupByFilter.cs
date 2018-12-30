using NBi.Core.Calculation.Grouping;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    class FilterGroupByFilter: IResultSetFilter
    {
        private readonly IResultSetFilter filter;
        private readonly IByColumnGrouping groupBy;

        public FilterGroupByFilter(IResultSetFilter filter, IByColumnGrouping groupBy)
        {
            this.filter = filter;
            this.groupBy = groupBy;
        }
        
        public ResultSet.ResultSet Apply(ResultSet.ResultSet rs)
        {
            var newRs = rs.Clone();
            var groups = groupBy.Execute(rs);
            foreach (var group in groups)
            {
                var groupRs = new ResultSet.ResultSet();
                groupRs.Load(group.Value);
                var filtered = filter.Apply(groupRs);
                newRs.AddRange(filtered.Rows.Cast<DataRow>());
            }
            return newRs;
        }

        public ResultSet.ResultSet AntiApply(ResultSet.ResultSet rs)
        {
            throw new NotImplementedException();
        }

        public string Describe()
        {
            throw new NotImplementedException();
        }
    }
}
