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
            var includedFilename = DiskOnFile.CreatePhysicalFile("TestSuiteIncludedTestSuite.xml", "NBi.Testing.Unit.Xml.Resources.TestSuiteIncludedTestSuite.xml");
            Console.WriteLine("Included file created at '{0}'", includedFilename);
            filename = DiskOnFile.CreatePhysicalFile("TestSuiteWithIncludeTestSuite.nbits", "NBi.Testing.Unit.Xml.Resources.TestSuiteWithIncludeTestSuite.xml");
            Console.WriteLine("Main file created at '{0}'", filename);
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
        public void Load_ValidFileInSubFolder_TwoTestsLoaded()
        {
            var includedFilename = DiskOnFile.CreatePhysicalFile(@"Dtd\TestSuiteIncludedTestSuite.xml", "NBi.Testing.Unit.Xml.Resources.TestSuiteIncludedTestSuite.xml");
            Console.WriteLine("Included file created at '{0}'", includedFilename);
            filename = DiskOnFile.CreatePhysicalFile(@"Dtd\TestSuiteWithIncludeTestSuite.nbits", "NBi.Testing.Unit.Xml.Resources.TestSuiteWithIncludeTestSuite.xml");
            Console.WriteLine("Main file created at '{0}'", filename);

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
