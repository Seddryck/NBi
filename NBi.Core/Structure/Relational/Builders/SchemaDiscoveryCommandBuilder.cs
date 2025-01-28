using NBi.Core.Structure;
using NBi.Core.Structure.Relational.PostFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational.Builders;

class SchemaDiscoveryCommandBuilder : RelationalDiscoveryCommandBuilder
{
    public SchemaDiscoveryCommandBuilder()
        : base("schema", "schemata")
    { }

    protected override IEnumerable<ICommandFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        var filter = filters.SingleOrDefault(f => f.Target == Target.Perspectives);
        if (filter != null)
            yield return new CommandFilter($"[schema_name]='{filter.Caption}'");
    }

    protected override IEnumerable<IFilter> BuildNonCaptionFilters(IEnumerable<IFilter> filters)
    {
        var ownerFilter = (IValueFilter?)filters.SingleOrDefault(f => f is OwnerFilter, null);
        if (ownerFilter != null)
            yield return new CommandFilter($"[schema_owner]='{ownerFilter.Value}'");
    }
}
