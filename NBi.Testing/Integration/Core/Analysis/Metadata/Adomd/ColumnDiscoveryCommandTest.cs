using System;
using System.Linq;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Analysis.Metadata.Adomd
{
    public class ColumnDiscoveryCommandTest
    {
        [Test]
        [Category("Olap")]
        public void Execute_OnTableNamedCurrency_ListStructureContainingThreeColumns()
        {
            var request = new MetadataDiscoveryRequest();
            request.SpecifyFilter(new CaptionFilter("Internet Operation", DiscoveryTarget.Perspectives));
            request.SpecifyFilter(new CaptionFilter("Currency", DiscoveryTarget.Tables));

            var disco = new ColumnDiscoveryCommand(ConnectionStringReader.GetAdomdTabular());
            disco.Filters = request.GetAllFilters();

            var structs = disco.Execute();

            Assert.That(structs.Count(), Is.EqualTo(3));
        }

    }
}
