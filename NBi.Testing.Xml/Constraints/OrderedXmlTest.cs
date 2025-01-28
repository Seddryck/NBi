using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class OrderedXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_OrderedConstraintAlphabeticalNothingSpecified()
    {
        int testNr = 0;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<OrderedXml>());
        Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Rule, Is.EqualTo(OrderedXml.Order.Alphabetical));
        Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Descending, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_OrderedConstraintAlphabeticalDescendingSpecified()
    {
        int testNr = 1;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<OrderedXml>());
        Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Descending, Is.True);
    }

    [Test]
    public void Deserialize_SampleFile_OrderedConstraintChronologicalSpecified()
    {
        int testNr = 2;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<OrderedXml>());
        Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Rule, Is.EqualTo(OrderedXml.Order.Chronological));
    }

    [Test]
    public void Deserialize_SampleFile_OrderedConstraintAlphabeticalSpecificSpecified()
    {
        int testNr = 3;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<OrderedXml>());
        Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Rule, Is.EqualTo(OrderedXml.Order.Specific));
        Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Definition, Has.Count.EqualTo(3));
        Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Definition[0], Is.EqualTo("Leopold"));
    }

    [Test]
    public void Deserialize_SampleFile_OrderedConstraintSpecificSpecifiedAndOneColumnQuery()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<OrderedXml>());
        Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Rule, Is.EqualTo(OrderedXml.Order.Specific));
        Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Definition, Is.Null.Or.Empty);
        Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).DefinitionSpecified, Is.False);
        Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Query, Is.Not.Null);
    }
}
