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
            var manager = new XmlManager();
            manager.Load(@"Xml\TestSuiteSample.xml");

            Assert.That(manager.TestSuite, Is.Not.Null);
        }
    }
}
