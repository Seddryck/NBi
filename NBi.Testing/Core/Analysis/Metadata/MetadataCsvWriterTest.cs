using System.IO;
using NUnit.Framework;
using NBi.Core.Analysis.Metadata;

namespace NBi.Testing.Core.Analysis.Metadata
{
    [TestFixture]
    public class MetadataCsvWriterTest
    {
        [Test]
        public void Write_NotExistingFile_FileIsCorrectlyBuilt()
        {
            var expectedFilename = DiskOnFile.CreatePhysicalFile("ExpectedCSV.csv", "NBi.Testing.Core.Analysis.Metadata.Resources.MetadataCsvSample.csv");
            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");
            var persp = BuildFakeMetadata();

            //set the object to test
            var mcw = new MetadataCsvWriter(filename);
            mcw.Write(persp);

            //Assertion
            FileAssert.AreEqual(expectedFilename, filename);
        }

        private CubeMetadata BuildFakeMetadata()
        {
            var metadata = new CubeMetadata();
            
            var p = new Perspective("p");
            
            var mg = new MeasureGroup("mg");

            var h1 = new Hierarchy("[h1]", "h1");
            var h2 = new Hierarchy("[h2]", "h2");

            var hs = new HierarchyCollection();
            hs.Add(h1);
            hs.Add(h2);

            var d = new Dimension("[d]", "d", hs);
            mg.LinkedDimensions.Add(d);

            var m1 = new Measure("[m1]", "m1");
            var m2 = new Measure("[m2]", "m2");
            mg.Measures.Add(m1);
            mg.Measures.Add(m2);

            p.MeasureGroups.Add(mg);

            metadata.Perspectives.Add(p);
            return metadata;
        }
    }
}
