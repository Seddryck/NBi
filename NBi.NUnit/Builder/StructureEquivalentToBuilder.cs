using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Structure;

namespace NBi.NUnit.Builder
{
    class StructureEquivalentToBuilder: AbstractStructureBuilder
    {
        protected EquivalentToXml ConstraintXml { get; set; }
        
        public StructureEquivalentToBuilder() : base()
        {
        }

        internal StructureEquivalentToBuilder(StructureDiscoveryFactoryProvider discoveryProvider)
            : base(discoveryProvider)
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

        protected NBiConstraint InstantiateConstraint(EquivalentToXml ctrXml)
        {
            var ctr = new NBi.NUnit.Structure.EquivalentToConstraint(ctrXml.GetItems());
        
            //Ignore-case if requested
            if (ctrXml.IgnoreCase)
                ctr = ctr.IgnoreCase;

            return ctr;
        }

    }
}
