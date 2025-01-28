using NBi.Core.Structure;
using NBi.Core.Structure.Olap.PostFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.Builders;

class DimensionRelationDiscoveryCommandBuilder : MultidimensionalDiscoveryCommandBuilder
{
    protected override string BasicCommandText
    {
        get { return "select [{0}_unique_name], {1} from [$system].mdschema_{2} where left(cube_name,1)<>'$'{3}"; }
    }

    public DimensionRelationDiscoveryCommandBuilder()
        : base("dimension", string.Empty, "measuregroup_dimensions", "dimension")
    { }

    protected override IEnumerable<IFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        yield return new CommandFilter("len(measuregroup_name)>0");

        var filter = filters.SingleOrDefault(f => f.Target == Target.Perspectives);
        if (filter != null)
            yield return new CommandFilter(string.Format("[cube_name]='{0}'"
                                                        , filters.Single(f => f.Target == Target.Perspectives).Caption
                                                        ));

        filter = filters.SingleOrDefault(f => f.Target == Target.MeasureGroups);
        if (filter != null)
            yield return new CommandFilter(string.Format("[measuregroup_name]='{0}'"
                                , filters.Single(f => f.Target == Target.MeasureGroups).Caption
                                ));

        filter = filters.SingleOrDefault(f => f.Target == Target.Dimensions);
        if (filter != null)
            yield return new CommandFilter(string.Format("[dimension_unique_name]='[{0}]'"
                                                       , filter.Caption
                                                       ));

    }

}
