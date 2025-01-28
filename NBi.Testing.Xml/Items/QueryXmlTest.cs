using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Xml.Constraints;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
using NBi.Xml.SerializationOption;

namespace NBi.Xml.Testing.Unit.Items;

[TestFixture]
public class QueryXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_QueryWithoutParams_QueryXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<QueryXml>());
        var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(query, Is.Not.Null);
        Assert.That(query.InlineQuery, Does.Contain("select top 1 myColumn from myTable"));
        Assert.That(query.Parameters, Has.Count.EqualTo(0));
    }

    [Test]
    public void Deserialize_QueryConnectionString_QueryXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<QueryXml>());
        var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(query.ConnectionString, Is.Not.Null);
        Assert.That(query.ConnectionString, Is.EqualTo("myConnectionString"));
    }

    [Test]
    public void Deserialize_QueryConnectionStringNewSyntax_QueryXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<QueryXml>());
        var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(query.ConnectionString, Is.Not.Null);
        Assert.That(query.ConnectionString, Is.EqualTo("myConnectionString"));
    }

    [Test]
    public void Deserialize_QueryWithTwoParams_QueryXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(query.InlineQuery, Does.Contain("select myColumn from myTable where myFirstField=@FirstField and 1=@NonEmpty"));

        Assert.That(query.Parameters, Is.Not.Null);
        Assert.That(query.Parameters, Is.Not.Empty);
        Assert.That(query.Parameters, Has.Count.EqualTo(2));

        Assert.That(query.Parameters[0].Name, Is.EqualTo("FirstField"));
        Assert.That(query.Parameters[0].GetValue<string>(), Is.EqualTo("Identifier"));

        Assert.That(query.Parameters[1].Name, Is.EqualTo("NonEmpty"));
        Assert.That(query.Parameters[1].GetValue<int>(), Is.EqualTo(1));
    }

    [Test]
    public void Deserialize_QueryWithOneParamAndSqlType_QueryXml()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(query.Parameters, Is.Not.Null);
        Assert.That(query.Parameters, Is.Not.Empty);
        Assert.That(query.Parameters, Has.Count.EqualTo(1));

        Assert.That(query.Parameters[0].SqlType.ToLower(), Is.EqualTo("varchar(255)"));
    }

    [Test]
    public void Deserialize_QueryWithRemovedParameter_QueryXml()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(query.Parameters, Is.Not.Null);
        Assert.That(query.Parameters, Is.Not.Empty);
        Assert.That(query.Parameters, Has.Count.EqualTo(1));
        Assert.That(query.Parameters[0].IsRemoved, Is.True);

        Assert.That(query.GetParameters(), Has.Count.EqualTo(0));
    }
    
    //[Test]
    //public void Deserialize_OneRowQuery_OneRowQueryXml()
    //{
    //    int testNr = 4;

    //    // Create an instance of the XmlSerializer specifying type and namespace.
    //    var ts = DeserializeSample();

    //    var query = ((EqualToXml)ts.Tests[testNr].Constraints[0]).BaseItem;

    //    Assert.That(, Is.InstanceOf<OneRowQueryXml>(query);
    //}

    [Test]
    public void Serialize_InlineQuery_UseCData()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ctrXml = new EqualToXml();
        var queryXml = new QueryXml
        {
            ConnectionString = "my connection*string",
            InlineQuery = "select * from table"
        };
        ctrXml.Query = queryXml;

        var overrides = new WriteOnlyAttributes();
        overrides.Build();
        var serializer = new XmlSerializer(typeof(EqualToXml), overrides);
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, ctrXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("<![CDATA["));
        Assert.That(content, Does.Contain("select * from table"));
        Assert.That(content, Does.Contain("connection-string="));
        Assert.That(content, Does.Not.Contain("connectionString="));
        Assert.That(content, Does.Contain("my connection*string"));
        Assert.That(content.Split([' ']), Has.Exactly(1).EqualTo("*"));
    }

}
