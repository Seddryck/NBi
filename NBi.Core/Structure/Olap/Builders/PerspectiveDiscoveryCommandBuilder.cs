using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.Builders;

class PerspectiveDiscoveryCommandBuilder : MultidimensionalDiscoveryCommandBuilder
{
    public PerspectiveDiscoveryCommandBuilder()
        : base("cube", string.Empty, "dimensions", "dimension")
    { }

    protected override string BasicCommandText
    {
        get { return "select {0}_name, {1} from [$system].mdschema_{2} where left(cube_name,1)<>'$'{3}"; }
    }

    protected override IEnumerable<IFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        var filter = filters.SingleOrDefault(f => f.Target == Target.Perspectives);
        if (filter != null)
            yield return new CommandFilter(string.Format("[cube_name]='{0}'"
                                                       , filter.Caption
                                                       ));

    }
}
