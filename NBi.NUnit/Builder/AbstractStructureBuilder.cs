using System;
using System.Linq;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    abstract class AbstractStructureBuilder : AbstractTestCaseBuilder
    {
        protected StructureXml SystemUnderTestXml { get; set; }
        protected readonly MetadataDiscoveryRequestBuilder discoveryFactory;

        public AbstractStructureBuilder()
        {
            discoveryFactory = new MetadataDiscoveryRequestBuilder();
        }

        internal AbstractStructureBuilder(MetadataDiscoveryRequestBuilder factory)
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

        protected virtual object InstantiateSystemUnderTest(StructureXml sutXml)
        {
            return InstantiateCommand(sutXml.Item);
        }

        protected virtual MetadataDiscoveryRequest InstantiateCommand(AbstractItem item)
        {
            var request = discoveryFactory.Build(item, MetadataDiscoveryRequestBuilder.MetadataDiscoveryRequestType.Direct);
            return request;
        }

    }
}
