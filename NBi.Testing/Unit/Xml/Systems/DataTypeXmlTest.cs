#region Using directives
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.Xml.Systems
{
    [TestFixture]
    public class DataTypeXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.DataTypeXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_Column()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            
            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<DataTypeXml>());
            Assert.That(((DataTypeXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<ColumnXml>());

            ColumnXml item = (ColumnXml)((DataTypeXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Caption, Is.EqualTo("column"));
            Assert.That(item.Perspective, Is.EqualTo("dwh"));
            Assert.That(item.GetConnectionString(), Is.EqualTo("ConnectionString"));
        }


        [Test]
        public void Serialize_DataTypeXml_NoDefaultAndSettings()
        {
            var columnXml = new ColumnXml();
            columnXml.Caption = "My Caption";
            columnXml.Perspective = "My Schema";
            columnXml.Default = new DefaultXml() { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = "connStr" };
            columnXml.Settings = new SettingsXml()
            {
                References = new List<ReferenceXml>() 
                    { new ReferenceXml() 
                        { Name = "Bob", ConnectionString = "connStr" } 
                    }
            };
            var dataTypeXml = new DataTypeXml() { Item = columnXml };

            var serializer = new XmlSerializer(typeof(DataTypeXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, dataTypeXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("<column"));
            Assert.That(content, Is.StringContaining("caption=\"My Caption\""));
            Assert.That(content, Is.StringContaining("perspective=\"My Schema\""));
            Assert.That(content, Is.Not.StringContaining("efault"));
            Assert.That(content, Is.Not.StringContaining("eference"));
        }
        

    }
}
