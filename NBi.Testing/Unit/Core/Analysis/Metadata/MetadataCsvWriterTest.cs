using System.IO;
using System.Text;
using NBi.Core.Analysis.Metadata;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.Analysis.Metadata
{
    [TestFixture]
    public class MetadataCsvWriterTest
    {
        [Test]
        public void Write_OneElementAtEachStep_FileIsCorrectlyBuilt()
        {
            var header = GetHeader();
            var expectedContent = header + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d\";\"[d]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n";
            var expectedFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ExpectedCSV.csv");
            if (File.Exists(expectedFilename))
                File.Delete(expectedFilename);
            File.AppendAllText(expectedFilename, expectedContent, Encoding.UTF8);

            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");

            var metadata = new CubeMetadata();
            var p = new Perspective("p");
            metadata.Perspectives.Add(p);
            var mg = new MeasureGroup("mg");
            p.MeasureGroups.Add(mg);
            var m1 = new Measure("[m1]", "m1", "df");
            mg.Measures.Add(m1);
            var d = new Dimension("[d]", "d");
            mg.LinkedDimensions.Add(d);
            var h1 = new Hierarchy("[h1]", "h1");
            d.Hierarchies.Add(h1);
            var l1 = new Level("[l1]", "l1", 0);
            h1.Levels.Add(l1);
            var p1 = new Property("[p1]", "p1");
            l1.Properties.Add(p1);
            
            //set the object to test
            var mcw = new MetadataCsvWriter(filename);
            mcw.Write(metadata);

            //Assertion
            FileAssert.AreEqual(expectedFilename, filename);
        }

        [Test]
        public void Write_NoPropertyForTheLevel_FileIsCorrectlyBuilt()
        {
            var header = GetHeader();
            var expectedContent = header + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d\";\"[d]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"\";\"\"\r\n";
            var expectedFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ExpectedCSV.csv");
            if (File.Exists(expectedFilename))
                File.Delete(expectedFilename);
            File.AppendAllText(expectedFilename, expectedContent, Encoding.UTF8);

            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");

            var metadata = new CubeMetadata();
            var p = new Perspective("p");
            metadata.Perspectives.Add(p);
            var mg = new MeasureGroup("mg");
            p.MeasureGroups.Add(mg);
            var m1 = new Measure("[m1]", "m1", "df");
            mg.Measures.Add(m1);
            var d = new Dimension("[d]", "d");
            mg.LinkedDimensions.Add(d);
            var h1 = new Hierarchy("[h1]", "h1");
            d.Hierarchies.Add(h1);
            var l1 = new Level("[l1]", "l1", 0);
            h1.Levels.Add(l1);

            //set the object to test
            var mcw = new MetadataCsvWriter(filename);
            mcw.Write(metadata);

            //Assertion
            FileAssert.AreEqual(expectedFilename, filename);
        }

        [Test]
        public void Write_TwoPropertiesForTheSameLevel_FileIsCorrectlyBuilt()
        {
            var header = GetHeader();
            var expectedContent = header 
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d\";\"[d]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d\";\"[d]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p2\";\"[p2]\"\r\n"
                ;
            var expectedFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ExpectedCSV.csv");
            if (File.Exists(expectedFilename))
                File.Delete(expectedFilename);
            File.AppendAllText(expectedFilename, expectedContent, Encoding.UTF8);

            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");

            var metadata = new CubeMetadata();
            var p = new Perspective("p");
            metadata.Perspectives.Add(p);
            var mg = new MeasureGroup("mg");
            p.MeasureGroups.Add(mg);
            var m1 = new Measure("[m1]", "m1", "df");
            mg.Measures.Add(m1);
            var d = new Dimension("[d]", "d");
            mg.LinkedDimensions.Add(d);
            var h1 = new Hierarchy("[h1]", "h1");
            d.Hierarchies.Add(h1);
            var l1 = new Level("[l1]", "l1", 0);
            h1.Levels.Add(l1);
            var p1 = new Property("[p1]", "p1");
            l1.Properties.Add(p1);
            var p2 = new Property("[p2]", "p2");
            l1.Properties.Add(p2);

            //set the object to test
            var mcw = new MetadataCsvWriter(filename);
            mcw.Write(metadata);

            //Assertion
            FileAssert.AreEqual(expectedFilename, filename);
        }

        [Test]
        public void Write_TwoLevelsSameHierarchy_FileIsCorrectlyBuilt()
        {
            var header = GetHeader();
            var expectedContent = header
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d\";\"[d]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d\";\"[d]\";\"h1\";\"[h1]\";\"l2\";\"[l2]\";\"1\";\"p2\";\"[p2]\"\r\n"
                ;
            var expectedFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ExpectedCSV.csv");
            if (File.Exists(expectedFilename))
                File.Delete(expectedFilename);
            File.AppendAllText(expectedFilename, expectedContent, Encoding.UTF8);

            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");

            var metadata = new CubeMetadata();
            var p = new Perspective("p");
            metadata.Perspectives.Add(p);
            var mg = new MeasureGroup("mg");
            p.MeasureGroups.Add(mg);
            var m1 = new Measure("[m1]", "m1", "df");
            mg.Measures.Add(m1);
            var d = new Dimension("[d]", "d");
            mg.LinkedDimensions.Add(d);
            var h1 = new Hierarchy("[h1]", "h1");
            d.Hierarchies.Add(h1);
            var l1 = new Level("[l1]", "l1", 0);
            h1.Levels.Add(l1);
            var p1 = new Property("[p1]", "p1");
            l1.Properties.Add(p1);
            var l2 = new Level("[l2]", "l2", 1);
            h1.Levels.Add(l2);
            var p2 = new Property("[p2]", "p2");
            l2.Properties.Add(p2);

            //set the object to test
            var mcw = new MetadataCsvWriter(filename);
            mcw.Write(metadata);

            //Assertion
            FileAssert.AreEqual(expectedFilename, filename);
        }

        [Test]
        public void Write_TwoHierarchiesSameDimension_FileIsCorrectlyBuilt()
        {
            var header = GetHeader();
            var expectedContent = header
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h2\";\"[h2]\";\"l2\";\"[l2]\";\"1\";\"p2\";\"[p2]\"\r\n"
                ;
            var expectedFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ExpectedCSV.csv");
            if (File.Exists(expectedFilename))
                File.Delete(expectedFilename);
            File.AppendAllText(expectedFilename, expectedContent, Encoding.UTF8);

            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");

            var metadata = new CubeMetadata();
            var p = new Perspective("p");
            metadata.Perspectives.Add(p);
            var mg = new MeasureGroup("mg");
            p.MeasureGroups.Add(mg);
            var m1 = new Measure("[m1]", "m1", "df");
            mg.Measures.Add(m1);
            var d1 = new Dimension("[d1]", "d1");
            mg.LinkedDimensions.Add(d1);
            var h1 = new Hierarchy("[h1]", "h1");
            d1.Hierarchies.Add(h1);
            var l1 = new Level("[l1]", "l1", 0);
            h1.Levels.Add(l1);
            var p1 = new Property("[p1]", "p1");
            l1.Properties.Add(p1);
            var h2 = new Hierarchy("[h2]", "h2");
            d1.Hierarchies.Add(h2);
            var l2 = new Level("[l2]", "l2", 1);
            h2.Levels.Add(l2);
            var p2 = new Property("[p2]", "p2");
            l2.Properties.Add(p2);

            //set the object to test
            var mcw = new MetadataCsvWriter(filename);
            mcw.Write(metadata);

            //Assertion
            FileAssert.AreEqual(expectedFilename, filename);
        }

        [Test]
        public void Write_TwoMeasuresSameMeasureGroups_FileIsCorrectlyBuilt()
        {
            var header = GetHeader();
            var expectedContent = header
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg\";\"m2\";\"[m2]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                ;
            var expectedFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ExpectedCSV.csv");
            if (File.Exists(expectedFilename))
                File.Delete(expectedFilename);
            File.AppendAllText(expectedFilename, expectedContent, Encoding.UTF8);

            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");

            var metadata = new CubeMetadata();
            var p = new Perspective("p");
            metadata.Perspectives.Add(p);
            var mg = new MeasureGroup("mg");
            p.MeasureGroups.Add(mg);
            var m1 = new Measure("[m1]", "m1", "df");
            mg.Measures.Add(m1);
            var m2 = new Measure("[m2]", "m2", "df");
            mg.Measures.Add(m2);
            var d1 = new Dimension("[d1]", "d1");
            mg.LinkedDimensions.Add(d1);
            var h1 = new Hierarchy("[h1]", "h1");
            d1.Hierarchies.Add(h1);
            var l1 = new Level("[l1]", "l1", 0);
            h1.Levels.Add(l1);
            var p1 = new Property("[p1]", "p1");
            l1.Properties.Add(p1);

            //set the object to test
            var mcw = new MetadataCsvWriter(filename);
            mcw.Write(metadata);

            //Assertion
            FileAssert.AreEqual(expectedFilename, filename);
        }

        [Test]
        public void Write_TwoDimensionsSameMeasureGroup_FileIsCorrectlyBuilt()
        {
            var header = GetHeader();
            var expectedContent = header
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d2\";\"[d2]\";\"h2\";\"[h2]\";\"l2\";\"[l2]\";\"1\";\"p2\";\"[p2]\"\r\n"
                ;
            var expectedFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ExpectedCSV.csv");
            if (File.Exists(expectedFilename))
                File.Delete(expectedFilename);
            File.AppendAllText(expectedFilename, expectedContent, Encoding.UTF8);

            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");

            var metadata = new CubeMetadata();
            var p = new Perspective("p");
            metadata.Perspectives.Add(p);
            var mg = new MeasureGroup("mg");
            p.MeasureGroups.Add(mg);
            var m1 = new Measure("[m1]", "m1", "df");
            mg.Measures.Add(m1);
            var d1 = new Dimension("[d1]", "d1");
            mg.LinkedDimensions.Add(d1);
            var h1 = new Hierarchy("[h1]", "h1");
            d1.Hierarchies.Add(h1);
            var l1 = new Level("[l1]", "l1", 0);
            h1.Levels.Add(l1);
            var p1 = new Property("[p1]", "p1");
            l1.Properties.Add(p1);
            var d2 = new Dimension("[d2]", "d2");
            mg.LinkedDimensions.Add(d2);
            var h2 = new Hierarchy("[h2]", "h2");
            d2.Hierarchies.Add(h2);
            var l2 = new Level("[l2]", "l2", 1);
            h2.Levels.Add(l2);
            var p2 = new Property("[p2]", "p2");
            l2.Properties.Add(p2);

            //set the object to test
            var mcw = new MetadataCsvWriter(filename);
            mcw.Write(metadata);

            //Assertion
            FileAssert.AreEqual(expectedFilename, filename);
        }

        [Test]
        public void Write_TwoMeasureGroupsWithDifferentDimensions_FileIsCorrectlyBuilt()
        {
            var header = GetHeader();
            var expectedContent = header
                + "\"p\";\"mg1\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg2\";\"m2\";\"[m2]\";\"d2\";\"[d2]\";\"h2\";\"[h2]\";\"l2\";\"[l2]\";\"0\";\"p2\";\"[p2]\"\r\n"
                ;
            var expectedFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ExpectedCSV.csv");
            if (File.Exists(expectedFilename))
                File.Delete(expectedFilename);
            File.AppendAllText(expectedFilename, expectedContent, Encoding.UTF8);

            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");

            var metadata = new CubeMetadata();
            var p = new Perspective("p");
            metadata.Perspectives.Add(p);
            var mg1 = new MeasureGroup("mg1");
            p.MeasureGroups.Add(mg1);
            var m1 = new Measure("[m1]", "m1", "df");
            mg1.Measures.Add(m1);
            var d1 = new Dimension("[d1]", "d1");
            mg1.LinkedDimensions.Add(d1);
            var h1 = new Hierarchy("[h1]", "h1");
            d1.Hierarchies.Add(h1);
            var l1 = new Level("[l1]", "l1", 0);
            h1.Levels.Add(l1);
            var p1 = new Property("[p1]", "p1");
            l1.Properties.Add(p1);
            var mg2 = new MeasureGroup("mg2");
            p.MeasureGroups.Add(mg2);
            var m2 = new Measure("[m2]", "m2", "df");
            mg2.Measures.Add(m2);
            var d2 = new Dimension("[d2]", "d2");
            mg2.LinkedDimensions.Add(d2);
            var h2 = new Hierarchy("[h2]", "h2");
            d2.Hierarchies.Add(h2);
            var l2 = new Level("[l2]", "l2", 0);
            h2.Levels.Add(l2);
            var p2 = new Property("[p2]", "p2");
            l2.Properties.Add(p2);

            //set the object to test
            var mcw = new MetadataCsvWriter(filename);
            mcw.Write(metadata);

            //Assertion
            FileAssert.AreEqual(expectedFilename, filename);
        }

        [Test]
        public void Write_TwoMeasureGroupsSharingCommonDimension_FileIsCorrectlyBuilt()
        {
            var header = GetHeader();
            var expectedContent = header
                + "\"p\";\"mg1\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg2\";\"m2\";\"[m2]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                ;
            var expectedFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ExpectedCSV.csv");
            if (File.Exists(expectedFilename))
                File.Delete(expectedFilename);
            File.AppendAllText(expectedFilename, expectedContent, Encoding.UTF8);

            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");

            var metadata = new CubeMetadata();
            var p = new Perspective("p");
            metadata.Perspectives.Add(p);
            var mg1 = new MeasureGroup("mg1");
            p.MeasureGroups.Add(mg1);
            var m1 = new Measure("[m1]", "m1", "df");
            mg1.Measures.Add(m1);
            var d1 = new Dimension("[d1]", "d1");
            mg1.LinkedDimensions.Add(d1);
            var h1 = new Hierarchy("[h1]", "h1");
            d1.Hierarchies.Add(h1);
            var l1 = new Level("[l1]", "l1", 0);
            h1.Levels.Add(l1);
            var p1 = new Property("[p1]", "p1");
            l1.Properties.Add(p1);
            var mg2 = new MeasureGroup("mg2");
            p.MeasureGroups.Add(mg2);
            var m2 = new Measure("[m2]", "m2", "df");
            mg2.Measures.Add(m2);
            mg2.LinkedDimensions.Add(d1);

            //set the object to test
            var mcw = new MetadataCsvWriter(filename);
            mcw.Write(metadata);

            //Assertion
            FileAssert.AreEqual(expectedFilename, filename);
        }

        [Test]
        public void Write_TwoPerspectivesIdentical_FileIsCorrectlyBuilt()
        {
            var header = GetHeader();
            var expectedContent = header
                + "\"p1\";\"mg1\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p2\";\"mg1\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                ;
            var expectedFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ExpectedCSV.csv");
            if (File.Exists(expectedFilename))
                File.Delete(expectedFilename);
            File.AppendAllText(expectedFilename, expectedContent, Encoding.UTF8);

            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");

            var metadata = new CubeMetadata();
            var pe1 = new Perspective("p1");
            metadata.Perspectives.Add(pe1);
            var mg1 = new MeasureGroup("mg1");
            pe1.MeasureGroups.Add(mg1);
            var m1 = new Measure("[m1]", "m1", "df");
            mg1.Measures.Add(m1);
            var d1 = new Dimension("[d1]", "d1");
            mg1.LinkedDimensions.Add(d1);
            var h1 = new Hierarchy("[h1]", "h1");
            d1.Hierarchies.Add(h1);
            var l1 = new Level("[l1]", "l1", 0);
            h1.Levels.Add(l1);
            var p1 = new Property("[p1]", "p1");
            l1.Properties.Add(p1);

            var pe2 = new Perspective("p2");
            metadata.Perspectives.Add(pe2);
            pe2.MeasureGroups.Add(mg1);

            //set the object to test
            var mcw = new MetadataCsvWriter(filename);
            mcw.Write(metadata);

            //Assertion
            FileAssert.AreEqual(expectedFilename, filename);
        }

        [Test]
        public void Write_TwoPerspectivesCompletelyDifferent_FileIsCorrectlyBuilt()
        {
            var header = GetHeader();
            var expectedContent = header
                + "\"p1\";\"mg1\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p2\";\"mg2\";\"m2\";\"[m2]\";\"d2\";\"[d2]\";\"h2\";\"[h2]\";\"l2\";\"[l2]\";\"0\";\"p2\";\"[p2]\"\r\n"
                ;
            var expectedFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ExpectedCSV.csv");
            if (File.Exists(expectedFilename))
                File.Delete(expectedFilename);
            File.AppendAllText(expectedFilename, expectedContent, Encoding.UTF8);

            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");

            var metadata = new CubeMetadata();
            var pe1 = new Perspective("p1");
            metadata.Perspectives.Add(pe1);
            var mg1 = new MeasureGroup("mg1");
            pe1.MeasureGroups.Add(mg1);
            var m1 = new Measure("[m1]", "m1", "df");
            mg1.Measures.Add(m1);
            var d1 = new Dimension("[d1]", "d1");
            mg1.LinkedDimensions.Add(d1);
            var h1 = new Hierarchy("[h1]", "h1");
            d1.Hierarchies.Add(h1);
            var l1 = new Level("[l1]", "l1", 0);
            h1.Levels.Add(l1);
            var p1 = new Property("[p1]", "p1");
            l1.Properties.Add(p1);

            var pe2 = new Perspective("p2");
            metadata.Perspectives.Add(pe2);
            var mg2 = new MeasureGroup("mg2");
            pe2.MeasureGroups.Add(mg2);
            var m2 = new Measure("[m2]", "m2", "df");
            mg2.Measures.Add(m2);
            var d2 = new Dimension("[d2]", "d2");
            mg2.LinkedDimensions.Add(d2);
            var h2 = new Hierarchy("[h2]", "h2");
            d2.Hierarchies.Add(h2);
            var l2 = new Level("[l2]", "l2", 0);
            h2.Levels.Add(l2);
            var p2 = new Property("[p2]", "p2");
            l2.Properties.Add(p2);

            //set the object to test
            var mcw = new MetadataCsvWriter(filename);
            mcw.Write(metadata);

            //Assertion
            FileAssert.AreEqual(expectedFilename, filename);
        }

        private string GetHeader()
        {
            return "\"Perspective\";\"MeasureGroup\";\"MeasureCaption\";\"MeasureUniqueName\";\"DimensionCaption\";\"DimensionUniqueName\";\"HierarchyCaption\";\"HierarchyUniqueName\";\"LevelCaption\";\"LevelUniqueName\";\"LevelNumber\";\"PropertyCaption\";\"PropertyUniqueName\"\r\n";
        }

        
    }
}
