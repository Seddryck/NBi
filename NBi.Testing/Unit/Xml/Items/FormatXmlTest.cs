using System.IO;
using System.Reflection;
using NBi.Core.Format;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items.Format;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    public class FormatXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.FormatXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_SampleFile_NumericFormat()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<MatchPatternXml>());

            NumericFormatXml format = ((MatchPatternXml)ts.Tests[testNr].Constraints[0]).NumericFormat;
            Assert.That(format.DecimalDigits, Is.EqualTo(4));
            Assert.That(format.DecimalSeparator, Is.EqualTo(","));
            Assert.That(format.GroupSeparator, Is.EqualTo(""));
        }

        [Test]
        public void Deserialize_SampleFile_CurrencyFormat()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<MatchPatternXml>());

            CurrencyFormatXml format = ((MatchPatternXml)ts.Tests[testNr].Constraints[0]).CurrencyFormat;
            Assert.That(format.DecimalDigits, Is.EqualTo(2));
            Assert.That(format.DecimalSeparator, Is.EqualTo("."));
            Assert.That(format.GroupSeparator, Is.EqualTo(","));
            Assert.That(format.CurrencyPattern, Is.EqualTo(CurrencyPattern.SuffixSpace));
            Assert.That(format.CurrencySymbol, Is.EqualTo("€"));
        }

        [Test]
        public void Deserialize_SampleFile_DefaultCurrencyFormat()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<MatchPatternXml>());

            CurrencyFormatXml format = ((MatchPatternXml)ts.Tests[testNr].Constraints[0]).CurrencyFormat;
            Assert.That(format.DecimalDigits, Is.EqualTo(2));
            Assert.That(format.DecimalSeparator, Is.EqualTo("."));
            Assert.That(format.GroupSeparator, Is.EqualTo(","));
            Assert.That(format.CurrencyPattern, Is.EqualTo(CurrencyPattern.Prefix));
            Assert.That(format.CurrencySymbol, Is.EqualTo("SEK"));
        }
               
    }
}
