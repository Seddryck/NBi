using NBi.Core.Analysis.Metadata;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.Analysis.Metadata
{
    [TestFixture]
    public class MetadataAdomdExtractorTest
    {
        [Test]
        public void GetMetadata_ExistingCube_ListOfMetadata()
        {
            var me = new MetadataAdomdExtractor("Data Source=localhost;Catalog='Finances Analysis';");
            
            var metadata = me.GetMetadata();

            Assert.That(metadata.Perspectives["Finances"].Dimensions.ContainsKey("[Date]"));
            Assert.That(!metadata.Perspectives["Finances"].Dimensions.ContainsKey("[Measures]"));

            Assert.That(metadata.Perspectives["Finances"].Dimensions["[Date]"].Hierarchies.ContainsKey("[Date].[Calendar]"));

            Assert.That("[Date].[Calendar].[Month]" ,Is.EqualTo(metadata.Perspectives["Finances"].Dimensions["[Date]"].Hierarchies["[Date].[Calendar]"].Levels[2].UniqueName));

            Assert.That(metadata.Perspectives["Finances"].MeasureGroups.ContainsKey("Fact Amount"));
            Assert.That(!metadata.Perspectives["Finances"].MeasureGroups.ContainsKey("[Date]"));

            Assert.That(metadata.Perspectives["Finances"].MeasureGroups["Fact Amount"].LinkedDimensions.ContainsKey("[Date]"));
            Assert.That(!metadata.Perspectives["Finances"].MeasureGroups["Fact Amount"].LinkedDimensions.ContainsKey("[Measures]"));

            Assert.That(metadata.Perspectives["Finances"].MeasureGroups["Fact Amount"].Measures.ContainsKey("[Measures].[Amount]"));
        }

        

    }
}
