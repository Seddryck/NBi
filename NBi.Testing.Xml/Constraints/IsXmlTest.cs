#region Using directives
using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
#endregion

namespace NBi.Testing.Xml.Unit.Constraints
{
    [TestFixture]
    public class IsXmlTest
    {


        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream($"{GetType().Assembly.GetName().Name}.Resources.IsXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_IsConstraint()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<IsXml>());
        }

        [Test]
        public void Deserialize_SampleFile_IsConstraintValue()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<IsXml>());
            var ctrXml = ts.Tests[testNr].Constraints[0] as IsXml;
            Assert.That(ctrXml.Value, Is.EqualTo("varchar(50)"));
        }

        [Test]
        public void Deserialize_SampleFile_IsConstraintValueCrLf()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<IsXml>());
            var ctrXml = ts.Tests[testNr].Constraints[0] as IsXml;
            Assert.That(ctrXml.Value, Is.EqualTo("varchar(50)"));
        }


        [Test]
        public void Serialize_IsXml_NoDefaultAndSettings()
        {
            var isXml = new IsXml();
            isXml.Value = "decimal(10,2)";

            var testXml = new TestXml();
            testXml.Constraints.Add(isXml);

            var serializer = new XmlSerializer(typeof(TestXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, testXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Does.Contain("<is"));
            Assert.That(content, Does.Contain(">decimal(10,2)<"));
            Assert.That(content, Does.Not.Contain("efault"));
            Assert.That(content, Does.Not.Contain("eference"));
        }
    }
}
