using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Tabular.Builders;

class ColumnDiscoveryCommandBuilder : TabularDiscoveryCommandBuilder
{
    public ColumnDiscoveryCommandBuilder()
        : base("column_name", string.Empty, "columns", string.Empty)
    { }

    protected override IEnumerable<IFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        yield return new CommandFilter("column_name<>'RowNumber'");

        var filter = filters.SingleOrDefault(f => f.Target == Target.Perspectives);
        if (filter != null)
            yield return new CommandFilter(string.Format("[table_schema]='{0}'"
                                                        , filters.Single(f => f.Target == Target.Perspectives).Caption
                                                        ));

        yield return new CommandFilter(string.Format("[table_name]='${0}'"
                                                       , filters.Single(f => f.Target == Target.Tables).Caption
                                                       ));

        filter = filters.SingleOrDefault(f => f.Target == Target.Columns);
        if (filter != null)
            yield return new CommandFilter(string.Format("[column_name]='{0}'"
                                                       , filter.Caption
                                                       ));

    }
}
