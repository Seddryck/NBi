using System;
using System.Linq;
using NBi.Core.Analysis.Discovery;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    abstract class AbstractStructureBuilder : AbstractTestCaseBuilder
    {
        protected StructureXml SystemUnderTestXml { get; set; }
        protected readonly DiscoveryFactory discoveryFactory;

        public AbstractStructureBuilder()
        {
            discoveryFactory = new DiscoveryFactory();
        }

        internal AbstractStructureBuilder(DiscoveryFactory factory)
        {
            discoveryFactory = factory;
        }

        protected override void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(sutXml is StructureXml))
                throw new ArgumentException("Constraint must be a 'StructureXml'");

            SystemUnderTestXml = (StructureXml)sutXml;
        }

        protected override void BaseBuild()
        {
            SystemUnderTest = InstantiateSystemUnderTest(SystemUnderTestXml);
        }

        protected abstract object InstantiateSystemUnderTest(StructureXml sutXml);

    }
}
