using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit.Items;

[TestFixture]
public class SharedDatasetXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SharedDatasetWithEverythingDefined_SharedDatasetXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<SharedDatasetXml>());
        var sharedDataset = (SharedDatasetXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(sharedDataset, Is.Not.Null);
        Assert.That(sharedDataset.Source, Is.EqualTo(@"Data Source=(local)\SQL2017;Initial Catalog=ReportServer;Integrated Security=True;"));
        Assert.That(sharedDataset.Path, Is.EqualTo("/AdventureWorks Sample Reports/"));
        Assert.That(sharedDataset.Name, Is.EqualTo("EmpSalesMonth"));
        Assert.That(sharedDataset.ConnectionString, Is.EqualTo(@"Data Source=tadam;Initial Catalog=AdventureWorks2012;User Id=sqlfamily;password=sqlf@m1ly"));
    }

    [Test]
    public void Deserialize_SharedDatasetWithoutConnectionString_SharedDatasetXmlUsingDefaultConnectionString()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<SharedDatasetXml>());
        var sharedDataset = (SharedDatasetXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(sharedDataset.ConnectionString, Is.Null.Or.Empty);
    }

    [Test]
    public void Deserialize_SharedDatasetWithTwoParameters_SharedDatasetXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<SharedDatasetXml>());
        var sharedDataset = (SharedDatasetXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(sharedDataset.Parameters, Is.Not.Null);
        Assert.That(sharedDataset.Parameters, Has.Count.EqualTo(2));
    }

    [Test]
    public void Deserialize_SharedDatasetWithReference_SharedDatasetXml()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<SharedDatasetXml>());
        var sharedDataset = (SharedDatasetXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(sharedDataset, Is.Not.Null);
        Assert.That(sharedDataset.Source, Is.EqualTo(@"http://reports.com/reports"));
        Assert.That(sharedDataset.Path, Is.EqualTo("Dashboard"));
        Assert.That(sharedDataset.Name, Is.EqualTo("EmpSalesMonth"));
        Assert.That(sharedDataset.ConnectionString, Is.Null.Or.Empty);
    }

    [Test]
    public void Deserialize_SharedDatasetWithDefault_SharedDatasetXml()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<SharedDatasetXml>());
        var sharedDataset = (SharedDatasetXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(sharedDataset, Is.Not.Null);
        Assert.That(sharedDataset.Source, Is.EqualTo(@"http://new.reports.com/reports"));
        Assert.That(sharedDataset.Path, Is.EqualTo("Details"));
        Assert.That(sharedDataset.Name, Is.EqualTo("EmpSalesMonth"));
        Assert.That(sharedDataset.ConnectionString, Is.Null.Or.Empty);
    }

    [Test]
    public void Deserialize_SharedDatasetWithMixDefaultReference_SharedDatasetXml()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<SharedDatasetXml>());
        var sharedDataset = (SharedDatasetXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(sharedDataset, Is.Not.Null);
        Assert.That(sharedDataset.Source, Is.EqualTo(@"http://new.reports.com/reports"));
        Assert.That(sharedDataset.Path, Is.EqualTo("alternate"));
        Assert.That(sharedDataset.Name, Is.EqualTo("EmpSalesMonth"));
        Assert.That(sharedDataset.ConnectionString, Is.Null.Or.Empty);
    }
}
