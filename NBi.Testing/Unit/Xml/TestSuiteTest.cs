using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class TestSuiteTest
    {

        protected TestSuiteXml DeserializeSample()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            XmlSerializer serializer = new XmlSerializer(typeof(TestSuiteXml));
            // Declare an object variable of the type to be deserialized.
            TestSuiteXml ts;

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.TestSuiteSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                // Use the Deserialize method to restore the object's state.
                ts = (TestSuiteXml)serializer.Deserialize(reader);
            }
            return ts;
        }
        
        [Test]
        public void Deserialize_SampleFile_TestSuiteLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Name, Is.EqualTo("The TestSuite"));
        }
  

        [Test]
        public void Deserialize_SampleFile_TestsLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests.Count, Is.EqualTo(4));
        }
        [Test]
        public void Deserialize_SampleFile_TestMembersLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[0].Name, Is.EqualTo("My first test case"));
            Assert.That(ts.Tests[0].UniqueIdentifier, Is.EqualTo("0001"));
        }

        [Test]
        public void Deserialize_SampleFile_ConstraintsLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[0].Constraints.Count, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void Deserialize_SampleFile_ConstraintSyntacticallyCorrectLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            ts.Tests.GetRange(0,2).ForEach(t => Assert.That(t.Constraints[0], Is.InstanceOf<SyntacticallyCorrectXml>()));

        }

        [Test]
        public void Deserialize_SampleFile_ConstraintFasterThanLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[1].Constraints[1], Is.InstanceOf<FasterThanXml>());
            Assert.That(((FasterThanXml)ts.Tests[1].Constraints[1]).MaxTimeMilliSeconds, Is.EqualTo(5000));
        }

        [Test]
        public void Deserialize_SampleFile_ConstraintEqualToLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[2].Constraints[0], Is.InstanceOf<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[2].Constraints[0]).ResultSetFile, Is.Not.Empty);
        }
        
        [Test]
        public void Deserialize_SampleFile_TestCaseMembersLoaded()
        {
            //Create the queryfile to read
            DiskOnFile.CreatePhysicalFile("Select all products.sql", "NBi.Testing.Unit.Xml.Resources.SelectAllProducts.sql");
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[1].TestCases[0], Is.InstanceOf<TestCaseXml>());
            Assert.That(ts.Tests[1].TestCases[0].Query, Is.Not.Null.And.Not.Empty.And.ContainsSubstring("SELECT"));
            Assert.That(ts.Tests[1].TestCases[0].InlineQuery, Is.Not.Null.And.Not.Empty.And.ContainsSubstring("SELECT"));
            Assert.That(ts.Tests[1].TestCases[0].Filename, Is.Null);
            Assert.That(ts.Tests[1].TestCases[1].Query, Is.Not.Null.And.Not.Empty.And.ContainsSubstring("SELECT"));
            Assert.That(ts.Tests[1].TestCases[1].InlineQuery, Is.Null);
            Assert.That(ts.Tests[1].TestCases[1].Filename, Is.Not.Null.And.Not.Empty);
            
            Assert.That(ts.Tests[2].TestCases[0].ConnectionString, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void Deserialize_SampleFile_TestCategoriesLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[1].Categories.Count, Is.EqualTo(2));
            Assert.That(ts.Tests[1].Categories, Has.Member("Category 1"));
            Assert.That(ts.Tests[1].Categories, Has.Member("Category 2"));
        }

        [Test]
        public void Deserialize_SampleFile_EqualToWithQuery()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[3].Constraints[0], Is.TypeOf<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[3].Constraints[0]).ConnectionString, Is.Not.Null.And.Not.Empty);
            Assert.That(((EqualToXml)ts.Tests[3].Constraints[0]).QueryFile, Is.Not.Null.And.Not.Empty);
        }
    }
}
