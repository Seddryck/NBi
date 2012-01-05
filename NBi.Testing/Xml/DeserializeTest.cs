using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Testing.Xml
{
    [TestFixture]
    public class DeserializeTest
    {
        [Test]
        public void Deserialize_SampleFile_Successfully()
        {   
            // Create an instance of the XmlSerializer specifying type and namespace.
            XmlSerializer serializer = new XmlSerializer(typeof(TestSuiteXml));
            // Declare an object variable of the type to be deserialized.
            TestSuiteXml ts;

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                               .GetManifestResourceStream("NBi.Testing.Xml.TestSuiteSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                // Use the Deserialize method to restore the object's state.
                ts = (TestSuiteXml)serializer.Deserialize(reader);
            }

            // Write out the properties of the object.
            Assert.That(ts.Name, Is.EqualTo("The TestSuite"));

            Assert.That(ts.Tests.Count, Is.GreaterThanOrEqualTo(2));
            
            Assert.That(ts.Tests[0].Name, Is.EqualTo("My first test case"));
            Assert.That(ts.Tests[0].UniqueIdentifier,  Is.EqualTo("0001"));

            Assert.That(ts.Tests[0].Constraints.Count, Is.GreaterThanOrEqualTo(1));

            Assert.That(ts.Tests[0].Constraints[0], Is.InstanceOf<SyntacticallyCorrectXml>());
            Assert.That(((SyntacticallyCorrectXml)ts.Tests[0].Constraints[0]).ConnectionString, Is.Not.Null.And.Not.Empty);

            Assert.That(ts.Tests[1].Constraints[0], Is.InstanceOf<SyntacticallyCorrectXml>());
            Assert.That(((SyntacticallyCorrectXml)ts.Tests[1].Constraints[0]).ConnectionString, Is.Not.Null.And.Not.Empty);
            Assert.That(ts.Tests[1].Constraints[1], Is.InstanceOf<FasterThanXml>());
            Assert.That(((FasterThanXml)ts.Tests[1].Constraints[1]).ConnectionString, Is.Not.Empty);
            Assert.That(((FasterThanXml)ts.Tests[1].Constraints[1]).MaxTimeMilliSeconds, Is.EqualTo(5000));

            Assert.That(ts.Tests[1].TestCases[0], Is.InstanceOf<TestCaseXml>());
            Assert.That(ts.Tests[1].TestCases[0].Sql, Is.Not.Null.And.Not.Empty.And.ContainsSubstring("SELECT"));
            Assert.That(ts.Tests[1].TestCases[0].Filename, Is.Null);
            Assert.That(ts.Tests[1].TestCases[1].Sql, Is.Null);
            Assert.That(ts.Tests[1].TestCases[1].Filename, Is.Not.Null.And.Not.Empty);
            
        }
    }
}
