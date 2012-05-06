using System.IO;
using System.Text;
using NBi.Core.Analysis.Metadata;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.Analysis.Metadata
{
    [TestFixture]
    public class MetadataCsvReaderTest
    {
        [Test]
        public void Read_SimpleLine_PerspectiveIsCorrectlyRead()
        {
            var header = GetHeader();
            var testContent = header + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d\";\"[d]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n";
            var testFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ReadCSV.csv");
            if (File.Exists(testFilename))
                File.Delete(testFilename);
            File.AppendAllText(testFilename, testContent, Encoding.UTF8);

            //set the object to test
            var mcr = new MetadataCsvReader(testFilename);
            var metadata = mcr.Read();

            //Assertion
            Assert.That(metadata.Perspectives.Count, Is.EqualTo(1));
            Assert.That(metadata.Perspectives["p"].Name, Is.EqualTo("p"));
        }

        [Test]
        public void Read_TwoLinesWithPerspectivesCompletelyDifferent_PerspectiveAreCorrectlyRead()
        {
            var header = GetHeader();
            var testContent = header
                + "\"p1\";\"mg1\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p2\";\"mg2\";\"m2\";\"[m2]\";\"d2\";\"[d2]\";\"h2\";\"[h2]\";\"l2\";\"[l2]\";\"0\";\"p2\";\"[p2]\"\r\n"
                ;
            var testFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ReadCSV.csv");
            if (File.Exists(testFilename))
                File.Delete(testFilename);
            File.AppendAllText(testFilename, testContent, Encoding.UTF8);

            //set the object to test
            var mcr = new MetadataCsvReader(testFilename);
            var metadata = mcr.Read();

            //Assertion
            Assert.That(metadata.Perspectives.Count, Is.EqualTo(2));
            Assert.That(metadata.Perspectives["p1"].Name, Is.EqualTo("p1"));
            Assert.That(metadata.Perspectives["p2"].Name, Is.EqualTo("p2"));
        }


        [Test]
        public void Read_TwoMeasureGroups_MeasureGroupsAreCorrectlyRead()
        {
            var header = GetHeader();
            var testContent = header
                + "\"p\";\"mg1\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg2\";\"m2\";\"[m2]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                ;
            var testFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ReadCSV.csv");
            if (File.Exists(testFilename))
                File.Delete(testFilename);
            File.AppendAllText(testFilename, testContent, Encoding.UTF8);
            
            //set the object to test
            var mcr = new MetadataCsvReader(testFilename);
            var metadata = mcr.Read();

            //Assertion
            Assert.That(metadata.Perspectives["p"].MeasureGroups.Count, Is.EqualTo(2));
            Assert.That(metadata.Perspectives["p"].MeasureGroups["mg1"].Name, Is.EqualTo("mg1"));
            Assert.That(metadata.Perspectives["p"].MeasureGroups["mg2"].Name, Is.EqualTo("mg2"));
        }


        [Test]
        public void Read_TwoDimensionsSameMeasureGroup_DimensionsAreCorrectlyRead()
        {
            var header = GetHeader();
            var testContent = header
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d2\";\"[d2]\";\"h2\";\"[h2]\";\"l2\";\"[l2]\";\"1\";\"p2\";\"[p2]\"\r\n"
                ;
            var testFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ReadCSV.csv");
            if (File.Exists(testFilename))
                File.Delete(testFilename);
            File.AppendAllText(testFilename, testContent, Encoding.UTF8);

            //set the object to test
            var mcr = new MetadataCsvReader(testFilename);
            var metadata = mcr.Read();

            //Assertion
            Assert.That(metadata.Perspectives["p"].MeasureGroups["mg"].LinkedDimensions.Count, Is.EqualTo(2));
            Assert.That(metadata.Perspectives["p"].MeasureGroups["mg"].LinkedDimensions["[d1]"].Caption, Is.EqualTo("d1"));
            Assert.That(metadata.Perspectives["p"].MeasureGroups["mg"].LinkedDimensions["[d2]"].Caption, Is.EqualTo("d2"));
            Assert.That(metadata.Perspectives["p"].MeasureGroups["mg"].LinkedDimensions["[d1]"].UniqueName, Is.EqualTo("[d1]"));
            Assert.That(metadata.Perspectives["p"].MeasureGroups["mg"].LinkedDimensions["[d2]"].UniqueName, Is.EqualTo("[d2]"));
        }


        [Test]
        public void Read_TwoHierarchiesSameDimension_HierarchiesAreCorrectlyRead()
        {
            var header = GetHeader();
            var testContent = header
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h2\";\"[h2]\";\"l2\";\"[l2]\";\"1\";\"p2\";\"[p2]\"\r\n"
                ;
            var testFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ReadCSV.csv");
            if (File.Exists(testFilename))
                File.Delete(testFilename);
            File.AppendAllText(testFilename, testContent, Encoding.UTF8);


            //set the object to test
            var mcr = new MetadataCsvReader(testFilename);
            var metadata = mcr.Read();

            var dim = metadata.Perspectives["p"].MeasureGroups["mg"].LinkedDimensions["[d1]"];
            //Assertion
            Assert.That(dim.Hierarchies.Count, Is.EqualTo(2));
            Assert.That(dim.Hierarchies["[h1]"].Caption, Is.EqualTo("h1"));
            Assert.That(dim.Hierarchies["[h2]"].Caption, Is.EqualTo("h2"));
            Assert.That(dim.Hierarchies["[h1]"].UniqueName, Is.EqualTo("[h1]"));
            Assert.That(dim.Hierarchies["[h2]"].UniqueName, Is.EqualTo("[h2]"));
        }

        [Test]
        public void Read_TwoLevelsSameHierarchy_LevelsAreCorrectlyRead()
        {
            var header = GetHeader();
            var testContent = header
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d\";\"[d]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d\";\"[d]\";\"h1\";\"[h1]\";\"l2\";\"[l2]\";\"1\";\"p2\";\"[p2]\"\r\n"
                ;
            var testFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ReadCSV.csv");
            if (File.Exists(testFilename))
                File.Delete(testFilename);
            File.AppendAllText(testFilename, testContent, Encoding.UTF8);

            //set the object to test
            var mcr = new MetadataCsvReader(testFilename);
            var metadata = mcr.Read();

            var hie = metadata.Perspectives["p"].MeasureGroups["mg"].LinkedDimensions["[d]"].Hierarchies["[h1]"];
            
            //Assertion
            Assert.That(hie.Levels.Count, Is.EqualTo(2));
            Assert.That(hie.Levels["[l1]"].Caption, Is.EqualTo("l1"));
            Assert.That(hie.Levels["[l2]"].Caption, Is.EqualTo("l2"));
            Assert.That(hie.Levels["[l1]"].UniqueName, Is.EqualTo("[l1]"));
            Assert.That(hie.Levels["[l2]"].UniqueName, Is.EqualTo("[l2]"));
            Assert.That(hie.Levels["[l1]"].Number, Is.EqualTo(0));
            Assert.That(hie.Levels["[l2]"].Number, Is.EqualTo(1));
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        [Test]
        public void Read_NoPropertyForTheLevel_PropertyCountIsEqualToZero()
        {
            var header = GetHeader();
            var testContent = header + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d\";\"[d]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"\";\"\"\r\n";
            var testFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ReadCSV.csv");
            if (File.Exists(testFilename))
                File.Delete(testFilename);
            File.AppendAllText(testFilename, testContent, Encoding.UTF8);

            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"ActualCSV.csv");

            //set the object to test
            var mcr = new MetadataCsvReader(testFilename);
            var metadata = mcr.Read();

            var lev = metadata.Perspectives["p"].MeasureGroups["mg"].LinkedDimensions["[d]"].Hierarchies["[h1]"].Levels["[l1]"];

            //Assertion
            Assert.That(lev.Properties.Count, Is.EqualTo(0));
        }

        [Test]
        public void Read_TwoPropertiesForTheSameLevel_PropertiesAreCorrectlyAssigned()
        {
            var header = GetHeader();
            var testContent = header 
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d\";\"[d]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d\";\"[d]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p2\";\"[p2]\"\r\n"
                ;
            var testFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ReadCSV.csv");
            if (File.Exists(testFilename))
                File.Delete(testFilename);
            File.AppendAllText(testFilename, testContent, Encoding.UTF8);

            //set the object to test
            var mcr = new MetadataCsvReader(testFilename);
            var metadata = mcr.Read();

            var lev = metadata.Perspectives["p"].MeasureGroups["mg"].LinkedDimensions["[d]"].Hierarchies["[h1]"].Levels["[l1]"];

            //Assertion
            Assert.That(lev.Properties.Count, Is.EqualTo(2));
            Assert.That(lev.Properties["[p1]"].Caption, Is.EqualTo("p1"));
            Assert.That(lev.Properties["[p2]"].Caption, Is.EqualTo("p2"));
            Assert.That(lev.Properties["[p1]"].UniqueName, Is.EqualTo("[p1]"));
            Assert.That(lev.Properties["[p2]"].UniqueName, Is.EqualTo("[p2]"));
        }

        [Test]
        public void Read_TwoMeasuresSameMeasureGroups_FileIsCorrectlyBuilt()
        {
            var header = GetHeader();
            var testContent = header
                + "\"p\";\"mg\";\"m1\";\"[m1]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                + "\"p\";\"mg\";\"m2\";\"[m2]\";\"d1\";\"[d1]\";\"h1\";\"[h1]\";\"l1\";\"[l1]\";\"0\";\"p1\";\"[p1]\"\r\n"
                ;
            var testFilename = Path.Combine(DiskOnFile.GetDirectoryPath(), "ReadCSV.csv");
            if (File.Exists(testFilename))
                File.Delete(testFilename);
            File.AppendAllText(testFilename, testContent, Encoding.UTF8);

            //set the object to test
            var mcr = new MetadataCsvReader(testFilename);
            var metadata = mcr.Read();

            var mg = metadata.Perspectives["p"].MeasureGroups["mg"];

            //Assertion
            Assert.That(mg.Measures.Count, Is.EqualTo(2));
            Assert.That(mg.Measures["[m1]"].Caption, Is.EqualTo("m1"));
            Assert.That(mg.Measures["[m2]"].Caption, Is.EqualTo("m2"));
            Assert.That(mg.Measures["[m1]"].UniqueName, Is.EqualTo("[m1]"));
            Assert.That(mg.Measures["[m2]"].UniqueName, Is.EqualTo("[m2]"));
        }

        private string GetHeader()
        {
            return "\"Perspective\";\"MeasureGroup\";\"MeasureCaption\";\"MeasureUniqueName\";\"DimensionCaption\";\"DimensionUniqueName\";\"HierarchyCaption\";\"HierarchyUniqueName\";\"LevelCaption\";\"LevelUniqueName\";\"LevelNumber\";\"PropertyCaption\";\"PropertyUniqueName\"\r\n";
        }

        
    }
}
