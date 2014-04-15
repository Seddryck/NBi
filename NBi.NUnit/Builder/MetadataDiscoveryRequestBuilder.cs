using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NBi.Xml.Items;

namespace NBi.NUnit.Builder
{
    internal class MetadataDiscoveryRequestBuilder
    {

        public enum MetadataDiscoveryRequestType
        {
            Direct,
            Relation
        }

        private string GetPropertyValue(object src, string propertyName)
        {
            var propertyInfo = src.GetType().GetProperty(propertyName);
            if (propertyInfo==null)
                return null;

            return (string)propertyInfo.GetValue(src, null);
        }

        private DiscoveryTarget GetTarget(object item)
        {
            if (item is MeasuresXml || item is MeasureXml)
                return DiscoveryTarget.Measures;
            if (item is MeasureGroupsXml || item is MeasureGroupXml)
                return DiscoveryTarget.MeasureGroups;
            if (item is PropertiesXml || item is PropertyXml)
                return DiscoveryTarget.Properties;
            if (item is LevelsXml || item is LevelXml)
                return DiscoveryTarget.Levels;
            if (item is HierarchiesXml || item is HierarchyXml)
                return DiscoveryTarget.Hierarchies;
            if (item is DimensionsXml || item is DimensionXml)
                return DiscoveryTarget.Dimensions;
            if (item is ColumnsXml || item is ColumnXml)
                return DiscoveryTarget.Columns;
            if (item is TablesXml || item is TableXml)
                return DiscoveryTarget.Tables;
            if (item is PerspectivesXml || item is PerspectiveXml)
                return DiscoveryTarget.Perspectives;

            throw new ArgumentException();
        }

        private DiscoveryTarget GetTargetRelation(object item)
        {
            if (item is MeasureGroupsXml || item is MeasureGroupXml)
                return DiscoveryTarget.Dimensions;
            if (item is DimensionsXml || item is DimensionXml)
                return DiscoveryTarget.MeasureGroups;

            throw new ArgumentException();
        }

        private DiscoveryTarget GetTargetForProperty(string propertyName)
        {
            if (propertyName == "DisplayFolder")
                return DiscoveryTarget.DisplayFolders;
            if (propertyName=="MeasureGroup")
                return DiscoveryTarget.MeasureGroups;
            if (propertyName == "Level")
                return DiscoveryTarget.Levels;
            if (propertyName == "Hierarchy")
                return DiscoveryTarget.Hierarchies;
            if (propertyName == "Dimension")
                return DiscoveryTarget.Dimensions;
            if (propertyName == "Table")
                return DiscoveryTarget.Tables;
            if (propertyName == "Perspective")
                return DiscoveryTarget.Perspectives;

            throw new ArgumentException();
        }

        private IFilter BuildCaptionFilter(object src, string propertyName)
        {
            var caption = GetPropertyValue(src, propertyName);
            if (string.IsNullOrEmpty(caption))
                return null;

            var target = GetTargetForProperty(propertyName);
            var filter = new CaptionFilter(caption, target);
            return filter;

        }

        private IFilter BuildCaptionFilterForCaptionProperty(object src, DiscoveryTarget target)
        {
            var caption = GetPropertyValue(src, "Caption");
            if (string.IsNullOrEmpty(caption))
                return null;

            var filter = new CaptionFilter(caption, target);
            return filter;
        }

        internal MetadataDiscoveryRequest Build(AbstractItem item, MetadataDiscoveryRequestType type)
        {
            var properties = new string[] { "Perspective", "Dimension", "Hierarchy", "Level", "Property", "MeasureGroup", "DisplayFolder", "Measure", "Table", "Column" };

            IFilter filter = null;
            var filters = new List<IFilter>();
            foreach (var property in properties)
            {
                filter = BuildCaptionFilter(item, property);
                if (filter != null)
                    filters.Add(filter);
            }

            var target = GetTarget(item);

            filter = BuildCaptionFilterForCaptionProperty(item, target);
            if (filter != null)
                filters.Add(filter);

            var connectionString = item.GetConnectionString();

            var factory = new DiscoveryRequestFactory();
            MetadataDiscoveryRequest request = null;
            switch (type)
            {
                case MetadataDiscoveryRequestType.Direct:
                    request = factory.BuildDirect(connectionString, target, filters);
                    break;
                case MetadataDiscoveryRequestType.Relation:
                    target = GetTargetRelation(item);
                    request = factory.BuildRelation(connectionString, target, filters);
                    break;
                default:
                    break;
            }
            return request;
        }      

        
    }
}
