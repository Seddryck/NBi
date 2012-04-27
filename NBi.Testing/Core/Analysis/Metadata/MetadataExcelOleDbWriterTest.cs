using System.IO;
using NUnit.Framework;
using NBi.Core.Analysis.Metadata;

namespace NBi.Testing.Core.Analysis.Metadata
{
    [TestFixture]
    public class MetadataExcelOleDbWriterTest
    {
        [Test]
        public void Write_ExistingSheet_FileBiggerThanOriginal()
        {
            //Build the fullpath for the file to read
            var filename = DiskOnFile.CreatePhysicalFile("MetadataExistingSheet.xls", "NBi.Testing.Core.Analysis.Metadata.MetadataExcelSample.xls");
            var initLength = new FileInfo(filename).Length;

            //set the object to test
            var persp = BuildFakeMetadata();

            var mew = new MetadataExcelOleDbWriter(filename);
            mew.SheetName = "Metadata";
            mew.Write(persp);
            
            //Assertion
            Assert.Greater(new FileInfo(filename).Length, initLength);
        }

        [Test]
        public void Write_NotExistingSheet_FileBiggerThanOriginal()
        {
            var persp = BuildFakeMetadata();

            //Build the fullpath for the file to read
            var filename = DiskOnFile.CreatePhysicalFile("MetadataNotExistingSheet.xls", "NBi.Testing.Core.Analysis.Metadata.MetadataExcelSample.xls");
            var initLength = new FileInfo(filename).Length;

            //set the object to test
            var mew = new MetadataExcelOleDbWriter(filename);
            mew.SheetName = "NotExistingMetadata";
            mew.Write(persp);

            //Assertion
            Assert.Greater(new FileInfo(filename).Length, initLength);
        }

        [Test]
        public void Write_NotExistingFile_FileIsCreated()
        {
            var persp = BuildFakeMetadata();

            //Build the fullpath for the file to read
            var filename = Path.Combine(DiskOnFile.GetDirectoryPath(), @"MetadataNotExistingFile.xls");

            //set the object to test
            var mew = new MetadataExcelOleDbWriter(filename);
            mew.SheetName = "MySheet";
            mew.Write(persp);

            //Assertion
            Assert.IsTrue(File.Exists(filename));
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
