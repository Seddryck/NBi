#region Using directives
using System.IO;
using System.Reflection;
using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Xml.Testing.Unit.Items;

[TestFixture]
public class AssemblyXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_MethodWithoutParam_AssemblyXml()
    {
        int testNr = 0;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<AssemblyXml>());
        var assembly = (AssemblyXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(assembly, Is.Not.Null);
        Assert.That(assembly.Path, Is.EqualTo("NBi.Testing.dll"));
        Assert.That(assembly.Klass, Is.EqualTo("NBi.Testing.Unit.Acceptance.Resource.AssemblyClass"));
        Assert.That(assembly.Method, Is.EqualTo("GetSelectString"));
        Assert.That(assembly.MethodParameters, Has.Count.EqualTo(0));
    }

    [Test]
    public void Deserialize_MethodWithParamString_AssemblyXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<AssemblyXml>());
        var assembly = (AssemblyXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(assembly, Is.Not.Null);
        Assert.That(assembly.MethodParameters, Is.Not.Null);
        Assert.That(assembly.MethodParameters, Has.Count.EqualTo(1));

        Assert.That(assembly.MethodParameters[0].Name, Is.EqualTo("MyString"));
        Assert.That(assembly.MethodParameters[0].Value, Is.EqualTo("FirstValue"));
    }

    [Test]
    public void Deserialize_MethodWithParamDecimal_AssemblyXml()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<AssemblyXml>());
        var assembly = (AssemblyXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(assembly, Is.Not.Null);
        Assert.That(assembly.MethodParameters, Is.Not.Null);
        Assert.That(assembly.MethodParameters, Has.Count.EqualTo(1));

        Assert.That(assembly.MethodParameters[0].Name, Is.EqualTo("MyDecimal"));
        Assert.That(assembly.MethodParameters[0].Value, Is.EqualTo("10.52"));
    }

    [Test]
    public void Deserialize_MethodWithParamEnum_AssemblyXml()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<AssemblyXml>());
        var assembly = (AssemblyXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(assembly, Is.Not.Null);
        Assert.That(assembly.MethodParameters, Is.Not.Null);
        Assert.That(assembly.MethodParameters, Has.Count.EqualTo(1));

        Assert.That(assembly.MethodParameters[0].Name, Is.EqualTo("MyEnum"));
        Assert.That(assembly.MethodParameters[0].Value, Is.EqualTo("Beta"));
    }

    [Test]
    public void Deserialize_MethodWithParamDateTime_AssemblyXml()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<AssemblyXml>());
        var assembly = (AssemblyXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(assembly, Is.Not.Null);
        Assert.That(assembly.MethodParameters, Is.Not.Null);
        Assert.That(assembly.MethodParameters, Has.Count.EqualTo(1));

        Assert.That(assembly.MethodParameters[0].Name, Is.EqualTo("MyDateTime"));
        Assert.That(assembly.MethodParameters[0].Value, Is.EqualTo("2012-10-16 10:15"));
    }
    
    [Test]
    public void Deserialize_MethodWithConnectionStringInfo_AssemblyXml()
    {
        int testNr = 5;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<AssemblyXml>());
        var assembly = (AssemblyXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(assembly.ConnectionString, Is.EqualTo("data source=foo;initial catalog=bar"));
        Assert.That(assembly.Roles, Is.EqualTo("admin"));
        Assert.That(assembly.Timeout, Is.EqualTo(10));
        Assert.That(assembly.ConnectionString, Does.Contain("data source=foo;initial catalog=bar"));
        Assert.That(assembly.Roles, Does.Contain("admin"));
    }


}
