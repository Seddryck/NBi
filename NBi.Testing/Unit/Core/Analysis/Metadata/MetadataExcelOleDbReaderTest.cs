using NUnit.Framework;
using NBi.Core.Analysis.Metadata;

namespace NBi.Testing.Unit.Core.Analysis.Metadata
{
    [TestFixture]
    public class MetadataExcelOleDbReaderTest
    {
        [Test]
        public void Read_Format10_CorrectlyLoaded()
        {
            //Build the fullpath for the file to read
            var filename = DiskOnFile.CreatePhysicalFile("MetadataFormat10.xls", "NBi.Testing.Unit.Core.Analysis.Metadata.Resources.MetadataExcelSample.xls");

            //set the object to test
            var mer = new MetadataExcelOleDbReader(filename);
            mer.SheetName = "Format10";

            var metadata = mer.Read();
            Perspective perspective = metadata.Perspectives["Perspective"];

            //Assertions
            Assert.AreEqual(2, perspective.MeasureGroups.Count);
            Assert.That(perspective.MeasureGroups.ContainsKey("Measure Group 1"));
            Assert.That(perspective.MeasureGroups.ContainsKey("Measure Group 2"));
            Assert.AreEqual("Measure Group 1", perspective.MeasureGroups["Measure Group 1"].Name);

            Assert.AreEqual(1, perspective.MeasureGroups["Measure Group 1"].Measures.Count);
            Assert.AreEqual(2, perspective.MeasureGroups["Measure Group 2"].Measures.Count);
            Assert.That(perspective.MeasureGroups["Measure Group 1"].Measures.ContainsKey("[Measure 1.1]"));
            Assert.AreEqual("Measure 1.1", perspective.MeasureGroups["Measure Group 1"].Measures["[Measure 1.1]"].Caption);
            Assert.AreEqual("[Measure 1.1]", perspective.MeasureGroups["Measure Group 1"].Measures["[Measure 1.1]"].UniqueName);

            Assert.AreEqual(3, perspective.Dimensions.Count);
            Assert.That(perspective.Dimensions.ContainsKey("[Dimension 1]"));
            Assert.That(perspective.Dimensions.ContainsKey("[Dimension 2]"));
            Assert.That(perspective.Dimensions.ContainsKey("[Dimension 3]"));

            Assert.AreEqual(1, perspective.Dimensions["[Dimension 1]"].Hierarchies.Count);
            Assert.AreEqual(2, perspective.Dimensions["[Dimension 3]"].Hierarchies.Count);
            Assert.AreEqual("Dimension 3", perspective.Dimensions["[Dimension 3]"].Caption);
            Assert.AreEqual("[Dimension 3]", perspective.Dimensions["[Dimension 3]"].UniqueName);

            Assert.That(perspective.Dimensions["[Dimension 1]"].Hierarchies.ContainsKey("[Hierarchy 1.1]"));
            Assert.AreEqual("Hierarchy 1.1", perspective.Dimensions["[Dimension 1]"].Hierarchies["[Hierarchy 1.1]"].Caption);
            Assert.AreEqual("[Hierarchy 1.1]", perspective.Dimensions["[Dimension 1]"].Hierarchies["[Hierarchy 1.1]"].UniqueName);
        }
  
        [Test]
        public void Read_Format20_CorrectlyLoaded()
        {
            //Build the fullpath for the file to read
            var filename = DiskOnFile.CreatePhysicalFile("MetadataFormat20.xls", "NBi.Testing.Unit.Core.Analysis.Metadata.Resources.MetadataExcelSample.xls");

            //set the object to test
            var mer = new MetadataExcelOleDbReader(filename);
            mer.SheetName = "Format20";

            var metadata = mer.Read();
            Perspective perspective = metadata.Perspectives["Perspective"];

            //Assertions
            Assert.AreEqual(2, perspective.MeasureGroups.Count);
            Assert.That(perspective.MeasureGroups.ContainsKey("Measure Group 1"));
            Assert.That(perspective.MeasureGroups.ContainsKey("Measure Group 2"));
            Assert.AreEqual("Measure Group 1", perspective.MeasureGroups["Measure Group 1"].Name);

            Assert.AreEqual(1, perspective.MeasureGroups["Measure Group 1"].Measures.Count);
            Assert.AreEqual(2, perspective.MeasureGroups["Measure Group 2"].Measures.Count);
            Assert.That(perspective.MeasureGroups["Measure Group 1"].Measures.ContainsKey("[Measure 1.1]"));
            Assert.AreEqual("Measure 1.1", perspective.MeasureGroups["Measure Group 1"].Measures["[Measure 1.1]"].Caption);
            Assert.AreEqual("[Measure 1.1]", perspective.MeasureGroups["Measure Group 1"].Measures["[Measure 1.1]"].UniqueName);

            Assert.AreEqual(3, perspective.Dimensions.Count);
            Assert.That(perspective.Dimensions.ContainsKey("[Dimension 1]"));
            Assert.That(perspective.Dimensions.ContainsKey("[Dimension 2]"));
            Assert.That(perspective.Dimensions.ContainsKey("[Dimension 3]"));

            Assert.AreEqual(1, perspective.Dimensions["[Dimension 1]"].Hierarchies.Count);
            Assert.AreEqual(2, perspective.Dimensions["[Dimension 3]"].Hierarchies.Count);
            Assert.AreEqual("Dimension 3", perspective.Dimensions["[Dimension 3]"].Caption);
            Assert.AreEqual("[Dimension 3]", perspective.Dimensions["[Dimension 3]"].UniqueName);

            Assert.That(perspective.Dimensions["[Dimension 1]"].Hierarchies.ContainsKey("[Hierarchy 1.1]"));
            Assert.AreEqual("Hierarchy 1.1", perspective.Dimensions["[Dimension 1]"].Hierarchies["[Hierarchy 1.1]"].Caption);
            Assert.AreEqual("[Hierarchy 1.1]", perspective.Dimensions["[Dimension 1]"].Hierarchies["[Hierarchy 1.1]"].UniqueName);

        }


    }
}
