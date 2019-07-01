using NBi.Core.ResultSet;
using NBi.Core.Transformation;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Items.Alteration;
using NBi.Xml.Items.Alteration.Conversion;
using NBi.Xml.Items.Alteration.Renaming;
using NBi.Xml.Items.Alteration.Transform;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.SerializationOption;
using NBi.Xml.Systems;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Xml.Systems
{
    [TestFixture]
    public class ResultSetSystemXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.ResultSetSystemXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_CsvFile()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.File.Path, Is.EqualTo("myFile.csv"));
        }

        [Test]
        public void Deserialize_SampleFileWithParser_CsvFile()
        {
            int testNr = 10;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.File.Path, Is.EqualTo("myFile.csv"));
            Assert.That(rs.File.Parser.Name, Is.EqualTo("tabular"));
        }

        [Test]
        public void Deserialize_SampleFileWithParserInline_CsvFile()
        {
            int testNr = 11;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.File.Path, Is.EqualTo("myFile.csv"));
            Assert.That(rs.File.Parser.Name, Is.EqualTo("tabular"));
        }

        [Test]
        public void Deserialize_SampleFile_EmbeddedResultSet()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.Content, Is.Not.Null);
            Assert.That(rs.Content.Rows, Has.Count.EqualTo(2));
        }

        [Test]
        public void Deserialize_SampleFile_QueryFile()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.Query, Is.Not.Null);
            Assert.That(rs.Query.File, Is.EqualTo("myfile.sql"));
        }

        [Test]
        public void Deserialize_SampleFile_EmbeddedQuery()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.Query, Is.Not.Null);
            Assert.That(rs.Query.File, Is.Null.Or.Empty);

            Assert.That(rs.Query.InlineQuery, Is.EqualTo("select * from myTable;"));
        }

        [Test]
        public void Deserialize_SampleFile_AssemblyQuery()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.Query, Is.Not.Null);
            Assert.That(rs.Query.Assembly, Is.Not.Null);

            Assert.That(rs.Query.Assembly.Path, Is.EqualTo("NBi.Testing.dll"));
        }

        [Test]
        public void Deserialize_SampleFile_ReportQuery()
        {
            int testNr = 5;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.Query, Is.Not.Null);
            Assert.That(rs.Query.Report, Is.Not.Null);

            Assert.That(rs.Query.Report.Name, Is.EqualTo("MyReport"));
        }

        [Test]
        public void Deserialize_SampleFile_AlterationFilter()
        {
            int testNr = 6;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.Alteration, Is.Not.Null);
            Assert.That(rs.Alteration.Filters, Is.Not.Null);
            Assert.That(rs.Alteration.Filters, Has.Count.EqualTo(1));

            Assert.That(rs.Alteration.Filters[0].Predication, Is.Not.Null);
        }

        [Test]
        public void Deserialize_SampleFile_AlterationConvert()
        {
            int testNr = 7;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.Alteration, Is.Not.Null);
            Assert.That(rs.Alteration.Conversions, Is.Not.Null);
            Assert.That(rs.Alteration.Conversions, Has.Count.EqualTo(1));

            Assert.That(rs.Alteration.Conversions[0], Is.Not.Null);
            Assert.That(rs.Alteration.Conversions[0], Is.TypeOf<ConvertXml>());

            Assert.That(rs.Alteration.Conversions[0].Column, Is.EqualTo("#0"));
            Assert.That(rs.Alteration.Conversions[0].Converter, Is.TypeOf<TextToDateConverterXml>());
            Assert.That(rs.Alteration.Conversions[0].Converter.Culture, Is.EqualTo("fr-fr"));
            Assert.That(rs.Alteration.Conversions[0].Converter.DefaultValue, Is.Null);
        }

        [Test]
        public void Deserialize_SampleFile_AlterationTransformation()
        {
            int testNr = 8;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.Alteration, Is.Not.Null);
            Assert.That(rs.Alteration.Transformations, Is.Not.Null);
            Assert.That(rs.Alteration.Transformations, Has.Count.EqualTo(1));

            Assert.That(rs.Alteration.Transformations[0], Is.Not.Null);
            Assert.That(rs.Alteration.Transformations[0], Is.TypeOf<TransformXml>());

            Assert.That(rs.Alteration.Transformations[0].Language, Is.EqualTo(LanguageType.CSharp));
            Assert.That(rs.Alteration.Transformations[0].OriginalType, Is.EqualTo(ColumnType.Text));
            Assert.That(rs.Alteration.Transformations[0].Identifier.Label, Is.EqualTo("#1"));
            Assert.That(rs.Alteration.Transformations[0].Identifier, Is.TypeOf<ColumnOrdinalIdentifier>());
            Assert.That((rs.Alteration.Transformations[0].Identifier as ColumnOrdinalIdentifier).Ordinal, Is.EqualTo(1));
            Assert.That(rs.Alteration.Transformations[0].Code.Trim(), Is.EqualTo("value.EndsWith(\".\") ? value : value + \".\""));
        }

        [Test]
        public void Deserialize_SampleFile_AlterationRename()
        {
            int testNr = 9;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.Alteration, Is.Not.Null);
            Assert.That(rs.Alteration.Renamings, Is.Not.Null);
            Assert.That(rs.Alteration.Renamings, Has.Count.EqualTo(1));

            Assert.That(rs.Alteration.Renamings[0].Identifier.Label, Is.EqualTo("#3"));
            Assert.That(rs.Alteration.Renamings[0].NewName.Label, Is.EqualTo("[myNewName]"));
        }

        [Test]
        public void Serialize_FileAndParser_Correct()
        {
            var root = new ResultSetSystemXml()
            {
                File = new FileXml()
                {
                    Path = "myFile.csv",
                    Parser = new ParserXml()
                    {
                        Name = "myParser",
                    }
                }
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(root);
            Console.WriteLine(xml);
            Assert.That(xml, Is.StringContaining("<file>"));
            Assert.That(xml, Is.StringContaining("<path>myFile.csv</path>"));
            Assert.That(xml, Is.StringContaining("<parser name=\"myParser\" />"));
            Assert.That(xml, Is.StringContaining("</file>"));
        }

        [Test]
        public void Serialize_InlineFileAndParser_Correct()
        {
            var root = new ResultSetSystemXml()
            {
#pragma warning disable 0618
                FilePath = "myFile.csv!myParser",
#pragma warning restore 0618
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(root);
            Console.WriteLine(xml);
            Assert.That(xml, Is.StringContaining("<file>"));
            Assert.That(xml, Is.StringContaining("<path>myFile.csv</path>"));
            Assert.That(xml, Is.StringContaining("<parser name=\"myParser\" />"));
            Assert.That(xml, Is.StringContaining("</file>"));
        }

        [Test]
        public void Serialize_InlineFileWithoutParser_Correct()
        {
            var root = new ResultSetSystemXml()
            {
#pragma warning disable 0618
                FilePath = "myFile.csv",
#pragma warning restore 0618
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(root);
            Console.WriteLine(xml);
            Assert.That(xml, Is.StringContaining("<file>"));
            Assert.That(xml, Is.StringContaining("<path>myFile.csv</path>"));
            Assert.That(xml, Is.Not.StringContaining("<parser"));
            Assert.That(xml, Is.StringContaining("</file>"));
        }

        [Test]
        public void Serialize_Renaming_Correct()
        {
            var root = new ResultSetSystemXml()
            {
                File = new FileXml() { Path=@"C:\Temp\foo.txt" },
                Alteration = new AlterationXml()
                {
                    Renamings = new List<RenamingXml>() { new RenamingXml()
                    { Identifier= new ColumnOrdinalIdentifier(5), NewName = new ColumnNameIdentifier("myNewName") } }
                }
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(root);
            Console.WriteLine(xml);
            Assert.That(xml, Is.StringContaining("<rename"));
            Assert.That(xml, Is.StringContaining("#5"));
            Assert.That(xml, Is.StringContaining("[myNewName]"));
        }

    }
}
