using System;
using System.Linq;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Structure;

namespace NBi.NUnit.Builder
{
    class StructureSubsetOfBuilder: StructureContainBuilder
    {
        protected new SubsetOfXml ConstraintXml { get; set; }
        
        public StructureSubsetOfBuilder() : base()
        {
        }

        internal StructureSubsetOfBuilder(StructureDiscoveryFactoryProvider provider)
            : base(provider)
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

        protected NBiConstraint InstantiateConstraint(SubsetOfXml ctrXml)
        {
            var ctr = new NBi.NUnit.Structure.SubsetOfConstraint(ctrXml.GetItems());

                //Ignore-case if requested
                if (ctrXml.IgnoreCase)
                    ctr = ctr.IgnoreCase;
            return ctr;
        }
    }
}
