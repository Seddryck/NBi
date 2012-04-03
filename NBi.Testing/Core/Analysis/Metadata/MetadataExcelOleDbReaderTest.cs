using NUnit.Framework;
using NBi.Core.Analysis.Metadata;

namespace NBi.Testing.Core.Analysis.Metadata
{
    [TestFixture]
    public class MetadataExcelOleDbReaderTest
    {
        [Test]
        public void Read_Format10_CorrectlyLoaded()
        {
            //Build the fullpath for the file to read
            var filename = DiskOnFile.CreatePhysicalFile("MetadataFormat10.xls", "NBi.Testing.Core.Analysis.Metadata.MetadataExcelSample.xls");

            //set the object to test
            var mer = new MetadataExcelOleDbReader(filename);
            mer.SheetName = "Format10";
            var mgs = new MeasureGroups();
            var dims = new Dimensions();

            mer.Read(ref mgs, ref dims);

            //Assertions
            Assert.AreEqual(2, mgs.Count);
            Assert.That(mgs.ContainsKey("Measure Group 1"));
            Assert.That(mgs.ContainsKey("Measure Group 2"));
            Assert.AreEqual("Measure Group 1", mgs["Measure Group 1"].Name);

            Assert.AreEqual(1, mgs["Measure Group 1"].Measures.Count);
            Assert.AreEqual(2, mgs["Measure Group 2"].Measures.Count);
            Assert.That(mgs["Measure Group 1"].Measures.ContainsKey("[Measure 1.1]"));
            Assert.AreEqual("Measure 1.1", mgs["Measure Group 1"].Measures["[Measure 1.1]"].Caption);
            Assert.AreEqual("[Measure 1.1]", mgs["Measure Group 1"].Measures["[Measure 1.1]"].UniqueName);

            Assert.AreEqual(3, dims.Count);
            Assert.That(dims.ContainsKey("[Dimension 1]"));
            Assert.That(dims.ContainsKey("[Dimension 2]"));
            Assert.That(dims.ContainsKey("[Dimension 3]"));

            Assert.AreEqual(1, dims["[Dimension 1]"].Hierarchies.Count);
            Assert.AreEqual(2, dims["[Dimension 3]"].Hierarchies.Count);
            Assert.AreEqual("Dimension 3", dims["[Dimension 3]"].Caption);
            Assert.AreEqual("[Dimension 3]", dims["[Dimension 3]"].UniqueName);

            Assert.That(dims["[Dimension 1]"].Hierarchies.ContainsKey("[Hierarchy 1.1]"));
            Assert.AreEqual("Hierarchy 1.1", dims["[Dimension 1]"].Hierarchies["[Hierarchy 1.1]"].Caption);
            Assert.AreEqual("[Hierarchy 1.1]", dims["[Dimension 1]"].Hierarchies["[Hierarchy 1.1]"].UniqueName);
        }
  
        [Test]
        public void Read_Format20_CorrectlyLoaded()
        {
            //Build the fullpath for the file to read
            var filename = DiskOnFile.CreatePhysicalFile("MetadataFormat20.xls", "NBi.Testing.Core.Analysis.Metadata.MetadataExcelSample.xls");

            //set the object to test
            var mer = new MetadataExcelOleDbReader(filename);
            mer.SheetName = "Format20";
            var mgs = new MeasureGroups();
            var dims = new Dimensions();

            mer.Read(ref mgs, ref dims);

            //Assertions
            Assert.AreEqual(2, mgs.Count);
            Assert.That(mgs.ContainsKey("Measure Group 1"));
            Assert.That(mgs.ContainsKey("Measure Group 2"));
            Assert.AreEqual("Measure Group 1", mgs["Measure Group 1"].Name);

            Assert.AreEqual(1, mgs["Measure Group 1"].Measures.Count);
            Assert.AreEqual(2, mgs["Measure Group 2"].Measures.Count);
            Assert.That(mgs["Measure Group 1"].Measures.ContainsKey("[Measure 1.1]"));
            Assert.AreEqual("Measure 1.1", mgs["Measure Group 1"].Measures["[Measure 1.1]"].Caption);
            Assert.AreEqual("[Measure 1.1]", mgs["Measure Group 1"].Measures["[Measure 1.1]"].UniqueName);

            Assert.AreEqual(3, dims.Count);
            Assert.That(dims.ContainsKey("[Dimension 1]"));
            Assert.That(dims.ContainsKey("[Dimension 2]"));
            Assert.That(dims.ContainsKey("[Dimension 3]"));

            Assert.AreEqual(1, dims["[Dimension 1]"].Hierarchies.Count);
            Assert.AreEqual(2, dims["[Dimension 3]"].Hierarchies.Count);
            Assert.AreEqual("Dimension 3", dims["[Dimension 3]"].Caption);
            Assert.AreEqual("[Dimension 3]", dims["[Dimension 3]"].UniqueName);

            Assert.That(dims["[Dimension 1]"].Hierarchies.ContainsKey("[Hierarchy 1.1]"));
            Assert.AreEqual("Hierarchy 1.1", dims["[Dimension 1]"].Hierarchies["[Hierarchy 1.1]"].Caption);
            Assert.AreEqual("[Hierarchy 1.1]", dims["[Dimension 1]"].Hierarchies["[Hierarchy 1.1]"].UniqueName);

        }


    }
}
