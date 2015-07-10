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

namespace NBi.Core.Structure.Tabular
{
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

            switch (target)
            {
                case Target.Perspectives:
                    return new PerspectiveDiscoveryCommandBuilder();
                case Target.MeasureGroups:
                    return new MeasureGroupDiscoveryCommandBuilder();
                case Target.Measures:
                    return new MeasureDiscoveryCommandBuilder();
                case Target.Dimensions:
                    return new DimensionDiscoveryCommandBuilder();
                case Target.Hierarchies:
                    return new HierarchyDiscoveryCommandBuilder();
                case Target.Levels:
                    return new LevelDiscoveryCommandBuilder();
                case Target.Properties:
                    return new PropertyDiscoveryCommandBuilder();
                case Target.Tables:
                    return new TableDiscoveryCommandBuilder();
                case Target.Columns:
                    return new ColumnDiscoveryCommandBuilder();
                case Target.Sets:
                    return new SetDiscoveryCommandBuilder();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
    }
}
