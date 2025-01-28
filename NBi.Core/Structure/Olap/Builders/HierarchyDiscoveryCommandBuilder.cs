using NBi.Core.Structure;
using NBi.Core.Structure.Olap.PostFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.Builders;

class HierarchyDiscoveryCommandBuilder : MultidimensionalDiscoveryCommandBuilder
{
    public HierarchyDiscoveryCommandBuilder()
        : base("hierarchy", "hierarchy", "hierarchies", "hierarchy")
    { }

    protected override IEnumerable<IFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        yield return new CommandFilter(string.Format("[cube_name]='{0}'"
                                                       , filters.Single(f => f.Target == Target.Perspectives).Caption
                                                       ));

        yield return new CommandFilter(string.Format("[dimension_unique_name]='[{0}]'"
                                                        , filters.Single(f => f.Target == Target.Dimensions).Caption
                                                        ));

        var filter = filters.SingleOrDefault(f => f.Target == Target.Hierarchies);
        if (filter != null)
            yield return new CommandFilter(string.Format("[hierarchy_caption]='{0}'"
                                                       , filter.Caption
                                                       ));

        var dfFilter = filters.SingleOrDefault(f => f.Target == Target.DisplayFolders);
        if (dfFilter != null)
            yield return new DisplayFolder(dfFilter.Caption);

    }

}
