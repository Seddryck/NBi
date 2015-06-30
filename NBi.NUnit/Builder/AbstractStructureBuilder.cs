using System;
using System.Linq;
using NBi.Core.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using System.Collections.Generic;
using NBi.Xml.Items.Filters;

namespace NBi.NUnit.Builder
{
    abstract class AbstractStructureBuilder : AbstractTestCaseBuilder
    {
        protected StructureXml SystemUnderTestXml { get; set; }
        protected readonly StructureDiscoveryFactoryProvider discoveryProvider;

        public AbstractStructureBuilder()
        {
            discoveryProvider = new StructureDiscoveryFactoryProvider();
        }

        internal AbstractStructureBuilder(StructureDiscoveryFactoryProvider discoveryProvider)
        {
            this.discoveryProvider = discoveryProvider;
        }

        protected override void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(sutXml is StructureXml))
                throw new ArgumentException("System-under-test must be a 'StructureXml'");

            SystemUnderTestXml = (StructureXml)sutXml;
        }

        protected override void BaseBuild()
        {
            SystemUnderTest = InstantiateSystemUnderTest(SystemUnderTestXml);
        }

        protected virtual object InstantiateSystemUnderTest(StructureXml sutXml)
        {
            return InstantiateCommand(sutXml.Item);
        }

        protected virtual StructureDiscoveryCommand InstantiateCommand(AbstractItem item)
        {
            var factory = discoveryProvider.Instantiate(item.GetConnectionString());

            var target = BuildTarget(item);
            var filters = BuildFilters(item);

            var command = factory.Instantiate(target, TargetType.Object, filters);
            return command;
        }

        protected virtual IEnumerable<CaptionFilter> BuildFilters(AbstractItem item)
        {
            if (item is IPerspectiveFilter)
                yield return new CaptionFilter(Target.Perspectives, ((IPerspectiveFilter)item).Perspective);
            if (item is IDimensionFilter)
                yield return new CaptionFilter(Target.Dimensions, ((IDimensionFilter)item).Dimension);
            if (item is IHierarchyFilter)
                yield return new CaptionFilter(Target.Hierarchies, ((IHierarchyFilter)item).Hierarchy);
            if (item is ILevelFilter)
                yield return new CaptionFilter(Target.Levels, ((ILevelFilter)item).Level);
            if (item is IMeasureGroupFilter && !(string.IsNullOrEmpty(((IMeasureGroupFilter)item).MeasureGroup)))
                yield return new CaptionFilter(Target.MeasureGroups, ((IMeasureGroupFilter)item).MeasureGroup);
            if (item is ITableFilter)
                yield return new CaptionFilter(Target.Tables, ((ITableFilter)item).Table);

            var itselfTarget = BuildTarget(item);
            if (!string.IsNullOrEmpty(item.Caption))
                yield return new CaptionFilter(itselfTarget, item.Caption);
        }

        protected virtual Target BuildTarget(AbstractItem item)
        {

            if (item is MeasuresXml || item is MeasureXml)
                return Target.Measures;
            if (item is MeasureGroupsXml || item is MeasureGroupXml)
                return Target.MeasureGroups;
            if (item is ColumnsXml || item is ColumnXml)
                return Target.Columns;
            if (item is TablesXml || item is TableXml)
                return Target.Tables;
            if (item is PropertiesXml || item is PropertyXml)
                return Target.Properties;
            if (item is LevelsXml || item is LevelXml)
                return Target.Levels;
            if (item is HierarchiesXml || item is HierarchyXml)
                return Target.Hierarchies;
            if (item is DimensionsXml || item is DimensionXml)
                return Target.Dimensions;
            if (item is SetsXml || item is SetXml)
                return Target.Sets;
            if (item is RoutinesXml || item is RoutineXml)
                return Target.Routines;
            if (item is PerspectivesXml || item is PerspectiveXml)
                return Target.Perspectives;
            else
                throw new ArgumentException(item.GetType().Name);
        }

    }
}
