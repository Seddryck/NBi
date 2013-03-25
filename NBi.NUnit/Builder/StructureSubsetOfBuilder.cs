using System;
using System.Linq;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class StructureSubsetOfBuilder: StructureContainsBuilder
    {
        protected new SubsetOfXml ConstraintXml { get; set; }
        
        public StructureSubsetOfBuilder() : base()
        {
        }

        internal StructureSubsetOfBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SubsetOfXml))
                throw new ArgumentException("Constraint must be a 'SubsetOfXml'");

            ConstraintXml = (SubsetOfXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(SubsetOfXml ctrXml)
        {
            var ctr = new NBi.NUnit.Structure.SubsetOfConstraint(ctrXml.Items);

                //Ignore-case if requested
                if (ctrXml.IgnoreCase)
                    ctr = ctr.IgnoreCase;
            return ctr;
        }
    }
}
