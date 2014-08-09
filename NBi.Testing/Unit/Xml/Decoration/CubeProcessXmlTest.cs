using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Decoration;
using NBi.Xml.Decoration.Command;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Decoration
{
    [TestFixture]
    public class CubeProcessXmlTest
    {

        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.CubeProcessXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_WholeCube_CorrectAttributeAssignements()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Setup.Commands, Has.Count.EqualTo(1));
            Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.TypeOf<CubeProcessXml>());

            var cubeProcess = ts.Tests[testNr].Setup.Commands[0] as CubeProcessXml;          

            Assert.That(cubeProcess.ConnectionString, Is.EqualTo(@"Provider=MSOLAP.4;Data Source=(local)\SQL2012;Initial Catalog='Adventure Works DW 2012';localeidentifier=1033"));
            Assert.That(cubeProcess.Cube, Is.EqualTo("MyCube"));
        }


        [Test]
        public void Deserialize_WholeCube_CorrectDefinitionOfObjectsToProcess()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var cubeProcess = ts.Tests[testNr].Setup.Commands[0] as CubeProcessXml;

            Assert.That(cubeProcess.IsWholeCube, Is.True);
            Assert.That(cubeProcess.Dimensions, Is.Null.Or.Empty);
            Assert.That(cubeProcess.MeasureGroups, Is.Null.Or.Empty);
        }

        [Test]
        public void Deserialize_TwoDimensions_CorrectDefinitionsOfObjectToProcess()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var cubeProcess = ts.Tests[testNr].Setup.Commands[0] as CubeProcessXml;

            Assert.That(cubeProcess.IsWholeCube, Is.False);
            Assert.That(cubeProcess.Dimensions, Is.Not.Null.And.Not.Empty);
            Assert.That(cubeProcess.MeasureGroups, Is.Null.Or.Empty);
        }

        [Test]
        public void Deserialize_TwoDimensions_CorrectParameters()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var cubeProcess = ts.Tests[testNr].Setup.Commands[0] as CubeProcessXml;

            Assert.That(cubeProcess.Dimensions, Has.Count.EqualTo(2));
            Assert.That(List.Map(cubeProcess.Dimensions).Property("Name"), Is.EquivalentTo(new string[] {"First dimension", "Second dimension"}));
        }

        [Test]
        public void Deserialize_MeasureGroup_CorrectDefinitionsOfObjectToProcess()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var cubeProcess = ts.Tests[testNr].Setup.Commands[0] as CubeProcessXml;

            Assert.That(cubeProcess.IsWholeCube, Is.False);
            Assert.That(cubeProcess.Dimensions, Is.Null.Or.Empty);
            Assert.That(cubeProcess.MeasureGroups, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void Deserialize_MeasureGroup_CorrectParameters()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var cubeProcess = ts.Tests[testNr].Setup.Commands[0] as CubeProcessXml;

            Assert.That(cubeProcess.MeasureGroups[0].IsAllPartitions, Is.True);
            Assert.That(cubeProcess.MeasureGroups[0].Name, Is.EqualTo("MyMeasureGroup"));
        }

        [Test]
        public void Deserialize_MeasureGroupWithPartitions_CorrectParameters()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var cubeProcess = ts.Tests[testNr].Setup.Commands[0] as CubeProcessXml;

            Assert.That(cubeProcess.MeasureGroups[0].IsAllPartitions, Is.False);
            Assert.That(cubeProcess.MeasureGroups[0].Partitions, Is.Unique);
            Assert.That(cubeProcess.MeasureGroups[0].Partitions, Is.EquivalentTo(new int[]{1,2,3,7,9,10,11,12}));
        }


    }
}
