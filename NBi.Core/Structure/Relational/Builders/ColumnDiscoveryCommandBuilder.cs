using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational.Builders;

class ColumnDiscoveryCommandBuilder : RelationalDiscoveryCommandBuilder
{
    public ColumnDiscoveryCommandBuilder()
        : base("column", "columns")
    { }

    protected override IEnumerable<ICommandFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        yield return new CommandFilter(string.Format("[table_schema]='{0}'"
                                                        , filters.Single(f => f.Target == Target.Perspectives).Caption
                                                        ));

        yield return new CommandFilter(string.Format("[table_name]='{0}'"
                                                       , filters.Single(f => f.Target == Target.Tables).Caption
                                                       ));

        var filter = filters.SingleOrDefault(f => f.Target == Target.Columns);
        if (filter != null)
            yield return new CommandFilter(string.Format("[column_name]='{0}'"
                                                       , filter.Caption
                                                       ));

    }
}
