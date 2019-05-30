#region Using directives
using System.IO;
using System.Reflection;
using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Core.Transformation;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Items.Alteration.Transform;
using NBi.Xml.Items.Alteration;
using System.Collections.Generic;
using System;
#endregion

namespace NBi.Testing.Unit.Xml.Items.ResultSet
{
    [TestFixture]
    public class TransformXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.TransformXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_CSharp_CSharpAndCode()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
            var transfo = ((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0].Transformation;



            Assert.That(transfo.Language, Is.EqualTo(LanguageType.CSharp));
            Assert.That(transfo.Code.Trim, Is.EqualTo("value.Trim().ToUpper();"));
        }

        [Test]
        public void Deserialize_Native_NativeAndNativeTransfo()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
            var transfo = ((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0].Transformation;

            Assert.That(transfo.Language, Is.EqualTo(LanguageType.Native));
            Assert.That(transfo.Code, Is.EqualTo("empty-to-null"));
        }

        [Test]
        public void Deserialize_OldValueTransformation_CorrectlyRead()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
            var transfo = ((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0].Transformation;

            Assert.That(transfo.Language, Is.EqualTo(LanguageType.Native));
            Assert.That(transfo.Code, Is.EqualTo("empty-to-null"));
        }

        [Test]
        public void Serialize_CSharp_CodeTransfo()
        {
            var def = new ColumnDefinitionXml()
            {
                TransformationInner = new LightTransformXml()
                {
                    Language = LanguageType.CSharp,
                    Code = "value.Trim().ToUpper();"
                }
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<ColumnDefinitionXml>(def);

            Assert.That(xml, Is.StringContaining(">value.Trim().ToUpper();<"));
            Assert.That(xml, Is.Not.StringContaining("column-index"));
        }

        [Test]
        public void Serialize_Format_CodeTransfo()
        {
            var def = new ColumnDefinitionXml()
            {
                TransformationInner = new LightTransformXml()
                {
                    Language = LanguageType.Format,
                    Code = "##.000"
                }
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<ColumnDefinitionXml>(def);

            Assert.That(xml, Is.StringContaining("format"));
            Assert.That(xml, Is.StringContaining(">##.000<"));
            Assert.That(xml, Is.Not.StringContaining("column-index"));
        }

        [Test]
        public void Serialize_Native_CodeTransfo()
        {
            var def = new ColumnDefinitionXml()
            {
                TransformationInner = new LightTransformXml()
                {
                    Language = LanguageType.Native,
                    Code = "empty-to-null"
                }
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<ColumnDefinitionXml>(def);

            Assert.That(xml, Is.StringContaining("native"));
            Assert.That(xml, Is.StringContaining(">empty-to-null<"));
            Assert.That(xml, Is.Not.StringContaining("column-index"));
        }

        [Test]
        public void Serialize_AlterTransform_OrdinalCorrect()
        {
            var root = new AlterationXml()
            {
                Transformations = new List<TransformXml>()
                {
                    new TransformXml()
                    {
                        Identifier = new ColumnOrdinalIdentifier(2),
                        Language = LanguageType.Native,
                        Code = "empty-to-null"
                    }
                }
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(root);
            Console.WriteLine(xml);
            Assert.That(xml, Is.StringContaining("<transform "));
            Assert.That(xml, Is.StringContaining("column=\"#2\""));
        }

        [Test]
        public void Serialize_AlterTransform_IdentifierCorrect()
        {
            var root = new AlterationXml()
            {
                Transformations = new List<TransformXml>()
                {
                    new TransformXml()
                    {
                        Identifier = new ColumnIdentifierFactory().Instantiate("[MyName]"),
                        Language = LanguageType.Native,
                        Code = "empty-to-null"
                    }
                }
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(root);
            Console.WriteLine(xml);
            Assert.That(xml, Is.StringContaining("<transform "));
            Assert.That(xml, Is.StringContaining("column=\"[MyName]\""));
        }
    }
}
