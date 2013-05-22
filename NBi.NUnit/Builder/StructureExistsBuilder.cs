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

        internal StructureExistsBuilder(MetadataDiscoveryRequestBuilder factory)
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

    }
}
