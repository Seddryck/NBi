using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.SerializationOption;
using NBi.Xml.Systems;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Xml.Testing.Unit.Items;

public class FileXmlTest
{
    [Test]
    public void Serialize_JustFileName_NoElementForParser()
    {
        var root = new ResultSetSystemXml()
        {
            File = new FileXml
            {
                Path = "c:\\myFile.txt",
            }
        };

        var overrides = new WriteOnlyAttributes();
        overrides.Build();

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root, overrides);
        Assert.That(xml, Does.Contain("<file>"));
        Assert.That(xml, Does.Contain("<path>c:\\myFile.txt</path>"));
        Assert.That(xml, Does.Contain("</file>"));
        Assert.That(xml, Does.Not.Contain("<parser"));
        Assert.That(xml, Does.Not.Contain("<if-missing"));
    }

    [Test]
    public void Serialize_FileWithParser_NoAttributeTwoElements()
    {
        var root = new ResultSetSystemXml()
        {
            File = new FileXml
            {
                Path = "c:\\myFile.txt",
                Parser = new ParserXml() { Name = "myName" }
            }
        };

        var overrides = new WriteOnlyAttributes();
        overrides.Build();

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root, overrides);
        Assert.That(xml, Does.Contain("<file>"));
        Assert.That(xml, Does.Contain("<path>c:\\myFile.txt</path>"));
        Assert.That(xml, Does.Contain("<parser name=\"myName\" />"));
        Assert.That(xml, Does.Contain("</file>"));
    }

    [Test]
    public void Serialize_FileWithIfMissing_NoAttributeTwoElements()
    {
        var root = new ResultSetSystemXml()
        {
            File = new FileXml
            {
                Path = "c:\\myFile.txt",
                IfMissing = new IfMissingXml() { File = new FileXml() { Path = "C:\\myOtherFile.txt"} },
            }
        };

        var overrides = new WriteOnlyAttributes();
        overrides.Build();

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root, overrides);
        Assert.That(xml, Does.Contain("<file>"));
        Assert.That(xml, Does.Contain("<path>c:\\myFile.txt</path>"));
        Assert.That(xml, Does.Contain("<if-missing"));
        Assert.That(xml, Does.Contain(">C:\\myOtherFile.txt<"));
        Assert.That(xml, Does.Contain("</file>"));
    }
}
