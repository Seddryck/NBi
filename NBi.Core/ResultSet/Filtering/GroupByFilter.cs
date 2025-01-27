using NBi.Core.Calculation.Grouping;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering;

class GroupByFilter : IResultSetFilter
{
    protected IResultSetFilter Filter { get; }
    protected IGroupBy GroupBy { get; }
    protected Func<IResultSet, IResultSet> Execution { get; }

    public GroupByFilter(IResultSetFilter filter, IGroupBy groupBy)
        => (Filter, GroupBy, Execution) = (filter, groupBy, Apply);

    public IResultSet Execute(IResultSet rs)
        => Execution.Invoke(rs);

    public IResultSet Apply(IResultSet rs)
    {
        var newRs = rs.Clone();
        var groups = GroupBy.Execute(rs);
        foreach (var group in groups)
        {
            var filtered = Filter.Apply(group.Value);
            newRs.AddRange(filtered.Rows);
        }
        return newRs;
    }

    public IResultSet AntiApply(IResultSet rs)
        => throw new NotImplementedException();

    public virtual string Describe()
        => $"{Filter.Describe()} after grouping by {GroupBy.ToString()}";
}
