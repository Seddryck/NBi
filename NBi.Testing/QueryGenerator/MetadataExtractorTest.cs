using NBi.QueryGenerator;
using NUnit.Framework;

namespace NBi.Testing.QueryGenerator
{
    [TestFixture]
    public class MetadataExtractorTest
    {
        [Test]
        public void GetDimensions_ExistingCube_ListOfDimensions()
        {
            var me = new MetadataExtractor("Data Source=localhost;Catalog='Finances Analysis';", "Finances");
            
            me.GetDimensions();
            me.GetDimensionUsage();
            me.GetMeasures();

            var mb = new MdxBuilder(@"C:\Users\Seddryck\Documents\TestCCH\");
            mb.Build("Finances", me.MeasureGroups, "Children", "");
            Assert.Pass();
        }
    }
}
