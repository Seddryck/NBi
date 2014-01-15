using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class SubsetOfXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.SubsetOfXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_SubsetOfNotIgnoringCaseImplicitely()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SubsetOfXml>());
            Assert.That(((SubsetOfXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_SubsetOfCaptionIgnoringCaseExplicitely()
        {
            int testNr = 1;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(((SubsetOfXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_SubsetOfReadItems()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(((SubsetOfXml)ts.Tests[testNr].Constraints[0]).Items, Has.Count.EqualTo(2));
            Assert.That(((SubsetOfXml)ts.Tests[testNr].Constraints[0]).Items[0], Is.EqualTo("First hierarchy"));
            Assert.That(((SubsetOfXml)ts.Tests[testNr].Constraints[0]).Items[1], Is.EqualTo("Second hierarchy"));
        }

        [Test]
        public void Deserialize_SampleFile_SubsetOfMembers()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(((SubsetOfXml)ts.Tests[testNr].Constraints[0]).Members, Is.InstanceOf<MembersXml>());

            var members = ((SubsetOfXml)ts.Tests[testNr].Constraints[0]).Members;
            Assert.That(members.ChildrenOf, Is.EqualTo("All"));
            Assert.That(((HierarchyXml)(members.BaseItem)).Caption, Is.EqualTo("myHierarchy"));
        }
    }
}
