using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;
using Moq;
using NBi.Core.Etl;
using NBi.Core.Etl.IntegrationService;
using NBi.Xml.Items;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Etl.IntegrationService
{
    [TestFixture]
    public class EtlDtsWindowsRunnerTest
    {
        
        private static bool isIntegrationServiceStarted = false;

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {
            isIntegrationServiceStarted = CheckIfIntegrationServiceStarted();

            if (!isIntegrationServiceStarted)
                return;

            //Build the fullpath for the file to read
            Directory.CreateDirectory("ETL");
            var pkg = DiskOnFile.CreatePhysicalFile(@"Etl\Sample.dtsx", "NBi.Testing.Integration.Core.Etl.IntegrationService.Resources.Sample.dtsx");

            try
            {
                //Move the Etl to SQL Server Integration Services
                Application app = new Application();
                Package p = app.LoadPackage(pkg, null);

                // Save the package to the SQL Server msdb folder, which is
                // also the MSDB folder in the Integration Services service, or as a row in the
                //sysssispackages table.
                app.SaveToSqlServerAs(p, null, "nbi\\nbi-sample", ConnectionStringReader.GetIntegrationServerDatabase(), null, null);
            }
            catch (Exception ex)
            {
                isIntegrationServiceStarted = false;
                IgnoreMessage=string.Format("Test fixture 'EtlDtsWindowsRunnerTest' is skipped: {0}", ex.Message);
            }
            
        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
            if (!isIntegrationServiceStarted)
                Assert.Ignore();
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }

        private bool CheckIfIntegrationServiceStarted()
        {
            var pname = Process.GetProcesses().Where(p => p.ProcessName.Contains("MsDtsSrvr"));
            IgnoreMessage = "Integration Service not started.";
            return pname.Count() > 0;
        }
        #endregion

        [Test]
        [Category("Integration Service")]
        public void Execute_ExistingDataCollectorPackage_Failure()
        {
            var etl = Mock.Of<IEtl>( e =>
                e.Server == "."
                && e.Path == string.Empty
                && e.Name == @"MSDB\Data Collector\PerfCountersCollect"
                && e.Parameters == new List<EtlParameter>()
                );
            
            var runner = new EtlDtsWindowsRunner(etl);
            var result = runner.Run();

            Assert.That(result.IsSuccess, Is.False);
        }

        public string IgnoreMessage { get; set; }
    }
}
