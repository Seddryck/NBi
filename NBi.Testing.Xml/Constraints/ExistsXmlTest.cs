#region Using directives
using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;
#endregion

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class ExistsXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_ExistsConstraintWithIgnoreCaseTrue()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ExistsXml>());
        Assert.That(((ExistsXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.True);
    }

    [Test]
    public void Deserialize_SampleFile_ExistsConstraintWithIgnoreCaseFalseImplicitely()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ExistsXml>());
        Assert.That(((ExistsXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.False);
    }

    

}
