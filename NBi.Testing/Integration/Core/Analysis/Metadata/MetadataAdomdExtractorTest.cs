using NBi.Core.Analysis.Discovery;
using NBi.Core.Analysis.Metadata;
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

        public void GetChildStructure_DateDimensionWithHeighTeenHierarchies_ListStructureContainingHeighTeenElements()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = DiscoveryFactory.BuildForHierarchy(
                ConnectionStringReader.GetAdomd(),
                "Adventure Works",
                "[Date]");

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs, Has.Count.EqualTo(18));
        }

        public void GetChildStructure_CalendarHierarchyWithSixLevels_ListStructureContainingSixElements()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = DiscoveryFactory.BuildForLevel(
                ConnectionStringReader.GetAdomd(),
                "Adventure Works",
                "[Date].[Calendar]");

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs, Has.Count.EqualTo(6));
        }

        public void GetChildStructure_MonthLevelWithTwoProperties_ListStructureContainingTwoElements()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            //var disco = DiscoveryFactory.BuildForProperty(
            //    ConnectionStringReader.GetAdomd(),
            //    "Adventure Works",
            //    "[Date].[Calendar].[Month]");

            //var structs = me.GetPartialMetadata(disco);

            //Assert.That(structs, Has.Count.EqualTo(2));
            Assert.Fail();
        }

        public void GetMeasuresOfFolder_PerspectiveAdventureWorks_OneElement()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = DiscoveryFactory.BuildForMeasureGroup(
                ConnectionStringReader.GetAdomd(),
                "Finances",
                "Financial Reporting");

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs, Has.Count.EqualTo(1));
        }

        

    }
}
