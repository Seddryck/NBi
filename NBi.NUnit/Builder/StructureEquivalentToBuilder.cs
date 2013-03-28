using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class StructureEquivalentToBuilder: AbstractStructureBuilder
    {
        protected EquivalentToXml ConstraintXml { get; set; }
        
        public StructureEquivalentToBuilder() : base()
        {
        }

        internal StructureEquivalentToBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is EquivalentToXml))
                throw new ArgumentException("Constraint must be a 'EquivalentToXml'");

            ConstraintXml = (EquivalentToXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(EquivalentToXml ctrXml)
        {
            var ctr = new NBi.NUnit.Structure.EquivalentToConstraint(ctrXml.Items);
        
            //Ignore-case if requested
            if (ctrXml.IgnoreCase)
                ctr = ctr.IgnoreCase;

            return ctr;
        }

        protected override object InstantiateSystemUnderTest(StructureXml sutXml)
        {
            string perspective = null, measuregroup = null, displayFolder = null, measure = null, dimension = null, hierarchy = null, level = null;

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

            var target = GetTarget(sutXml.Item);

            return discoveryFactory.Build
                (
                    sutXml.GetConnectionString(),
                    target,
                    perspective,
                    measuregroup,
                    displayFolder,
                    measure,
                    dimension,
                    hierarchy,
                    level
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
