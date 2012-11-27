using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class StructureExistsBuilder : AbstractStructureBuilder
    {
        protected ExistsXml ConstraintXml {get; set;}

        public StructureExistsBuilder() : base()
        {
        }

        internal StructureExistsBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is ExistsXml))
                throw new ArgumentException("Constraint must be a 'ExistsXml'");

            ConstraintXml = (ExistsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(ExistsXml ctrXml)
        {
            var ctr = new ExistsConstraint();
            return ctr;
        }

        protected override object InstantiateSystemUnderTest(StructureXml sutXml)
        {
            string perspective = null, measuregroup = null, measure = null, dimension = null, hierarchy = null, level = null;
            DiscoveryTarget target = DiscoveryTarget.Undefined;
            
            if (sutXml.Item is PerspectiveXml)
            {
                perspective = sutXml.Item.Caption;
                target = DiscoveryTarget.Perspectives;
            }
            if (sutXml.Item is MeasureGroupXml)
            {
                perspective = ((MeasureGroupXml)sutXml.Item).Perspective;
                measuregroup = sutXml.Item.Caption;
                target = DiscoveryTarget.MeasureGroups;
            }
            if (sutXml.Item is MeasureXml)
            {
                measuregroup = null;
                measure = sutXml.Item.Caption;
                target = DiscoveryTarget.Measures;
            }
            if (sutXml.Item is DimensionXml)
            {
                perspective =((DimensionXml)sutXml.Item).Perspective;
                dimension = sutXml.Item.Caption;
                target = DiscoveryTarget.Dimensions;
            }
            if (sutXml.Item is HierarchyXml)
            {
                dimension =((HierarchyXml)sutXml.Item).Dimension;
                hierarchy = sutXml.Item.Caption;
                target = DiscoveryTarget.Hierarchies;
            }
            if (sutXml.Item is LevelXml)
            {
                hierarchy = ((LevelXml)sutXml.Item).Hierarchy;
                level = sutXml.Item.Caption;
                target = DiscoveryTarget.Levels;
            }

            if (target.Equals(DiscoveryTarget.Undefined))
                throw new ArgumentOutOfRangeException();

            var disco = discoveryFactory.Build(
                    sutXml.GetConnectionString(),
                    target,
                    perspective,
                    measuregroup,
                    measure,
                    dimension,
                    hierarchy,
                    level
                );
            return disco;
        }

    }
}
