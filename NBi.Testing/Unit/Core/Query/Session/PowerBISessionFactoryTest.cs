using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.PowerBiDesktop;
using NBi.Core.Query.Session;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Query.Session
{
    [TestFixture]
    public class PowerBISessionFactoryTest
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
            var factory = new PowerBiDesktopSessionFactory(new PowerBiDesktopConnectionStringBuilderFake());
            var actual = factory.Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<PowerBiDesktopSession>());
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<AdomdConnection>());
            Assert.That((conn as AdomdConnection).ConnectionString, Is.EqualTo(PowerBiDesktopConnectionStringBuilderFake.ConnectionString));
        }
    }
}
