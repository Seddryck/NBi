using System;
using NBi.Xml;
using NUnit.Framework;
using System.Xml;

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class XmlManagerWithDtdTest
    {
        private string filename { get; set; }
        
        [SetUp]
        public void Setup()
        {
            DiskOnFile.CreatePhysicalFile("TestSuiteIncludedTestSuite.nbiinclude", "NBi.Testing.Unit.Xml.Resources.TestSuiteIncludedTestSuite.xml");
            filename = DiskOnFile.CreatePhysicalFile("TestSuiteWithIncludeTestSuite.nbits", "NBi.Testing.Unit.Xml.Resources.TestSuiteWithIncludeTestSuite.xml");
        }
            
        [Test]
        public void Load_ValidFile_Success()
        {
            var manager = new XmlManager();
            manager.Load(filename, true);

            Assert.That(manager.TestSuite, Is.Not.Null);
        }

        [Test]
        public void Load_ValidFile_TwoTestsLoaded()
        {
            var manager = new XmlManager();
            manager.Load(filename, true);

            Assert.That(manager.TestSuite.Tests, Has.Count.EqualTo(2));
        }

        [Test]
        public void Load_ValidFileButWithoutDtdProcessingSetToTrue_Successfully()
        {
            var manager = new XmlManager();
            Assert.Throws<XmlException>(delegate { manager.Load(filename, false); });
        }
    }
}
