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
public class ReportXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_ReportWithEverythingDefined_ReportXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<ReportXml>());
        var report = (ReportXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(report, Is.Not.Null);
        Assert.That(report.Source, Is.EqualTo(@"Data Source=(local)\SQL2017;Initial Catalog=ReportServer;Integrated Security=True;"));
        Assert.That(report.Path, Is.EqualTo("/AdventureWorks Sample Reports/"));
        Assert.That(report.Name, Is.EqualTo("Currency_List"));
        Assert.That(report.Dataset, Is.EqualTo("currency"));
        Assert.That(report.ConnectionString, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void Deserialize_ReportWithoutConnectionString_ReportXmlUsingDefaultConnectionString()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<ReportXml>());
        var report = (ReportXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(report.ConnectionString, Is.Null.Or.Empty);
    }

    [Test]
    public void Deserialize_ReportWithTwoParameters_ReportXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<ReportXml>());
        var report = (ReportXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(report.Parameters, Is.Not.Null);
        Assert.That(report.Parameters, Has.Count.EqualTo(2));
    }

    [Test]
    public void Deserialize_ReportWithReference_ReportXml()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<ReportXml>());
        var report = (ReportXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(report, Is.Not.Null);
        Assert.That(report.Source, Is.EqualTo(@"http://reports.com/reports"));
        Assert.That(report.Path, Is.EqualTo("Dashboard"));
        Assert.That(report.Name, Is.EqualTo("Currency Rates"));
        Assert.That(report.Dataset, Is.EqualTo("DataSet1"));
        Assert.That(report.ConnectionString, Is.Null.Or.Empty);
    }

    [Test]
    public void Deserialize_ReportWithDefault_ReportXml()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<ReportXml>());
        var report = (ReportXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(report, Is.Not.Null);
        Assert.That(report.Source, Is.EqualTo(@"http://new.reports.com/reports"));
        Assert.That(report.Path, Is.EqualTo("Details"));
        Assert.That(report.Name, Is.EqualTo("Currency Rates"));
        Assert.That(report.Dataset, Is.EqualTo("DataSet1"));
        Assert.That(report.ConnectionString, Is.Null.Or.Empty);
    }

    [Test]
    public void Deserialize_ReportWithMixDefaultReference_ReportXml()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<ReportXml>());
        var report = (ReportXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

        Assert.That(report, Is.Not.Null);
        Assert.That(report.Source, Is.EqualTo(@"http://new.reports.com/reports"));
        Assert.That(report.Path, Is.EqualTo("alternate"));
        Assert.That(report.Name, Is.EqualTo("Currency Rates"));
        Assert.That(report.Dataset, Is.EqualTo("DataSet1"));
        Assert.That(report.ConnectionString, Is.Null.Or.Empty);
    }
}
