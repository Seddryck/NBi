using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Core.Etl;
using NBi.Xml;
using NBi.Xml.Decoration.Command;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit.Items;

[TestFixture]
public class EtlXmlTest
{
    protected TestSuiteXml DeserializeSample(string file)
    {
        // Declare an object variable of the type to be deserialized.
        var manager = new XmlManager();

        // A Stream is needed to read the XML document.
        using (var stream = Assembly.GetExecutingAssembly()
                                       .GetManifestResourceStream($"{GetType().Assembly.GetName().Name}.Resources.{file}Suite.xml")
                                           ?? throw new FileNotFoundException())
        using (var reader = new StreamReader(stream))
        {
            manager.Read(reader);
        }
        manager.ApplyDefaultSettings();
        return manager.TestSuite!;
    }

    [Test]
    public void Deserialize_EtlFromFileInSetup_EtlXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlTest");

        Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.InstanceOf<EtlRunXml>());
        var etl = ts.Tests[testNr].Setup.Commands[0] as EtlRunXml;

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl!.Server, Is.Null.Or.Empty);
        Assert.That(etl.Path, Is.EqualTo("/Etl/"));
        Assert.That(etl.Name, Is.EqualTo("Sample.dtsx"));
        Assert.That(etl.Password, Is.EqualTo("p@ssw0rd"));
    }

    [Test]
    public void Deserialize_EtlFromSqlServerInSetup_EtlXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlTest");

        Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.InstanceOf<EtlRunXml>());
        var etl = ts.Tests[testNr].Setup.Commands[0] as EtlRunXml;

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl!.Server, Is.EqualTo("."));
        Assert.That(etl.Path, Is.EqualTo(@"Etl\"));
        Assert.That(etl.Name, Is.EqualTo("Sample"));
    }

    [Test]
    public void Deserialize_EtlFromFileInSystemUnderTest_EtlXml()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl!.Path, Is.EqualTo(@"/Etl/"));
        Assert.That(etl.Name, Is.EqualTo("Sample.dtsx"));
    }

    [Test]
    public void Deserialize_EtlFromSqlServerInSystemUnderTest_EtlXml()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl!.Server, Is.EqualTo("."));
        Assert.That(etl.Path, Is.EqualTo(@"Etl\"));
        Assert.That(etl.Name, Is.EqualTo("Sample"));
    }

    [Test]
    public void Deserialize_WithParameters_EtlXml()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);
        var parameters = etl.Parameters;

        Assert.That(parameters, Is.Not.Null);
        Assert.That(parameters, Has.Count.EqualTo(2));
        Assert.That(parameters.Any(x => x.Name == "param1" && x.StringValue == "value1"));
        Assert.That(parameters.Any(x => x.Name == "param2" && x.StringValue == "value2"));
    }

    [Test]
    public void Deserialize_SetupWithParameters_EtlXml()
    {
        int testNr = 5;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlTest");

        Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.InstanceOf<EtlRunXml>());
        var etl = (EtlRunXml)ts.Tests[testNr].Setup.Commands[0]!;
        var parameters = etl.Parameters;

        Assert.That(parameters, Is.Not.Null);
        Assert.That(parameters, Has.Count.EqualTo(2));
        Assert.That(parameters.Any(x => x.Name == "param1" && x.StringValue == "value1"));
        Assert.That(parameters.Any(x => x.Name == "param2" && x.StringValue == "value2"));
    }

    [Test]
    public void Deserialize_FromSqlServerWithSqlServerAutentication_EtlXml()
    {
        int testNr = 6;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl.Server, Is.EqualTo("."));
        Assert.That(etl.Path, Is.EqualTo(@"/Etl/"));
        Assert.That(etl.Name, Is.EqualTo("Sample"));
        Assert.That(etl.UserName, Is.EqualTo(@"sa"));
        Assert.That(etl.Password, Is.EqualTo("p@ssw0rd"));

    }

    [Test]
    public void Deserialize_FromCatalog_EtlXml()
    {
        int testNr = 7;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl.Server, Is.EqualTo("."));
        Assert.That(etl.Catalog, Is.EqualTo(@"SSISDB"));
        Assert.That(etl.Folder, Is.EqualTo(@"Folder"));
        Assert.That(etl.Project, Is.EqualTo(@"Project"));
        Assert.That(etl.Name, Is.EqualTo("Sample"));
        Assert.That(etl.Is32Bits, Is.False);

    }

    [Test]
    public void Deserialize_FromCatalogWith32Bits_EtlXml()
    {
        int testNr = 8;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl.Is32Bits, Is.True);
    }

    [Test]
    public void Deserialize_FromCatalogWithEnvironment_EtlXml()
    {
        int testNr = 9;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl.Environment, Is.EqualTo("Environment"));
    }

    [Test]
    public void Deserialize_FromDefaultSetup_EtlXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlWithDefaultTest");

        Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.InstanceOf<EtlRunXml>());
        var etl = (EtlRunXml)ts.Tests[testNr].Setup.Commands[0];

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl!.Server, Is.EqualTo("."));
        Assert.That(etl.Environment, Is.EqualTo("Environment"));
        Assert.That(etl.Path, Is.EqualTo("/Etl/"));
        Assert.That(etl.Name, Is.EqualTo("Sample"));
        //Assert.That(etl.Version, Is.EqualTo("SqlServer2012"));
        //Assert.That(etl.Is32Bits, Is.True);
        //Assert.That(etl.Timeout, Is.EqualTo(30));
    }

    [Test]
    public void Deserialize_FromDefaultSut_EtlXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlWithDefaultTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl.Server, Is.EqualTo("localhost"));
        Assert.That(etl.Environment, Is.EqualTo("EnvironmentOverride"));
        Assert.That(etl.Path, Is.EqualTo("/Etl/"));
        Assert.That(etl.Name, Is.EqualTo("Sample"));
        //Assert.That(etl.Version, Is.EqualTo("SqlServer2014"));
        //Assert.That(etl.Is32Bits, Is.False);
        //Assert.That(etl.Timeout, Is.EqualTo(60));
    }

    [Test]
    public void Deserialize_FromReference_EtlXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlWithReferenceTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl.Server, Is.EqualTo("127.0.0.1"));
        Assert.That(etl.Environment, Is.EqualTo("Environment"));
    }

    [Test]
    public void Deserialize_FromReferenceAndOverride_EtlXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlWithReferenceTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl.Server, Is.EqualTo("127.0.0.1"));
        Assert.That(etl.Environment, Is.EqualTo("EnvironmentOverride"));
    }

    [Test]
    public void Deserialize_FromReferenceWithoutVersion_EtlXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlWithReferenceTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl.Version, Is.EqualTo("SqlServer2014"));
    }

    [Test]
    public void Deserialize_FromReferenceWithVersion_EtlXml()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlWithReferenceTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl.Version, Is.EqualTo("SqlServer2012"));
    }

    [Test]
    public void Deserialize_FromDefaultSsiSB_EtlXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlWithDefaultSsisDBTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl.Version, Is.EqualTo("SqlServer2014"));
        Assert.That(etl.Server, Is.EqualTo("localhost"));
        Assert.That(etl.Catalog, Is.EqualTo("SSISDB"));
        Assert.That(etl.Folder, Is.EqualTo("Folder"));
        Assert.That(etl.Project, Is.EqualTo("Project"));
        Assert.That(etl.Name, Is.EqualTo("Name"));
        Assert.That(etl.Path, Is.Null.Or.Empty);
    }

    public void Deserialize_FromReferenceSsiSB_EtlXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("EtlXmlWithDefaultSsisDBTest");

        Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
        var etl = (EtlXml)(ts.Tests[testNr].Systems[0].BaseItem!);

        Assert.That(etl, Is.Not.Null);
        Assert.That(etl.Version, Is.EqualTo("SqlServer2014"));
        Assert.That(etl.Server, Is.EqualTo("127.0.0.1"));
        Assert.That(etl.Catalog, Is.EqualTo("SSISDB"));
        Assert.That(etl.Folder, Is.EqualTo("FolderRef"));
        Assert.That(etl.Project, Is.EqualTo("ProjectRef"));
        Assert.That(etl.Name, Is.EqualTo("NameRef"));
        Assert.That(etl.Path, Is.Null.Or.Empty);
    }
}
