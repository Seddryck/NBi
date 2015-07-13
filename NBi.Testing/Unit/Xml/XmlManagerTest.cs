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
            var filename = DiskOnFile.CreatePhysicalFile("TestSuite.xml", "NBi.Testing.Unit.Xml.Resources.XmlManagerSample.xml");
            
            var manager = new XmlManager();
            manager.Load(filename);

            Assert.That(manager.TestSuite, Is.Not.Null);
        }

        [Test]
        public void Load_ValidFile_TestContentIsCorrect()
        {
            var filename = DiskOnFile.CreatePhysicalFile("TestContentIsCorrect.xml", "NBi.Testing.Unit.Xml.Resources.XmlManagerSample.xml");

            var manager = new XmlManager();
            manager.Load(filename);

            Assert.That(manager.TestSuite.Tests[0].Content, Is.Not.Null);
            Assert.That(manager.TestSuite.Tests[0].Content, Is.StringEnding("</test>"));
        }

        [Test]
        public void Load_InvalidFormat_ThrowException()
        {
            var filename = DiskOnFile.CreatePhysicalFile("InvalidFormat.nbits", "NBi.Testing.Unit.Xml.Resources.XmlManagerInvalidFormat.xml");

            var manager = new XmlManager();
            var ex = Assert.Throws<ArgumentException>(delegate { manager.Load(filename); });
            Assert.That(ex.Message, Is.StringContaining("At line 14"));
        }

        [Test]
        public void Load_InvalidFile_ThrowException()
        {
            var filename = DiskOnFile.CreatePhysicalFile("TestSuiteInvalidSyntax.xml", "NBi.Testing.Unit.Xml.Resources.XmlManagerInvalidSyntax.xml");

            var manager = new XmlManager();
            Assert.Throws<ArgumentException>(delegate { manager.Load(filename); });
        }

        [Test]
        public void Load_InvalidFile_ExceptionHasCorrectInformation()
        {
            var filename = DiskOnFile.CreatePhysicalFile("TestSuiteInvalidSyntax.xml", "NBi.Testing.Unit.Xml.Resources.XmlManagerInvalidSyntax.xml");

            var manager = new XmlManager();
            var exception = Assert.Throws<ArgumentException>(delegate { manager.Load(filename); });
            Assert.That(exception.Message, Is.StringContaining("1 error has been found during the validation of the test-suite"));
            Assert.That(exception.Message, Is.StringContaining("\tAt line 4: The element 'test' in namespace 'http://NBi/TestSuite' has invalid child element 'syntacticallyCorrect' in namespace 'http://NBi/TestSuite'."));
        }

        [Test]
        public void Load_InvalidMultipleFile_ThrowException()
        {
            var filename = DiskOnFile.CreatePhysicalFile("TestSuiteInvalidSyntaxMultiple.xml", "NBi.Testing.Unit.Xml.Resources.XmlManagerInvalidSyntaxMultiple.xml");

            var manager = new XmlManager();
            Assert.Throws<ArgumentException>(delegate { manager.Load(filename); });
        }

        [Test]
        public void Load_InvalidMultipleFile_ExceptionHasCorrectInformation()
        {
            var filename = DiskOnFile.CreatePhysicalFile("TestSuiteInvalidSyntaxMultiple.xml", "NBi.Testing.Unit.Xml.Resources.XmlManagerInvalidSyntaxMultiple.xml");

            var manager = new XmlManager();
            var exception = Assert.Throws<ArgumentException>(delegate { manager.Load(filename); });
            Assert.That(exception.Message, Is.StringContaining("3 errors have been found during the validation of the test-suite"));
            Assert.That(exception.Message, Is.StringContaining("At line 6: The element 'execution' in namespace 'http://NBi/TestSuite' has invalid child element 'sql' in namespace 'http://NBi/TestSuite'."));
            Assert.That(exception.Message, Is.StringContaining("At line 11: The 'name' attribute is not declared."));
            Assert.That(exception.Message, Is.StringContaining("At line 11: The 'http://NBi/TestSuite:less-than' element is invalid - The value 'alpha' is invalid according to its datatype 'http://NBi/TestSuite:more-less-type' - The string 'alpha' is not a valid Integer value."));
        }
    }
}
