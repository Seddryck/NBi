using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType.Relational.Builders;

class ColumnDiscoveryCommandBuilder : RelationalDiscoveryCommandBuilder
{

    public ColumnDiscoveryCommandBuilder()
    {
    }

    protected override IEnumerable<ICommandFilter> BuildFilters(IEnumerable<CaptionFilter> filters)
    {
        yield return new CommandFilter(string.Format("[table_schema]='{0}'"
                                                        , filters.Single(f => f.Target == Target.Perspectives).Caption
                                                        ));

        yield return new CommandFilter(string.Format("[table_name]='{0}'"
                                                       , filters.Single(f => f.Target == Target.Tables).Caption
                                                       ));

        yield return new CommandFilter(string.Format("[column_name]='{0}'"
                                                       , filters.Single(f => f.Target == Target.Columns).Caption
                                                       ));

    }
}
