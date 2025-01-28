using NBi.Xml;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Xml.Variables;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using NBi.Xml.Items;
using System.Reflection;
using NBi.Core.ResultSet;
using NBi.Xml.Variables.Sequence;
using NBi.Core.Calculation;

namespace NBi.Xml.Testing.Unit.Variables.Sequence;

public class QuerySequenceXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_VariableHasFileLoop()
    {
        var ts = DeserializeSample();
        var variable = ts.Tests[0].InstanceSettling.Variable as InstanceVariableXml;

        // Check the properties of the object.
        Assert.That(variable.Query, Is.Not.Null);
        Assert.That(variable.Query, Is.TypeOf<QueryXml>());
    }

    [Test]
    public void Deserialize_SampleFile_VariableHasCorrectNameAndType()
    {
        var ts = DeserializeSample();
        var variable = ts.Tests[0].InstanceSettling.Variable;

        // Check the properties of the object.
        Assert.That(variable.Query!.InlineQuery, Does.Contain(@"select [mycolumn] from myTable where [myFilter]=@filter"));
        Assert.That(variable.Query.Parameters, Has.Count.EqualTo(1));
    }
}
