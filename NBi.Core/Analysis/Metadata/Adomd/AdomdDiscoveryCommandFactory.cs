using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    public class AdomdDiscoveryCommandFactory
    {
        
        public AdomdDiscoveryCommandFactory()
        {
        }

        public virtual AdomdDiscoveryCommand BuildExact(MetadataDiscoveryRequest request)
        {
            AdomdDiscoveryCommand cmd = null;

            cmd = BuildBase(request.Target, request.ConnectionString);
            cmd.Filters = request.GetAllFilters();

            return cmd;
        }

        public virtual AdomdDiscoveryCommand BuildExternal(MetadataDiscoveryRequest request)
        {
            AdomdDiscoveryCommand cmd = null;

            cmd = BuildBase(request.Target, request.ConnectionString);
            cmd.Filters = request.GetAllFilters().Where(f => f.Target!=request.Target);

            return cmd;
        }

        public virtual AdomdDiscoveryCommand BuildInternal(MetadataDiscoveryRequest request)
        {
            AdomdDiscoveryCommand cmd = null;

            cmd = BuildBase(request.Target, request.ConnectionString);
            cmd.Filters = request.GetAllFilters().Where(f => f.Target == request.Target);

            return cmd;
        }


        public virtual AdomdDiscoveryCommand BuildLinkedTo(MetadataDiscoveryRequest request)
        {
            AdomdDiscoveryCommand cmd = null;

            switch (request.Target)
            {
                case DiscoveryTarget.MeasureGroups:
                    cmd= new DimensionLinkedToDiscoveryCommand(request.ConnectionString);
                    break;
                case DiscoveryTarget.Dimensions:
                    cmd= new MeasureGroupLinkedToDiscoveryCommand(request.ConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            cmd.Filters = request.GetAllFilters();

            return cmd;
        }

        public AdomdDiscoveryCommand Build(DiscoveryTarget target, IEnumerable<IFilter> filters, string connectionString)
        {
            var command = BuildBase(target, connectionString);
            command.Filters = filters;
            return command;
        }

        protected AdomdDiscoveryCommand BuildBase(DiscoveryTarget target, string connectionString)
        {
            switch (target)
            {
                case DiscoveryTarget.Perspectives:
                    return new PerspectiveDiscoveryCommand(connectionString);
                case DiscoveryTarget.MeasureGroups:
                    return new MeasureGroupDiscoveryCommand(connectionString);
                case DiscoveryTarget.Measures:
                    return new MeasureDiscoveryCommand(connectionString);
                case DiscoveryTarget.Dimensions:
                    return new DimensionDiscoveryCommand(connectionString);
                case DiscoveryTarget.Hierarchies:
                    return new HierarchyDiscoveryCommand(connectionString);
                case DiscoveryTarget.Levels:
                    return new LevelDiscoveryCommand(connectionString);
                case DiscoveryTarget.Properties:
                    return new PropertyDiscoveryCommand(connectionString);
                case DiscoveryTarget.Tables:
                    return new TableDiscoveryCommand(connectionString);
                case DiscoveryTarget.Columns:
                    return new ColumnDiscoveryCommand(connectionString);
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
