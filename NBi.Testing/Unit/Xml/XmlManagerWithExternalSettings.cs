using System;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class XmlManagerWithExternalSettings
    {
        private string testSuite { get; set; }
        private string settings { get; set; }
        
        [SetUp]
        public void Setup()
        {
            testSuite = DiskOnFile.CreatePhysicalFile("TestSuiteForExternalSettings.nbits", "NBi.Testing.Unit.Xml.Resources.TestSuiteForExternalSettings.xml");
            settings = DiskOnFile.CreatePhysicalFile("SettingsExternal.nbiset", "NBi.Testing.Unit.Xml.Resources.SettingsExternal.xml");
        }
            
        [Test]
        public void Load_ValidFile_Success()
        {
            var manager = new XmlManager();
            manager.Load(testSuite, settings, false);

            Assert.That(manager.TestSuite, Is.Not.Null);
        }

        [Test]
        public void Load_ValidFile_SettingsLoaded()
        {
            var manager = new XmlManager();
            manager.Load(testSuite, settings, false);

            Assert.That(manager.TestSuite.Settings, Is.Not.Null);
            //defaults
            Assert.That(manager.TestSuite.Settings.Defaults, Is.Not.Null);
            Assert.That(manager.TestSuite.Settings.GetDefault(NBi.Xml.Settings.SettingsXml.DefaultScope.SystemUnderTest), Is.Not.Null);
            Assert.That(manager.TestSuite.Settings.GetDefault(NBi.Xml.Settings.SettingsXml.DefaultScope.SystemUnderTest).ConnectionString, Is.EqualTo("My Sut Default Connection String"));

            //references
            Assert.That(manager.TestSuite.Settings.References, Is.Not.Null);
            Assert.That(manager.TestSuite.Settings.GetReference("MyReference"), Is.Not.Null);
            Assert.That(manager.TestSuite.Settings.GetReference("MyReference").ConnectionString, Is.EqualTo("My Reference Connection String"));
        }

        [Test]
        public void Load_NotExistingSettingFile_ArgumentException()
        {
            var manager = new XmlManager();
            Assert.Throws<ArgumentException>(delegate { manager.Load(testSuite, "NotFoundSettings.nbiset", false); });
        }
    }
}
