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

            Assert.That(metadata.Perspectives["Finances"].Dimensions.ContainsKey("[Date]"));
            Assert.That(!metadata.Perspectives["Finances"].Dimensions.ContainsKey("[Measures]"));

            Assert.That(metadata.Perspectives["Finances"].Dimensions["[Date]"].Hierarchies.ContainsKey("[Date].[Calendar]"));

            Assert.That(metadata.Perspectives["Finances"].Dimensions["[Date]"].Hierarchies["[Date].[Calendar]"].Levels.ContainsKey("[Date].[Calendar].[Month]"));

            Assert.That(metadata.Perspectives["Finances"]
                .Dimensions["[Date]"]
                .Hierarchies["[Date].[Calendar]"]
                .Levels["[Date].[Calendar].[Month]"]
                .Properties.ContainsKey("[Date].[Calendar].[Month].[Calendar Year]"));

            Assert.That(metadata.Perspectives["Finances"].MeasureGroups.ContainsKey("Fact Amount"));
            Assert.That(!metadata.Perspectives["Finances"].MeasureGroups.ContainsKey("[Date]"));

            Assert.That(metadata.Perspectives["Finances"].MeasureGroups["Fact Amount"].LinkedDimensions.ContainsKey("[Date]"));
            Assert.That(!metadata.Perspectives["Finances"].MeasureGroups["Fact Amount"].LinkedDimensions.ContainsKey("[Measures]"));

            Assert.That(metadata.Perspectives["Finances"].MeasureGroups["Fact Amount"].Measures.ContainsKey("[Measures].[Amount]"));
        }

        public void GetChildStructure_DateDimensionWithThreeHierarchies_ListStructureContainingThreeElements()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            disco.Perspective = "Finances";
            disco.Path = "[Date]";

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs, Has.Count.EqualTo(3));
        }

        public void GetChildStructure_CalendarHierarchyWithFourLevels_ListStructureContainingFourElements()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            disco.Perspective = "Finances";
            disco.Path = "[Date].[Calendar]";

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs, Has.Count.EqualTo(4));
        }

        public void GetChildStructure_MonthLevelWithTwoProperties_ListStructureContainingTwoElements()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            disco.Perspective = "Finances";
            disco.Path = "[Date].[Calendar].[Month]";

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs, Has.Count.EqualTo(2));
        }

        public void GetMeasures_PerspectiveEasyFinances_ListMeasuresContainingTwoElements()
        {
            var me = new MetadataAdomdExtractor(ConnectionStringReader.GetAdomd());
            var disco = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            disco.Perspective = "Easy finances";
            disco.Path = "[Measures].[Fact Amount]";

            var structs = me.GetPartialMetadata(disco);

            Assert.That(structs, Has.Count.EqualTo(1));
        }

        

    }
}
