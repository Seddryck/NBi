using NBi.Core.Structure;
using NBi.Core.Structure.Olap;
using NBi.Core.Structure.Relational;
using NBi.Core.Structure.Tabular;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Structure
{
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
    }
}
