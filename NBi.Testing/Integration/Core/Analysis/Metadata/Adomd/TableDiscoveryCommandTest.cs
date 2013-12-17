using System;
using System.Linq;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Analysis.Metadata.Adomd
{
    public class TableDiscoveryCommandTest
    {
        [Test]
        public void Execute_OnPerspectiveNamedInternetOperation_ListStructureContainingTenTables()
        {
            var request = new MetadataDiscoveryRequest();
            request.SpecifyFilter(new CaptionFilter("Internet Operation", DiscoveryTarget.Perspectives));

            var disco = new TableDiscoveryCommand(ConnectionStringReader.GetAdomdTabular());
            disco.Filters = request.GetAllFilters();

            var structs = disco.Execute();

            Assert.That(structs.Count(), Is.EqualTo(10));
        }

    }
}
