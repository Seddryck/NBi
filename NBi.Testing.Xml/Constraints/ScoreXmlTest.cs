#region Using directives
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;
using NBi.Xml.Constraints.Comparer;
using NBi.Core.ResultSet;
using NBi.Xml.Settings;
using NBi.Core.Evaluate;
using NBi.Xml.Items.Calculation;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
#endregion

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class ScoreXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyScore()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ScoreXml>());
        Assert.That(ts.Tests[testNr].Constraints[0].Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_DefaultThersholdIsOne()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ScoreXml>());
        Assert.That(((ScoreXml)ts.Tests[testNr].Constraints[0]).Threshold, Is.EqualTo(1));
    }

    [Test]
    public void Deserialize_SampleFile_DefaultThersholdIsValueAssigned()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ScoreXml>());
        Assert.That(((ScoreXml)ts.Tests[testNr].Constraints[0]).Threshold, Is.EqualTo(0.95));
    }
}
