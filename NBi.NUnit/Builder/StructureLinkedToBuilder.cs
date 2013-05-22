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

        internal StructureLinkedToBuilder(MetadataDiscoveryRequestBuilder factory)
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

        protected override MetadataDiscoveryRequest InstantiateCommand(AbstractItem item)
        {
            var request = discoveryFactory.Build(item, MetadataDiscoveryRequestBuilder.MetadataDiscoveryRequestType.Relation);
            return request;
        }

    }
}
