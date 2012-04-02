using NBi.Core.Analysis.Metadata;
using NUnit.Framework;

namespace NBi.Testing.Core.Analysis.Metadata
{
    [TestFixture]
    public class MetadataExtractorTest
    {
        [Test]
        public void GetMetadata_ExistingCube_ListOfMetadata()
        {
            var me = new MetadataExtractor("Data Source=localhost;Catalog='Finances Analysis';", "Finances");
            
            me.GetMetadata();

            Assert.That(me.Dimensions.ContainsKey("[Date]"));
            Assert.That(!me.Dimensions.ContainsKey("[Measures]"));

            Assert.That(me.Dimensions["[Date]"].Hierarchies.ContainsKey("[Date].[Calendar]"));
       
            Assert.That(me.MeasureGroups.ContainsKey("Fact Amount"));
            Assert.That(!me.MeasureGroups.ContainsKey("[Date]"));

            Assert.That(me.MeasureGroups["Fact Amount"].LinkedDimensions.ContainsKey("[Date]"));
            Assert.That(!me.MeasureGroups["Fact Amount"].LinkedDimensions.ContainsKey("[Measures]"));

            Assert.That(me.MeasureGroups["Fact Amount"].Measures.ContainsKey("[Measures].[Amount]"));
        }

        

    }
}
