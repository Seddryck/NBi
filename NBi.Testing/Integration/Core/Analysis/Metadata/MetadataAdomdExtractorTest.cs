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

            //Assert.That(metadata.Perspectives["Adventure Works"].MeasureGroups["Financial Reporting"].LinkedDimensions.ContainsKey("[Date]"));
            //Assert.That(!metadata.Perspectives["Adventure Works"].MeasureGroups["Financial Reporting"].LinkedDimensions.ContainsKey("[Measures]"));

            Assert.That(metadata.Perspectives["Adventure Works"].MeasureGroups["Financial Reporting"].Measures.ContainsKey("[Measures].[Amount]"));
        }       

    }
}
