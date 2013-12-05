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
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.MatchPatternXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_SampleFile_MatchPatternRegex()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<MatchPatternXml>());
            var matchPatternConstraint = (MatchPatternXml)ts.Tests[testNr].Constraints[0];
            
            Assert.That(matchPatternConstraint.Regex, Is.EqualTo(@"^[2-9]\d{2}-\d{3}-\d{4}$"));
        }
    }
}
