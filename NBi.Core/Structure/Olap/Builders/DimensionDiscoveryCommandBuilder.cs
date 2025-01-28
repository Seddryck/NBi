using NBi.Core.Structure;
using NBi.Core.Structure.Olap.PostFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.Builders;

class DimensionDiscoveryCommandBuilder : MultidimensionalDiscoveryCommandBuilder
{
    protected override string BasicCommandText
    {
        get { return "select {0}_caption, {1}, dimension_type from [$system].mdschema_{2} where left(cube_name,1)<>'$'{3}"; }
    }

    public DimensionDiscoveryCommandBuilder()
        : base("dimension", string.Empty, "dimensions", "dimension")
    { }

    protected override IEnumerable<IFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        yield return new CommandFilter(string.Format("[cube_name]='{0}'"
                                                        , filters.Single(f => f.Target == Target.Perspectives).Caption
                                                        ));

        yield return new DimensionType();

        var filter = filters.SingleOrDefault(f => f.Target == Target.Dimensions);
        if (filter != null)
            yield return new CommandFilter(string.Format("[dimension_caption]='{0}'"
                                                       , filter.Caption
                                                       ));

    }

}
