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

namespace NBi.Core.Structure.Olap
{
    class OlapStructureDiscoveryFactory : IStructureDiscoveryFactory
    {
        private readonly AdomdConnection connection;
        public OlapStructureDiscoveryFactory(IDbConnection connection)
        {
            this.connection = connection as AdomdConnection ?? throw new ArgumentException();
        }

        public StructureDiscoveryCommand Instantiate(Target target, TargetType type, IEnumerable<IFilter> filters)
        {
            if (connection is not AdomdConnection)
                throw new ArgumentException();

            var builder = InstantiateBuilder(target, type);
            builder.Build(filters);

            var cmd = connection.CreateCommand();
            cmd.CommandText = builder.GetCommandText();
            var postFilters = builder.GetPostFilters();

            var description = new CommandDescription(target, filters);

            OlapCommand? command;
            if ((target == Target.MeasureGroups && type == TargetType.Object) || target == Target.Perspectives)
                command = new DistinctOlapCommand(cmd, postFilters, description);
            else if (target == Target.Dimensions && type == TargetType.Object)
                command = new DimensionCommand(cmd, postFilters, description);
            else if (target == Target.Dimensions && type == TargetType.Relation)
                command = new DimensionRelationCommand(cmd, postFilters, description);
            else
                command = new OlapCommand(cmd, postFilters, description);

            return command!;
        }

        protected virtual IDiscoveryCommandBuilder InstantiateBuilder(Target target, TargetType type)
        {

            return type switch
            {
                TargetType.Object => target switch
                {
                    Target.Perspectives => new PerspectiveDiscoveryCommandBuilder(),
                    Target.MeasureGroups => new MeasureGroupDiscoveryCommandBuilder(),
                    Target.Measures => new MeasureDiscoveryCommandBuilder(),
                    Target.Dimensions => new DimensionDiscoveryCommandBuilder(),
                    Target.Hierarchies => new HierarchyDiscoveryCommandBuilder(),
                    Target.Levels => new LevelDiscoveryCommandBuilder(),
                    Target.Properties => new PropertyDiscoveryCommandBuilder(),
                    Target.Sets => new SetDiscoveryCommandBuilder(),
                    _ => throw new ArgumentOutOfRangeException(),
                },
                TargetType.Relation => target switch
                {
                    Target.MeasureGroups => new MeasureGroupRelationDiscoveryCommandBuilder(),
                    Target.Dimensions => new DimensionRelationDiscoveryCommandBuilder(),
                    _ => throw new ArgumentOutOfRangeException(),
                },
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
        
    }
}
