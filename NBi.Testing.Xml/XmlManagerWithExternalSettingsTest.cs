using System;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class XmlManagerWithExternalSettingsTest
{
    private string testSuite { get; set; } = string.Empty;
    private string settings { get; set; } = string.Empty;
    
    [SetUp]
    public void Setup()
    {
        testSuite = FileOnDisk.CreatePhysicalFile("TestSuiteForExternalSettings.nbits", $"{GetType().Assembly.GetName().Name}.Resources.TestSuiteForExternalSettings.xml");
        settings = FileOnDisk.CreatePhysicalFile("SettingsExternal.nbiset", $"{GetType().Assembly.GetName().Name}.Resources.SettingsExternal.xml");
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

        Assert.That(manager.TestSuite!.Settings, Is.Not.Null);
        //defaults
        Assert.That(manager.TestSuite.Settings.Defaults, Is.Not.Null);
        Assert.That(manager.TestSuite.Settings.GetDefault(NBi.Xml.Settings.SettingsXml.DefaultScope.SystemUnderTest), Is.Not.Null);
        Assert.That(manager.TestSuite.Settings.GetDefault(NBi.Xml.Settings.SettingsXml.DefaultScope.SystemUnderTest).ConnectionString.Inline, Is.EqualTo("My Sut Default Connection String"));

        //references
        Assert.That(manager.TestSuite.Settings.References, Is.Not.Null);
        Assert.That(manager.TestSuite.Settings.GetReference("MyReference"), Is.Not.Null);
        Assert.That(manager.TestSuite.Settings.GetReference("MyReference").ConnectionString.Inline, Is.EqualTo("My Reference Connection String"));
    }

    [Test]
    public void Load_ValidFileRelativePath_SettingsLoaded()
    {
        var relativePath = System.IO.Path.GetFileName(settings);

        var manager = new XmlManager();
        manager.Load(testSuite, relativePath, false);

        Assert.That(manager.TestSuite!.Settings, Is.Not.Null);
        //defaults
        Assert.That(manager.TestSuite.Settings.Defaults, Is.Not.Null);
        Assert.That(manager.TestSuite.Settings.GetDefault(NBi.Xml.Settings.SettingsXml.DefaultScope.SystemUnderTest), Is.Not.Null);
        Assert.That(manager.TestSuite.Settings.GetDefault(NBi.Xml.Settings.SettingsXml.DefaultScope.SystemUnderTest).ConnectionString.Inline, Is.EqualTo("My Sut Default Connection String"));

        //references
        Assert.That(manager.TestSuite.Settings.References, Is.Not.Null);
        Assert.That(manager.TestSuite.Settings.GetReference("MyReference"), Is.Not.Null);
        Assert.That(manager.TestSuite.Settings.GetReference("MyReference").ConnectionString.Inline, Is.EqualTo("My Reference Connection String"));
    }

    [Test]
    public void Load_NotExistingSettingFile_ArgumentException()
    {
        var manager = new XmlManager();
        Assert.Throws<ArgumentException>(delegate { manager.Load(testSuite, "NotFoundSettings.nbiset", false); });
    }
}
