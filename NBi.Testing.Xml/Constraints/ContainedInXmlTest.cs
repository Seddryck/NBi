using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class ContainedInXmlTest : BaseXmlTest
{ 
    [Test]
    public void Deserialize_SampleFile_ContainedInNotIgnoringCaseImplicitely()
    {
        int testNr = 0;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainedInXml>());
        Assert.That(((ContainedInXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_SubsetOfNotIgnoringCaseImplicitely()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<ContainedInXml>());
        Assert.That(((ContainedInXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ContainedInCaptionIgnoringCaseExplicitely()
    {
        int testNr = 2;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(((ContainedInXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.True);
    }

    [Test]
    public void Deserialize_SampleFile_ContainedInReadItems()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(((ContainedInXml)ts.Tests[testNr].Constraints[0]).Items, Has.Count.EqualTo(2));
        Assert.That(((ContainedInXml)ts.Tests[testNr].Constraints[0]).Items[0], Is.EqualTo("First hierarchy"));
        Assert.That(((ContainedInXml)ts.Tests[testNr].Constraints[0]).Items[1], Is.EqualTo("Second hierarchy"));
    }

    [Test]
    public void Deserialize_SampleFile_ContainedInMembers()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(((ContainedInXml)ts.Tests[testNr].Constraints[0]).Members, Is.InstanceOf<MembersXml>());

        var members = ((ContainedInXml)ts.Tests[testNr].Constraints[0]).Members!;
        Assert.That(members.ChildrenOf, Is.EqualTo("All"));
        Assert.That(((HierarchyXml)(members.BaseItem)).Caption, Is.EqualTo("myHierarchy"));
    }

    [Test]
    public void Serialize_WithContainedIn_ContainedIn()
    {
        var testXml = new TestXml();

        var containedInXml = new ContainedInXml()
        {
            IgnoreCase=true,
            IntegerRange = new NBi.Xml.Items.Ranges.IntegerRangeXml()
            {
                Start = 10,
                End = 20,
                Step = 1
            }
        };
        testXml.Constraints.Add(containedInXml);

        var serializer = new XmlSerializer(typeof(TestXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, testXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("<contained-in"));
    }
}
