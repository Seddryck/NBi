using NBi.Core.Analysis;
using NBi.Core.Analysis.Metadata;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.Core.Analysis.Metadata
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
            var disco = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            disco.Perspective = "Adventure Works";
            disco.Path = "[Date]";

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs, Has.Count.EqualTo(18));
        }

        public void GetChildStructure_CalendarHierarchyWithSixLevels_ListStructureContainingSixElements()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            disco.Perspective = "Adventure Works";
            disco.Path = "[Date].[Calendar]";

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs, Has.Count.EqualTo(6));
        }

        public void GetChildStructure_MonthLevelWithTwoProperties_ListStructureContainingTwoElements()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            disco.Perspective = "Adventure Works";
            disco.Path = "[Date].[Calendar].[Month]";

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs, Has.Count.EqualTo(2));
        }

        public void GetMeasuresOfFolder_PerspectiveAdventureWorks_OneElement()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            disco.Perspective = "Finance";
            disco.Path = "[Measures].[Financial Reporting]"; //Financial Reporting is the folder's name!

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs, Has.Count.EqualTo(1));
        }

        

    }
}
