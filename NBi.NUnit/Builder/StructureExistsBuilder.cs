using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Structure;

namespace NBi.NUnit.Builder
{
    class StructureExistsBuilder : AbstractStructureBuilder
    {
        protected ExistsXml ConstraintXml {get; set;}

        public StructureExistsBuilder() : base()
        {
        }

        internal StructureExistsBuilder(StructureDiscoveryFactoryProvider discoveryProvider)
            : base(discoveryProvider)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is ExistsXml))
                throw new ArgumentException("Constraint must be a 'ExistsXml'");

            if (!(sutXml is StructureXml))
                throw new ArgumentException("System-under-test must be a 'StructureXml'");

            SystemUnderTestXml = (StructureXml)sutXml;

            ConstraintXml = (ExistsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml, SystemUnderTestXml);
        }

        protected NBiConstraint InstantiateConstraint(ExistsXml ctrXml, StructureXml sutXml)
        {
            var expected = sutXml.Item.Caption;

            var ctr = new ExistsConstraint(expected);
            //Ignore-case if requested
            if (ctrXml.IgnoreCase)
                ctr = ctr.IgnoreCase;
            return ctr;
        }

    }
}
