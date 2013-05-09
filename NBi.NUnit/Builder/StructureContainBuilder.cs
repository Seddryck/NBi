using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class StructureContainBuilder: AbstractStructureBuilder
    {
        protected ContainXml ConstraintXml {get; set;}
        
        public StructureContainBuilder() : base()
        {
        }

        internal StructureContainBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is ContainXml))
                throw new ArgumentException("Constraint must be a 'ContainXml'");

            ConstraintXml = (ContainXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(ContainXml ctrXml)
        {
            var ctr = new NBi.NUnit.Structure.ContainConstraint(ctrXml.Items);
            //Ignore-case if requested
            if (ctrXml.IgnoreCase)
                ctr = ctr.IgnoreCase;
            return ctr;
        }

        protected override object InstantiateSystemUnderTest(StructureXml sutXml)
        {
            string perspective = null
                , measuregroup = null, displayFolder = null, measure = null
                , dimension = null, hierarchy = null, level = null
                , table = null, column = null;

            if (sutXml.Item.GetType() == typeof(PerspectivesXml))
            {
                perspective = null;
            }
            if (sutXml.Item is MeasureGroupsXml)
            {
                var persp = ((MeasureGroupsXml)sutXml.Item).Perspective;
                if (string.IsNullOrEmpty(persp))
                    throw new ArgumentException("Perspective must be specified");

                perspective = ((MeasureGroupsXml)sutXml.Item).Perspective;
            }
            if (sutXml.Item.GetType() == typeof(MeasuresXml))
            {
                var mg = ((MeasuresXml)sutXml.Item).MeasureGroup;
                if (string.IsNullOrEmpty(mg))
                    throw new ArgumentException("Measure-group must be specified");

                measuregroup = mg;
            }
            if (sutXml.Item is DimensionsXml)
            {
                var persp = ((DimensionsXml)sutXml.Item).Perspective;
                if (string.IsNullOrEmpty(persp))
                    throw new ArgumentException("Perspective must be specified");

                perspective = persp;
            }
            if (sutXml.Item is HierarchiesXml)
            {
                var dim = ((HierarchiesXml)sutXml.Item).Dimension;
                if (string.IsNullOrEmpty(dim))
                    throw new ArgumentException("Dimension must be specified");

                dimension = dim;
            }
            if (sutXml.Item is LevelsXml)
            {
                var dim = ((LevelsXml)sutXml.Item).Dimension;
                if (string.IsNullOrEmpty(dim))
                    throw new ArgumentException("Dimension must be specified");

                dimension = dim;
                var hie = ((LevelsXml)sutXml.Item).Hierarchy;
                if (string.IsNullOrEmpty(hie))
                    throw new ArgumentException("Hierarchy must be specified");

                hierarchy = hie;
            }
            if (sutXml.Item is TablesXml)
            {
                var persp = ((TablesXml)sutXml.Item).Perspective;
                if (string.IsNullOrEmpty(persp))
                    throw new ArgumentException("Perspective must be specified");

                perspective = persp;
            }
            if (sutXml.Item is ColumnsXml)
            {
                var tab = ((ColumnsXml)sutXml.Item).Table;
                if (string.IsNullOrEmpty(tab))
                    throw new ArgumentException("Table must be specified");
            }

            var target = GetTarget(sutXml.Item);

            return discoveryFactory.Build
                (
                    sutXml.Item.GetConnectionString(),
                    target,
                    perspective,
                    measuregroup,
                    displayFolder,
                    measure,
                    dimension,
                    hierarchy,
                    level,
                    table,
                    column
                );
        }

        private DiscoveryTarget GetTarget(AbstractItem item)
        {
            if (item is MeasuresXml)
                return DiscoveryTarget.Measures;
            if (item is MeasureGroupsXml)
                return DiscoveryTarget.MeasureGroups;
            if (item is LevelsXml)
                return DiscoveryTarget.Levels;
            if (item is HierarchiesXml)
                return DiscoveryTarget.Hierarchies;
            if (item is DimensionsXml)
                return DiscoveryTarget.Dimensions;
            if (item is PerspectivesXml)
                return DiscoveryTarget.Perspectives;

            throw new ArgumentException();

        }

    }
}
