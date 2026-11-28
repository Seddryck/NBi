using System;
using System.Linq;
using NBi.Model.Xml.Constraints;
using NBi.Model.Xml.Items;
using NBi.Model.Xml.Systems;
using NBi.Core.Structure;

namespace NBi.NUnit.Builder
{
    class StructureContainedInBuilder: StructureContainBuilder
    {
        protected new ContainedInXml ConstraintXml { get; set; }

        public StructureContainedInBuilder()
            : base()
        {
        }

        internal StructureContainedInBuilder(StructureDiscoveryFactoryProvider provider)
            : base(provider)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is ContainedInXml))
                throw new ArgumentException("Constraint must be a 'SubsetOfXml'");

            ConstraintXml = (ContainedInXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected NBiConstraint InstantiateConstraint(ContainedInXml ctrXml)
        {
            var ctr = new NBi.NUnit.Structure.ContainedInConstraint(ctrXml.GetItems());

                //Ignore-case if requested
            if (ctrXml.IgnoreCase)
                    ctr = ctr.IgnoreCase;
            return ctr;
        }
    }
}
