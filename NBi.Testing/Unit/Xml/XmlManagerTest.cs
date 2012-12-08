using System;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class XmlManagerTest
    {
        [Test]
        public void Load_ValidFile_Success()
        {
            var filename = DiskOnFile.CreatePhysicalFile("TestSuite.xml", "NBi.Testing.Unit.Xml.Resources.TestSuiteSample.xml");
            
            var manager = new XmlManager();
            manager.Load(filename);

            Assert.That(manager.TestSuite, Is.Not.Null);
        }

        [Test]
        public void Load_ValidFile_TestContentIsCorrect()
        {
            var filename = DiskOnFile.CreatePhysicalFile("TestContentIsCorrect.xml", "NBi.Testing.Unit.Xml.Resources.TestSuiteSample.xml");

            var manager = new XmlManager();
            manager.Load(filename);

            Assert.That(manager.TestSuite.Tests[0].Content, Is.Not.Null);
        }

        [Test]
        public void Load_InvalidFile_Successfully()
        {
            var filename = DiskOnFile.CreatePhysicalFile("TestSuiteInvalidSyntax.xml", "NBi.Testing.Unit.Xml.Resources.TestSuiteInvalidSyntax.xml");

            var manager = new XmlManager();
            Assert.Throws<ArgumentException>(delegate { manager.Load(filename); });
        }
    }
}
