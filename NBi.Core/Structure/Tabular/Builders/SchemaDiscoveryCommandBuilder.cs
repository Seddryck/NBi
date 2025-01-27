using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Tabular.Builders;

class SchemaDiscoveryCommandBuilder : TabularDiscoveryCommandBuilder
{
    public SchemaDiscoveryCommandBuilder()
        : base("table_schema", string.Empty, "tables", string.Empty)
    { }

    protected override IEnumerable<IFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        yield return new CommandFilter("left(table_schema,1)<>'$'");

        
        var filter = filters.SingleOrDefault(f => f.Target == Target.Perspectives);
        if (filter != null)
            yield return new CommandFilter(string.Format("[table_schema]='{0}'"
                                                       , filter.Caption
                                                       ));

    }

}
