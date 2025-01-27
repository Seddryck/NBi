using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Constraints.Comparer;
using NBi.Xml.Items.Calculation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Testing.Unit.Items.Calculation;

public class PredicateXmlTest : BaseXmlTest
{ 

    [Test]
    public void Deserialize_OnlyOperandNoName_PredicateXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<AllRowsXml>());
        var ctr = (AllRowsXml)ts.Tests[testNr].Constraints[0];
        Assert.That(ctr.Predication, Is.Not.Null);
        Assert.That(((ColumnNameIdentifier)ctr.Predication.Operand!).Name, Is.EqualTo("ModDepId"));
    }

    [Test]
    public void Deserialize_OnlyNameNoOperand_PredicateXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<AllRowsXml>());
        var ctr = (AllRowsXml)ts.Tests[testNr].Constraints[0];
        Assert.That(ctr.Predication, Is.Not.Null);
        Assert.That(((ColumnNameIdentifier)ctr.Predication.Operand!).Name, Is.EqualTo("ModDepId"));
    }

    [Test]
    public void Deserialize_CorrectPredicate_PredicateXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<AllRowsXml>());
        var ctr = (AllRowsXml)ts.Tests[testNr].Constraints[0];
        Assert.That(ctr.Predication, Is.Not.Null);
        Assert.That(ctr.Predication.Predicate, Is.TypeOf<MoreThanXml>());
    }

    [Test]
    public void Serialize_PredicateXml_OnlyOperandNoName()
    {
        var allRowsXml = new AllRowsXml
        {
            Predication = new SinglePredicationXml()
            {
                Operand = new ColumnOrdinalIdentifier(1),
                Predicate = new FalseXml()
            }
        };

        var serializer = new XmlSerializer(typeof(AllRowsXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, allRowsXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("operand"));
        Assert.That(content, Does.Not.Contain("name"));
    }

    [Test]
    public void Serialize_ModuloXml_AllPredicateInfoCorrectlySerialized()
    {
        var allRowsXml = new AllRowsXml
        {
            Predication = new SinglePredicationXml()
            {
                Operand = new ColumnOrdinalIdentifier(1),
                Predicate = new ModuloXml() { SecondOperand = "10", Reference = "5" }
            }
        };

        var serializer = new XmlSerializer(typeof(AllRowsXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, allRowsXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("<modulo"));
        Assert.That(content, Does.Contain("second-operand=\"10\""));
        Assert.That(content, Does.Contain(">5<"));
    }
}
