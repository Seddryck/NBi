using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class StructureLinkedToBuilder : AbstractStructureBuilder
    {
        protected LinkedToXml ConstraintXml { get; set; }

        public StructureLinkedToBuilder() : base()
        {
        }

        internal StructureLinkedToBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is LinkedToXml))
                throw new ArgumentException("Constraint must be a 'LinkedToXml'");

            ConstraintXml = (LinkedToXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(LinkedToXml ctrXml)
        {
            var ctr = new LinkedToConstraint(ctrXml.Item.Caption);
            return ctr;
        }

        protected override object InstantiateSystemUnderTest(StructureXml sutXml)
        {
            string perspective = null, measuregroup = null, dimension = null;
            DiscoveryTarget target = DiscoveryTarget.Undefined;

            if (!(sutXml.Item is MeasureGroupXml || sutXml.Item is DimensionXml))
                throw new ArgumentOutOfRangeException();

            if (sutXml.Item is MeasureGroupXml)
            {
                perspective = ((MeasureGroupXml)sutXml.Item).Perspective;
                measuregroup = sutXml.Item.Caption;
                target = DiscoveryTarget.Dimensions;
            }

            if (sutXml.Item is DimensionXml)
            {
                perspective =((DimensionXml)sutXml.Item).Perspective;
                dimension = sutXml.Item.Caption;
                target = DiscoveryTarget.MeasureGroups;
            }

            var disco = discoveryFactory.BuildLinkedTo(
                    sutXml.Item.GetConnectionString(),
                    target,
                    perspective,
                    measuregroup,
                    dimension
                );
            return disco;
        }

    }
}
