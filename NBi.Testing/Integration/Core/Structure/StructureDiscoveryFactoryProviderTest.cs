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
using System.Xml;

namespace NBi.Testing.Integration.Core.Structure
{
    public class StructureDiscoveryFactoryProviderTest
    {
        private class FakeStructureDiscoveryFactoryProvider : StructureDiscoveryFactoryProvider
        {
            public new string InquireFurtherAnalysisService(string connectionString)
            {
                return base.InquireFurtherAnalysisService(connectionString);
            }
        }

        [Test]
        [Category("Olap")]
        public void InquireFurtherAnalysisService_Multidimensional_ReturnCorrectServerMode()
        {
            var connectionString = ConnectionStringReader.GetAdomd();

            var provider = new FakeStructureDiscoveryFactoryProvider();
            var serverMode = provider.InquireFurtherAnalysisService(connectionString);
            Assert.That(serverMode, Is.EqualTo("olap"));
        }

        [Test]
        [Category("Tabular")]
        public void InquireFurtherAnalysisService_Tabular_ReturnCorrectServerMode()
        {
            var connectionString = ConnectionStringReader.GetAdomdTabular();

            var provider = new FakeStructureDiscoveryFactoryProvider();
            var serverMode = provider.InquireFurtherAnalysisService(connectionString);
            Assert.That(serverMode, Is.EqualTo("tabular"));
        }
    }
}
