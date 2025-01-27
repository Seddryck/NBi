using NBi.Core.Structure;
using NBi.Core.Structure.Olap;
using NBi.Core.Structure.Relational;
using NBi.Core.Structure.Tabular;
using NBi.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NBi.Core.Testing.Structure;

public class StructureDiscoveryFactoryProviderTest
{
    private class FakeStructureDiscoveryFactoryProvider : StructureDiscoveryFactoryProvider
    {
        private readonly string result;

        public FakeStructureDiscoveryFactoryProvider(string result)
            : base()
        {
            this.result = result;
        }

        protected override string InquireFurtherAnalysisService(string connectionString)
        {
            return result;
        }

        public new string ParseXmlaResponse(XmlDocument doc)
        {
            return base.ParseXmlaResponse(doc);
        }
    }

    [Test]
    public void Instantiate_EmptyConnectionString_GetDatabaseStructureDiscoveryFactory()
    {
        var connectionString = string.Empty;

        var provider = new StructureDiscoveryFactoryProvider();
        Assert.Throws<ArgumentNullException>(() => provider.Instantiate(connectionString));
    }

    [Test]
    public void Instantiate_SqlConnection_GetDatabaseStructureDiscoveryFactory()
    {
        var connectionString = ConnectionStringReader.GetSqlClient();
        
        var provider = new StructureDiscoveryFactoryProvider();
        var factory = provider.Instantiate(connectionString);
        Assert.That(factory, Is.TypeOf<RelationalStructureDiscoveryFactory>());
    }

    [Test]
    public void Instantiate_AdomdConnectionOlap_GetDatabaseStructureDiscoveryFactory()
    {
        var connectionString = ConnectionStringReader.GetAdomd();

        var provider = new FakeStructureDiscoveryFactoryProvider(StructureDiscoveryFactoryProvider.Olap);
        var factory = provider.Instantiate(connectionString);
        Assert.That(factory, Is.TypeOf<OlapStructureDiscoveryFactory>());
    }

    [Test]
    public void Instantiate_AdomdConnectionTabular_GetDatabaseStructureDiscoveryFactory()
    {
        var connectionString = ConnectionStringReader.GetAdomd();

        var provider = new FakeStructureDiscoveryFactoryProvider(StructureDiscoveryFactoryProvider.Tabular);
        var factory = provider.Instantiate(connectionString);
        Assert.That(factory, Is.TypeOf<TabularStructureDiscoveryFactory>());
    }

    
    [Test]
    [TestCase("Multidimensional")]
    [TestCase("Tabular")]
    [TestCase("Sharepoint")]
    [TestCase("Default")]
    public void ParseXmlaResponse_Tabular_GetCorrectServerMode(string serverMode)
    {
        var xml = ""
                    + "<Server xmlns=\"http://schemas.microsoft.com/analysisservices/2003/engine\">                                                                                                    "
                    + "            <Name>XXX\\SQL2014</Name>                                                                           "
                    + "            <ID>XXX\\SQL2014</ID>                                                                               "
                    + "            <CreatedTimestamp>2015-07-02T21:56:04.076667</CreatedTimestamp>                                         "
                    + "            <LastSchemaUpdate>2015-07-02T21:56:04.093333</LastSchemaUpdate>                                         "
                    + "            <Version>12.0.2000.8</Version>                                                                          "
                    + "            <Edition>Developer64</Edition>                                                                          "
                    + "            <EditionID>2176971986</EditionID>                                                                       "
                    + "            <ddl300:ServerMode xmlns:ddl300=\"http://schemas.microsoft.com/analysisservices/2011/engine/300\">$value$</ddl300:ServerMode>"
                    + "            <ddl400:ServerLocation xmlns:ddl400=\"http://schemas.microsoft.com/analysisservices/2012/engine/400\">OnPremise</ddl400:ServerLocation>"
                    + "            <ddl400:DefaultCompatibilityLevel xmlns:ddl400=\"http://schemas.microsoft.com/analysisservices/2012/engine/400\">1100</ddl400:DefaultCompatibilityLevel>"
                    + "</Server>                                                                                                   ";
        xml = xml.Replace("$value$", serverMode);
        var doc = new XmlDocument();
        
        doc.LoadXml(xml);
        var provider = new FakeStructureDiscoveryFactoryProvider(string.Empty);
        var parsedServerMode = provider.ParseXmlaResponse(doc);
        Assert.That(parsedServerMode, Is.EqualTo(serverMode));
    }

    [Test]
    [TestCase("10.0.200.12")]
    [TestCase("9.1.200")]
    public void ParseXmlaResponse_VersionBefore11_GetCorrectServerMode(string version)
    {
        var xml = ""
                    + "<Server xmlns=\"http://schemas.microsoft.com/analysisservices/2003/engine\">                                                                                                    "
                    + "            <Name>XXX\\SQL2014</Name>                                                                           "
                    + "            <ID>XXX\\SQL2014</ID>                                                                               "
                    + "            <CreatedTimestamp>2015-07-02T21:56:04.076667</CreatedTimestamp>                                         "
                    + "            <LastSchemaUpdate>2015-07-02T21:56:04.093333</LastSchemaUpdate>                                         "
                    + "            <Version>$value$</Version>                                                                          "
                    + "            <Edition>Developer64</Edition>                                                                          "
                    + "            <EditionID>2176971986</EditionID>                                                                       "
                    + "</Server>                                                                                                   ";
        xml = xml.Replace("$value$", version);
        var doc = new XmlDocument();

        doc.LoadXml(xml);
        var provider = new FakeStructureDiscoveryFactoryProvider(string.Empty);
        var parsedServerMode = provider.ParseXmlaResponse(doc);
        Assert.That(parsedServerMode, Is.EqualTo("Multidimensional"));
    }

    [Test]
    [TestCase("12.0.200.12")]
    [TestCase("11.1.200")]
    public void ParseXmlaResponse_VersionAfter11_ThrowExceptionImpossibleToGuess(string version)
    {
        var xml = ""
                    + "<Server xmlns=\"http://schemas.microsoft.com/analysisservices/2003/engine\">                                                                                                    "
                    + "            <Name>XXX\\SQL2014</Name>                                                                           "
                    + "            <ID>XXX\\SQL2014</ID>                                                                               "
                    + "            <CreatedTimestamp>2015-07-02T21:56:04.076667</CreatedTimestamp>                                         "
                    + "            <LastSchemaUpdate>2015-07-02T21:56:04.093333</LastSchemaUpdate>                                         "
                    + "            <Version>$value$</Version>                                                                          "
                    + "            <Edition>Developer64</Edition>                                                                          "
                    + "            <EditionID>2176971986</EditionID>                                                                       "
                    + "</Server>                                                                                                   ";
        xml = xml.Replace("$value$", version);
        var doc = new XmlDocument();

        doc.LoadXml(xml);
        var provider = new FakeStructureDiscoveryFactoryProvider(string.Empty);
        Assert.Throws<ArgumentException>(delegate { provider.ParseXmlaResponse(doc); });
    }
}
