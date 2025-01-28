using NBi.Core.Structure;
using NBi.Core.Structure.Olap.PostFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.Builders;

class SetDiscoveryCommandBuilder : MultidimensionalDiscoveryCommandBuilder
{
    public SetDiscoveryCommandBuilder()
        : base("set", "set", "sets", string.Empty)
    { }

    protected override IEnumerable<IFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        yield return new CommandFilter(string.Format("[cube_name]='{0}'"
                                                       , filters.Single(f => f.Target == Target.Perspectives).Caption
                                                       ));

        var filter = filters.SingleOrDefault(f => f.Target == Target.Sets);
        if (filter != null)
            yield return new CommandFilter(string.Format("[set_caption]='{0}'"
                                                       , filter.Caption
                                                       ));
        
        var dfFilter = filters.SingleOrDefault(f => f.Target == Target.DisplayFolders);
        if (dfFilter != null)
            yield return new DisplayFolder(dfFilter.Caption);
    }

}
