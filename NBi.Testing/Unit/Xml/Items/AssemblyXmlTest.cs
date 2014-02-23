#region Using directives
using System.IO;
using System.Reflection;
using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    public class AssemblyXmlTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.AssemblyXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_MethodWithoutParam_AssemblyXml()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<AssemblyXml>());
            var assembly = (AssemblyXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(assembly, Is.Not.Null);
            Assert.That(assembly.Path, Is.EqualTo("NBi.Testing.dll"));
            Assert.That(assembly.Klass, Is.EqualTo("NBi.Testing.Unit.Acceptance.Resource.AssemblyClass"));
            Assert.That(assembly.Method, Is.EqualTo("GetSelectString"));
            Assert.That(assembly.Parameters, Has.Count.EqualTo(0));
        }

        [Test]
        public void Deserialize_MethodWithParamString_AssemblyXml()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<AssemblyXml>());
            var assembly = (AssemblyXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(assembly, Is.Not.Null);
            Assert.That(assembly.Parameters, Is.Not.Null);
            Assert.That(assembly.Parameters, Has.Count.EqualTo(1));

            Assert.That(assembly.Parameters[0].Name, Is.EqualTo("MyString"));
            Assert.That(assembly.Parameters[0].Value, Is.EqualTo("FirstValue"));
        }

        [Test]
        public void Deserialize_MethodWithParamDecimal_AssemblyXml()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<AssemblyXml>());
            var assembly = (AssemblyXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(assembly, Is.Not.Null);
            Assert.That(assembly.Parameters, Is.Not.Null);
            Assert.That(assembly.Parameters, Has.Count.EqualTo(1));

            Assert.That(assembly.Parameters[0].Name, Is.EqualTo("MyDecimal"));
            Assert.That(assembly.Parameters[0].Value, Is.EqualTo("10.52"));
        }

        [Test]
        public void Deserialize_MethodWithParamEnum_AssemblyXml()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<AssemblyXml>());
            var assembly = (AssemblyXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(assembly, Is.Not.Null);
            Assert.That(assembly.Parameters, Is.Not.Null);
            Assert.That(assembly.Parameters, Has.Count.EqualTo(1));

            Assert.That(assembly.Parameters[0].Name, Is.EqualTo("MyEnum"));
            Assert.That(assembly.Parameters[0].Value, Is.EqualTo("Beta"));
        }

        [Test]
        public void Deserialize_MethodWithParamDateTime_AssemblyXml()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<AssemblyXml>());
            var assembly = (AssemblyXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(assembly, Is.Not.Null);
            Assert.That(assembly.Parameters, Is.Not.Null);
            Assert.That(assembly.Parameters, Has.Count.EqualTo(1));

            Assert.That(assembly.Parameters[0].Name, Is.EqualTo("MyDateTime"));
            Assert.That(assembly.Parameters[0].Value, Is.EqualTo("2012-10-16 10:15"));
        }


    }
}
