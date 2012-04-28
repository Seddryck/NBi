using NBi.Xml;
using NUnit.Framework;

namespace NBi.Testing.Xml
{
    [TestFixture]
    public class XmlManagerTest
    {
        [Test]
        public void XmlManager_Load_Successfully()
        {
            var filename = DiskOnFile.CreatePhysicalFile("TestSuite.xml", "NBi.Testing.Xml.Resources.TestSuiteSample.xml");
            
            var manager = new XmlManager();
            manager.Load(filename);

            Assert.That(manager.TestSuite, Is.Not.Null);
        }
    }
}
