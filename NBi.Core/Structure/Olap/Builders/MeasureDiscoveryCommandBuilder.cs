using NBi.Core.Structure;
using NBi.Core.Structure.Olap.PostFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.Builders;

class MeasureDiscoveryCommandBuilder : MultidimensionalDiscoveryCommandBuilder
{
    public MeasureDiscoveryCommandBuilder()
        : base("measure", "measure", "measures", "measure")
    { }

    protected override IEnumerable<IFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        yield return new CommandFilter("len(measuregroup_name)>0");

        yield return new CommandFilter(string.Format("[cube_name]='{0}'"
                                            , filters.Single(f => f.Target == Target.Perspectives).Caption
                                            ));

        var mgFilter = filters.SingleOrDefault(f => f.Target == Target.MeasureGroups);
        if (mgFilter != null)
            yield return new CommandFilter(string.Format("[measuregroup_name]='{0}'"
                                            , filters.Single(f => f.Target==Target.MeasureGroups).Caption
                                            ));

        var filter = filters.SingleOrDefault(f => f.Target == Target.Measures);
        if (filter != null)
            yield return new CommandFilter(string.Format("[measure_caption]='{0}'"
                                                       , filter.Caption
                                                       ));

        var dfFilter = filters.SingleOrDefault(f => f.Target == Target.DisplayFolders);
        if (dfFilter != null)
            yield return new DisplayFolder(dfFilter.Caption);
    }

}
