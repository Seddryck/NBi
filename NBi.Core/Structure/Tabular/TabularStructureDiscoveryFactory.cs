using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Structure.Olap.PostFilters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Structure.Olap.Builders;
using NBi.Core.Structure;
using NBi.Core.Structure.Tabular.Builders;
using NBi.Core.Structure.Olap;

namespace NBi.Core.Structure.Tabular;

class TabularStructureDiscoveryFactory : OlapStructureDiscoveryFactory
{
    public TabularStructureDiscoveryFactory(IDbConnection connection)
        : base(connection)
    {
    }

    protected override IDiscoveryCommandBuilder InstantiateBuilder(Target target, TargetType type)
    {
        if (type != TargetType.Object)
            throw new ArgumentOutOfRangeException();

        return target switch
        {
            Target.Perspectives => new PerspectiveDiscoveryCommandBuilder(),
            Target.MeasureGroups => new MeasureGroupDiscoveryCommandBuilder(),
            Target.Measures => new MeasureDiscoveryCommandBuilder(),
            Target.Dimensions => new DimensionDiscoveryCommandBuilder(),
            Target.Hierarchies => new HierarchyDiscoveryCommandBuilder(),
            Target.Levels => new LevelDiscoveryCommandBuilder(),
            Target.Properties => new PropertyDiscoveryCommandBuilder(),
            Target.Tables => new TableDiscoveryCommandBuilder(),
            Target.Columns => new ColumnDiscoveryCommandBuilder(),
            Target.Sets => new SetDiscoveryCommandBuilder(),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
    
}
