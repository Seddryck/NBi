using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class MatchPatternXmlTest
    {
        protected TestSuiteXml DeserializeSample(string descriminator)
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream(
                                           string.Format("NBi.Testing.Unit.Xml.Resources.MatchPatternXml{0}TestSuite.xml", descriminator)))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            manager.ApplyDefaultSettings();
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_SampleFile_MatchPatternRegex()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("");

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<MatchPatternXml>());
            var matchPatternConstraint = (MatchPatternXml)ts.Tests[testNr].Constraints[0];
            
            Assert.That(matchPatternConstraint.Regex, Is.EqualTo(@"^[2-9]\d{2}-\d{3}-\d{4}$"));
        }

        [Test]
        public void Deserialize_SampleFile_MatchPatternNumericFormat()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("WithReference");

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<MatchPatternXml>());
            var matchPatternConstraint = (MatchPatternXml)ts.Tests[testNr].Constraints[0];

            Assert.That(matchPatternConstraint.NumericFormat, Is.Not.Null);
            Assert.That(matchPatternConstraint.NumericFormat.DecimalDigits, Is.EqualTo(3));
            Assert.That(matchPatternConstraint.NumericFormat.DecimalSeparator, Is.EqualTo(","));
            Assert.That(matchPatternConstraint.NumericFormat.GroupSeparator, Is.EqualTo(" "));
        }
    }
}
