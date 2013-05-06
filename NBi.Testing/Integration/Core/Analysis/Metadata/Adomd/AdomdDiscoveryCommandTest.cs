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
        public void Execute_TabularDateDimensionWithHeighTeenHierarchies_ListStructureContainingSevenTeenElements()
        {
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomdTabular(),
                DiscoveryTarget.Hierarchies,
                "Internet Operation",
                null, null, null,
                "Date", null, null);

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(17));
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

        [Test]
        public void GetPartialMetadata_TabularCalendarHierarchyWithSixLevels_ListStructureContainingSixElements()
        {
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomdTabular(),
                DiscoveryTarget.Levels,
                "Internet Operation",
                null, null, null,
                "Date", "Calendar", null);

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(6));
        }

        [Test]
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
        public void GetPartialMetadata_TabularMonthLevelWithTwoProperties_ListStructureContainingNoElement()
        {
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomdTabular(),
                DiscoveryTarget.Properties,
                "Internet Operation",
                null, null, null,
                "Date", "Calendar", "Month");

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetPartialMetadata_MeasureGroupsForCubeFinance_OneElement()
        {
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.MeasureGroups,
                "Finance",
                null, null, null,
                null, null, null);

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetPartialMetadata_TabularMeasureGroupsForInternetOperation_ThreeElements()
        {
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomdTabular(),
                DiscoveryTarget.MeasureGroups,
                "Internet Operation",
                null, null, null,
                null, null, null);

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Execute_DateDimensionLinkedToElevenMeasureGroups_ListStructureContainingTenElements()
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

            Assert.That(structs.Count(), Is.EqualTo(10));
        }

        [Test]
        [Ignore]
        public void Execute_TabularDateDimensionLinkedToThreeMeasureGroups_ListStructureContainingThreeElements()
        {
            var disco = new DiscoveryRequestFactory().BuildLinkedTo(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.MeasureGroups,
                "Internet Operation",
                null,
                "Date");

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(3));
        }
    }


}
