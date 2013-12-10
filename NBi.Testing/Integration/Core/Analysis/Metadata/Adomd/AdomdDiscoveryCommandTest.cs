using System.Collections.Generic;
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
            var disco = new DiscoveryRequestFactory().BuildDirect(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.Hierarchies,
                new List<IFilter>() { 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions)
                });

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(18));
        }

        [Test]
        public void Execute_TabularDateDimensionWithHeighTeenHierarchies_ListStructureContainingSevenTeenElements()
        {
            var disco = new DiscoveryRequestFactory().BuildDirect(
                ConnectionStringReader.GetAdomdTabular(),
                DiscoveryTarget.Hierarchies,
                new List<IFilter>() { 
                    new CaptionFilter("Internet Operation", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions)
                });

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(17));
        }

        [Test]
        public void GetPartialMetadata_CalendarHierarchyWithSixLevels_ListStructureContainingSixElements()
        {
            var disco = new DiscoveryRequestFactory().BuildDirect(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.Levels,
                new List<IFilter>() { 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies)
                });

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(6));
        }

        [Test]
        public void GetPartialMetadata_TabularCalendarHierarchyWithSixLevels_ListStructureContainingSixElements()
        {
            var disco = new DiscoveryRequestFactory().BuildDirect(
                ConnectionStringReader.GetAdomdTabular(),
                DiscoveryTarget.Levels,
                new List<IFilter>() { 
                    new CaptionFilter("Internet Operation", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies)
                });

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(6));
        }

        [Test]
        public void GetPartialMetadata_MonthLevelWithTwoProperties_ListStructureContainingTwoElements()
        {
            var disco = new DiscoveryRequestFactory().BuildDirect(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.Properties,
                new List<IFilter>() { 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies),
                    new CaptionFilter("Month", DiscoveryTarget.Levels)
                });

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetPartialMetadata_TabularMonthLevelWithTwoProperties_ListStructureContainingNoElement()
        {
            var disco = new DiscoveryRequestFactory().BuildDirect(
                ConnectionStringReader.GetAdomdTabular(),
                DiscoveryTarget.Properties,
                new List<IFilter>() { 
                    new CaptionFilter("Internet Operations", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies),
                    new CaptionFilter("Month", DiscoveryTarget.Levels)
                });

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetPartialMetadata_MeasureGroupsForCubeFinance_OneElement()
        {
            var disco = new DiscoveryRequestFactory().BuildDirect(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.MeasureGroups,
                new List<IFilter>() { 
                    new CaptionFilter("Finance", DiscoveryTarget.Perspectives),
                });

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetPartialMetadata_TabularMeasureGroupsForInternetOperation_ThreeElements()
        {
            var disco = new DiscoveryRequestFactory().BuildDirect(
                ConnectionStringReader.GetAdomdTabular(),
                DiscoveryTarget.MeasureGroups,
                new List<IFilter>() { 
                    new CaptionFilter("Internet Operation", DiscoveryTarget.Perspectives),
                });

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Execute_DateDimensionLinkedToElevenMeasureGroups_ListStructureContainingTenElements()
        {
            var disco = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , new List<IFilter>() { 
                            new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Customer", DiscoveryTarget.Dimensions)
                        });

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(10));
        }

        [Test]
        [Ignore]
        public void Execute_TabularDateDimensionLinkedToThreeMeasureGroups_ListStructureContainingThreeElements()
        {
            var disco = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , new List<IFilter>() { 
                            new CaptionFilter("Internet Operation", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Date", DiscoveryTarget.Dimensions)
                        });

            var factory = new AdomdDiscoveryCommandFactory();
            var cmd = factory.BuildExact(disco);

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(3));
        }
    }


}
