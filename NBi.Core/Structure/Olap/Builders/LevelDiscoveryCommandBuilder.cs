using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.Builders;

class LevelDiscoveryCommandBuilder : MultidimensionalDiscoveryCommandBuilder
{
    public LevelDiscoveryCommandBuilder()
        : base("level", string.Empty, "levels", "level")
    { }

    protected override IEnumerable<IFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        yield return new CommandFilter(string.Format("[cube_name]='{0}'"
                                                       , filters.Single(f => f.Target == Target.Perspectives).Caption
                                                       ));

        yield return new CommandFilter(string.Format("[dimension_unique_name]='[{0}]'"
                                                        , filters.Single(f => f.Target == Target.Dimensions).Caption
                                                        ));

        yield return new CommandFilter(string.Format("[hierarchy_unique_name]='[{0}].[{1}]'"
                                            , filters.Single(f => f.Target == Target.Dimensions).Caption
                                            , filters.Single(f => f.Target == Target.Hierarchies).Caption
                                            ));

        var filter = filters.SingleOrDefault(f => f.Target == Target.Levels);
        if (filter != null)
            yield return new CommandFilter(string.Format("[level_caption]='{0}'"
                                                       , filter.Caption
                                                       ));

    }

}
