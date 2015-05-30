using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Structure;

namespace NBi.NUnit.Builder
{
    class StructureContainBuilder: AbstractStructureBuilder
    {
        protected ContainXml ConstraintXml {get; set;}
        
        public StructureContainBuilder() : base()
        {
        }

        internal StructureContainBuilder(StructureDiscoveryFactoryProvider provider)
            : base(provider)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is ContainXml))
                throw new ArgumentException("Constraint must be a 'ContainXml'");

            ConstraintXml = (ContainXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected NBiConstraint InstantiateConstraint(ContainXml ctrXml)
        {
            var ctr = new NBi.NUnit.Structure.ContainConstraint(ctrXml.GetItems());
            //Ignore-case if requested
            if (ctrXml.IgnoreCase)
                ctr = ctr.IgnoreCase;
            return ctr;
        }


    }
}
