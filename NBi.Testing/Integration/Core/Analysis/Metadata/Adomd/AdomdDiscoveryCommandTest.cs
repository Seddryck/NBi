using System.Linq;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Analysis.Metadata.Adomd
{
    [TestFixture]
    public class AdomdDiscoveryCommandTest
    {
        [Test]
        public void Execute_DateDimensionWithHeighTeenHierarchies_ListStructureContainingHeighTeenElements()
        {
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.Hierarchies,
                "Adventure Works",
                null, null, null,
                "Date", null, null);

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(18));
        }

        [Test]
        public void GetPartialMetadata_CalendarHierarchyWithSixLevels_ListStructureContainingSixElements()
        {
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.Levels,
                "Adventure Works",
                null, null, null,
                "Date", "Calendar", null);

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(6));
        }

        public void GetPartialMetadata_MonthLevelWithTwoProperties_ListStructureContainingTwoElements()
        {
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.Properties,
                "Adventure Works",
                null, null, null,
                "Date", "Calendar", "Month");

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetPartialMetadata_MeasureGroupFinancialReporting_OneElement()
        {
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.MeasureGroups,
                "Finance",
                "Financial Reporting", null, null,
                null, null, null);

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_DateDimensionLinkedToElevenMeasureGroups_ListStructureContainingElevenElements()
        {
            var disco = new DiscoveryRequestFactory().BuildLinkedTo(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.MeasureGroups,
                "Adventure Works",
                null,
                "Date");

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(11));
        }
    }


}
