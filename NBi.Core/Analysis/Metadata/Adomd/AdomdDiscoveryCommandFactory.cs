﻿using System;
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

            cmd = Build(request);
            cmd.Filters = request.GetAllFilters();

            return cmd;
        }

        public virtual AdomdDiscoveryCommand BuildExternal(MetadataDiscoveryRequest request)
        {
            AdomdDiscoveryCommand cmd = null;

            cmd = Build(request);
            cmd.Filters = request.GetAllFilters().Where(f => f.Target!=request.Target);

            return cmd;
        }

        public virtual AdomdDiscoveryCommand BuildInternal(MetadataDiscoveryRequest request)
        {
            AdomdDiscoveryCommand cmd = null;

            cmd = Build(request);
            cmd.Filters = request.GetAllFilters().Where(f => f.Target == request.Target);

            return cmd;
        }

        protected virtual AdomdDiscoveryCommand Build(MetadataDiscoveryRequest request)
        {
            switch (request.Target)
            {
                case DiscoveryTarget.Perspectives:
                    return new PerspectiveDiscoveryCommand(request.ConnectionString);
                case DiscoveryTarget.MeasureGroups:
                    return new MeasureGroupDiscoveryCommand(request.ConnectionString);
                case DiscoveryTarget.Measures:
                    return new MeasureDiscoveryCommand(request.ConnectionString);
                case DiscoveryTarget.Dimensions:
                    return new DimensionDiscoveryCommand(request.ConnectionString);
                case DiscoveryTarget.Hierarchies:
                    return new HierarchyDiscoveryCommand(request.ConnectionString);
                case DiscoveryTarget.Levels:
                    return new LevelDiscoveryCommand(request.ConnectionString);
                case DiscoveryTarget.Properties:
                    return new PropertyDiscoveryCommand(request.ConnectionString);
                case DiscoveryTarget.Tables:
                    return new TableDiscoveryCommand(request.ConnectionString);
                case DiscoveryTarget.Columns:
                    return new ColumnDiscoveryCommand(request.ConnectionString);
            }
            throw new ArgumentOutOfRangeException();
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
    }
}
