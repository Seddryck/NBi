using System.Linq;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Request;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Analysis.Metadata
{
    [TestFixture]
    public class MetadataAdomdExtractorTest
    {
        [Test]
        public void GetMetadata_ExistingCube_ListOfMetadata()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            
            var metadata = me.GetFullMetadata();

            Assert.That(metadata.Perspectives["Adventure Works"].Dimensions.ContainsKey("[Date]"));
            Assert.That(!metadata.Perspectives["Adventure Works"].Dimensions.ContainsKey("[Measures]"));

            Assert.That(metadata.Perspectives["Adventure Works"].Dimensions["[Date]"].Hierarchies.ContainsKey("[Date].[Calendar]"));

            Assert.That(metadata.Perspectives["Adventure Works"].Dimensions["[Date]"].Hierarchies["[Date].[Calendar]"].Levels.ContainsKey("[Date].[Calendar].[Month]"));

            Assert.That(metadata.Perspectives["Adventure Works"]
                .Dimensions["[Date]"]
                .Hierarchies["[Date].[Calendar]"]
                .Levels["[Date].[Calendar].[Month]"]
                .Properties.ContainsKey("[Date].[Calendar].[Month].[Calendar Quarter]"));

            Assert.That(metadata.Perspectives["Adventure Works"].MeasureGroups.ContainsKey("Financial Reporting"));
            Assert.That(!metadata.Perspectives["Adventure Works"].MeasureGroups.ContainsKey("[Date]"));

            Assert.That(metadata.Perspectives["Adventure Works"].MeasureGroups["Financial Reporting"].LinkedDimensions.ContainsKey("[Date]"));
            Assert.That(!metadata.Perspectives["Adventure Works"].MeasureGroups["Financial Reporting"].LinkedDimensions.ContainsKey("[Measures]"));

            Assert.That(metadata.Perspectives["Adventure Works"].MeasureGroups["Financial Reporting"].Measures.ContainsKey("[Measures].[Amount]"));
        }

        [Test]
        public void GetPartialMetadata_DateDimensionWithHeighTeenHierarchies_ListStructureContainingHeighTeenElements()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.Hierarchies,
                "Adventure Works",
                null, null,
                "Date", null, null);

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs.Count(), Is.EqualTo(18));
        }

        [Test]
        public void GetPartialMetadata_CalendarHierarchyWithSixLevels_ListStructureContainingSixElements()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.Levels,
                "Adventure Works",
                null, null,
                "Date", "Calendar", null);

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs.Count(), Is.EqualTo(6));
        }

        public void GetPartialMetadata_MonthLevelWithTwoProperties_ListStructureContainingTwoElements()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.Properties,
                "Adventure Works",
                null, null,
                "Date", "Calendar", "Month");

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetPartialMetadata_MeasureGroupFinancialReporting_OneElement()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                DiscoveryTarget.MeasureGroups,
                "Finance",
                "Financial Reporting", null, 
                null, null, null);

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs.Count(), Is.EqualTo(1));
        }

        

    }
}
