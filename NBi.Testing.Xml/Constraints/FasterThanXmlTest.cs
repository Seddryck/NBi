#region Using directives
using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;
#endregion

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class FasterThanXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyParametersFasterThanConstraint()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<FasterThanXml>());
        Assert.That(((FasterThanXml)ts.Tests[testNr].Constraints[0]).CleanCache, Is.True);
        Assert.That(((FasterThanXml)ts.Tests[testNr].Constraints[0]).MaxTimeMilliSeconds, Is.EqualTo(100));
    }

    [Test]
    public void Deserialize_SampleFile_AcceptIntMaxValueValueForMaxTimeMilliSeconds()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<FasterThanXml>());
        Assert.That(((FasterThanXml)ts.Tests[testNr].Constraints[0]).MaxTimeMilliSeconds, Is.EqualTo(int.MaxValue));
    }

    [Test]
    public void Deserialize_SampleFile_DefaultValueForCleanCacheIsFalse()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<FasterThanXml>());
        Assert.That(((FasterThanXml)ts.Tests[testNr].Constraints[0]).CleanCache, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_DefaultValueForTimeOutMilliSecondsIsZero()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<FasterThanXml>());
        Assert.That(((FasterThanXml)ts.Tests[testNr].Constraints[0]).TimeOutMilliSeconds, Is.EqualTo(0));
    }

    [Test]
    public void Deserialize_SampleFile_ReadValueForTimeOutMilliSecondsIsZero()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<FasterThanXml>());
        Assert.That(((FasterThanXml)ts.Tests[testNr].Constraints[0]).TimeOutMilliSeconds, Is.EqualTo(10000));
    }

    

}
