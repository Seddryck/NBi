using System;
using System.Linq;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Analysis.Metadata.Adomd
{
    public class SetDiscoveryCommandTest
    {
        [Test]
        [Category("Olap")]
        public void Execute_OnPerspectiveNamedChannelSales_ListStructureContainingFourSets()
        {
            var request = new MetadataDiscoveryRequest();
            request.SpecifyFilter(new CaptionFilter("Channel Sales", DiscoveryTarget.Perspectives));

            var disco = new SetDiscoveryCommand(ConnectionStringReader.GetAdomd());
            disco.Filters = request.GetAllFilters();

            var structs = disco.Execute();

            Assert.That(structs.Count(), Is.EqualTo(4));
        }

    }
}
