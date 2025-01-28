using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class LinkedToXmlTest : BaseXmlTest
{
    [Test]
    public void Deserialize_SampleFile_LinkedToMeasureGroup()
    {
        int testNr = 0;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<LinkedToXml>());
        var ctr = (LinkedToXml)ts.Tests[testNr].Constraints[0];
        Assert.That(ctr.Item, Is.TypeOf<MeasureGroupXml>());
        Assert.That(ctr.Item.Caption, Is.EqualTo("measure-group"));
    }

    [Test]
    public void Deserialize_SampleFile_LinkedToDimension()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<LinkedToXml>());
        var ctr = (LinkedToXml)ts.Tests[testNr].Constraints[0];
        Assert.That(ctr.Item, Is.TypeOf<DimensionXml>());
        Assert.That(ctr.Item.Caption, Is.EqualTo("dimension"));
    }

}
