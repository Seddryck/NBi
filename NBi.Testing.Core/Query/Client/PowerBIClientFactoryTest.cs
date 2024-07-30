using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.PowerBiDesktop;
using NBi.Core.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Query.Client
{
    [TestFixture]
    public class PowerBIClientFactoryTest
    {

        #region Power BI Desktop

        private class PowerBiDesktopConnectionStringBuilderFake : PowerBiDesktopConnectionStringBuilder
        {
            public static string ConnectionString = "Data Source=localhost:2325;";
            protected override string BuildLocalConnectionString(string name)
            {
                return ConnectionString;
            }
        }

        #endregion

        [Test]
        public void Get_PowerBiDesktop_AdommdConnection()
        {
            //Call the method to test
            var connStr = "PBIX=My Power BI Desktop;";
            var factory = new PowerBiDesktopClientFactory(new PowerBiDesktopConnectionStringBuilderFake());
            var actual = factory.Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<PowerBiDesktopClient>());
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<AdomdConnection>());
            Assert.That(((AdomdConnection)conn).ConnectionString, Is.EqualTo(PowerBiDesktopConnectionStringBuilderFake.ConnectionString));
        }
    }
}
