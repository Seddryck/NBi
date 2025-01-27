using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.Builders;

class PropertyDiscoveryCommandBuilder : MultidimensionalDiscoveryCommandBuilder
{
    public PropertyDiscoveryCommandBuilder()
        : base("property", string.Empty, "properties", "property")
    { }
    
    protected override IEnumerable<IFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        yield return new CommandFilter("[property_type]=1");
        
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

        yield return new CommandFilter(string.Format("[level_unique_name]='[{0}].[{1}].[{2}]'"
                                            , filters.Single(f => f.Target == Target.Dimensions).Caption
                                            , filters.Single(f => f.Target == Target.Hierarchies).Caption
                                            , filters.Single(f => f.Target == Target.Levels).Caption
                                            ));

        var filter = filters.SingleOrDefault(f => f.Target == Target.Properties);
        if (filter!=null)
            yield return new CommandFilter(string.Format("[property_caption]='{0}'"
                                                       , filter.Caption
                                                       ));
    }

}
