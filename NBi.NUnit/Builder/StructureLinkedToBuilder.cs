using System;
using System.Linq;
using NBi.NUnit.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Structure;
using System.Collections.Generic;

namespace NBi.NUnit.Builder
{
    class StructureLinkedToBuilder : AbstractStructureBuilder
    {
        protected LinkedToXml ConstraintXml { get; set; }

        public StructureLinkedToBuilder() : base()
        {
        }

        internal StructureLinkedToBuilder(StructureDiscoveryFactoryProvider discoveryProvider)
            : base(discoveryProvider)
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

        protected NBiConstraint InstantiateConstraint(LinkedToXml ctrXml)
        {
            var ctr = new LinkedToConstraint(ctrXml.Item.Caption);
            return ctr;
        }

        protected override StructureDiscoveryCommand InstantiateCommand(AbstractItem item)
        {
            var factory = discoveryProvider.Instantiate(item.GetConnectionString());

            var target = BuildTarget(item);
            var filters = BuildFilters(item);

            var command = factory.Instantiate(target, TargetType.Relation, filters);
            return command;
        }


    }
}
