using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Xml.Constraints;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
using NBi.Xml.SerializationOption;

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    public class QueryXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.QueryXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_QueryWithoutParams_QueryXml()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<QueryXml>());
            var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(query, Is.Not.Null);
            Assert.That(query.GetQuery(), Is.StringContaining("select top 1 myColumn from myTable"));
            Assert.That(query.Parameters, Has.Count.EqualTo(0));
        }

        [Test]
        public void Deserialize_QueryWithTwoParams_QueryXml()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(query.GetQuery(), Is.StringContaining("select myColumn from myTable where myFirstField=@FirstField and 1=@NonEmpty"));

            Assert.That(query.Parameters, Is.Not.Null);
            Assert.That(query.Parameters, Is.Not.Empty);
            Assert.That(query.Parameters, Has.Count.EqualTo(2));

            Assert.That(query.Parameters[0].Name, Is.EqualTo("FirstField"));
            Assert.That(query.Parameters[0].GetValue<string>(), Is.EqualTo("Identifier"));

            Assert.That(query.Parameters[1].Name, Is.EqualTo("NonEmpty"));
            Assert.That(query.Parameters[1].GetValue<int>(), Is.EqualTo(1));
        }

        [Test]
        public void Deserialize_QueryWithOneParamAndSqlType_QueryXml()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(query.Parameters, Is.Not.Null);
            Assert.That(query.Parameters, Is.Not.Empty);
            Assert.That(query.Parameters, Has.Count.EqualTo(1));

            Assert.That(query.Parameters[0].SqlType.ToLower(), Is.EqualTo("varchar(255)"));
        }

        [Test]
        public void Deserialize_QueryWithRemovedParameter_QueryXml()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(query.Parameters, Is.Not.Null);
            Assert.That(query.Parameters, Is.Not.Empty);
            Assert.That(query.Parameters, Has.Count.EqualTo(1));
            Assert.That(query.Parameters[0].IsRemoved, Is.True);

            Assert.That(query.GetParameters(), Has.Count.EqualTo(0));
        }
        
        //[Test]
        //public void Deserialize_OneRowQuery_OneRowQueryXml()
        //{
        //    int testNr = 4;

        //    // Create an instance of the XmlSerializer specifying type and namespace.
        //    TestSuiteXml ts = DeserializeSample();

        //    var query = ((EqualToXml)ts.Tests[testNr].Constraints[0]).BaseItem;

        //    Assert.IsInstanceOf<OneRowQueryXml>(query);
        //}

        [Test]
        public void Serialize_InlineQuery_UseCData()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            var ctrXml = new EqualToXml();
            var queryXml = new QueryXml
            {
                ConnectionString = "my connection-string",
                InlineQuery = "select * from table"
            };
            ctrXml.Query = queryXml;

            var overrides = new WriteOnlyAttributes();
            overrides.Build();
            var serializer = new XmlSerializer(typeof(EqualToXml), overrides);
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, ctrXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("<![CDATA["));
            Assert.That(content, Is.StringContaining("select * from table"));
            Assert.That(content, Is.StringContaining("my connection-string"));
            Assert.That(content.Split(new[] { ' ' }), Has.Exactly(1).EqualTo("*"));
        }

    }
}
